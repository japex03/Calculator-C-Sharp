using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	class Calculator
	{

		public const int MAX_PRIORITY = 6;

		public static Operation add = new Operation().addNames("+", "плюс").setPriority(2).setOperation((a, b) => a + b);
		public static Operation sub = new Operation().addNames("-", "минус").setPriority(2).setOperation((a, b) => a - b);
		public static Operation multi = new Operation().addNames("*", "умножить").setPriority(1).setOperation((a, b) => a * b);
		public static Operation div = new Operation().addNames("/", "разделить").setPriority(1).setOperation((a, b) => a / b);
		public static Operation mod = new Operation().addNames("%", "mod", "остаток от деления").setPriority(1).setOperation((a, b) => a % b);
		public static Operation pow = new Operation().addNames("^", "встепени").setPriority(2).setOperation((a, b) => (decimal) Math.Pow((double) a, (double) b));

		//public static Operation subUnary = new Operation().addNames("-").setPriority(0).setOperation((decimal a, decimal b) => -a).setUnary(true);
		public static Operation sin = new Operation().addNames("sin", "синус").setOperation((a, b) => (decimal) Math.Sin((double) b)).setUnary(true);
		public static Operation cos = new Operation().addNames("cos", "косинус").setOperation((a, b) => (decimal) Math.Cos((double) b)).setUnary(true);
		public static Operation tg = new Operation().addNames("tg", "тангенс").setOperation((a, b) => (decimal) Math.Tan((double) b)).setUnary(true);
		public static Operation ctg = new Operation().addNames("ctg", "котангенс").setOperation((a, b) => 1 / ((decimal) Math.Tan((double) b))).setUnary(true);

		public Operation[] operations = { add, sub, multi, div, mod, pow, sin, cos, tg, ctg };

		enum TypePart
		{
			OPERATION,
			NUMBER
		}

		public decimal ans = 0.0M;

		public Calculator()
		{

		}

		public decimal calculate(string question)
		{
			question = question.Replace(" ", "");
			question = question.Replace('.', ',');
			question = '(' + question + ')';

			while (question.Contains('('))
			{
				int brackStart = -1, brackEnd = -1;
				for (int i = 0; i < question.Length; i++)
				{
					char c = question[i];
					if (c == '(')
					{
						brackStart = i;
						brackEnd = -1;
					}
					else if (c == ')')
					{
						brackEnd = i;
						string inBracketQuestion = question.Substring(brackStart + 1, brackEnd - brackStart - 1);
						string inBracketAnswer = " " + simpleCalculate(inBracketQuestion) + " ";
						//Console.Write("|{0}| ", question);
						question = question.Remove(brackStart, brackEnd - brackStart + 1);
						//Console.Write("|{0}| ", question);
						question = question.Insert(brackStart, inBracketAnswer);
						//Console.Write("|{0}| ", question);

						//Console.WriteLine("|{0}|", inBracketAnswer);
						break;
					}
				}
			}

			ans = decimal.Parse(question);
			return ans;
		}

		private string simpleCalculate(string simpleQuestion)
		{
			//int startStatIndex = 0, endStatIndex = simpleQuestion.Length;
			for (int i = 0; i < simpleQuestion.Length; i++)
			{
				int offset = startWithOperation(simpleQuestion.Substring(i));
				if (offset > 0)
				{
					simpleQuestion = simpleQuestion.Insert(i + offset, " ");
					simpleQuestion = simpleQuestion.Insert(i, " ");
					i += 2;
				}
			}

			simpleQuestion = simpleQuestion.Trim();
			while (simpleQuestion.Contains("  "))
				simpleQuestion = simpleQuestion.Replace("  ", " ");

			List<string> partsString = simpleQuestion.Split(' ').ToList();
			List<decimal> partsNumber = new List<decimal>();
			List<TypePart> partsType = new List<TypePart>();

			for (int i = 0; i < partsString.Count; i++)
			{
				decimal x = 0.0M;
				partsType.Add(decimal.TryParse(partsString[i], out x) ? TypePart.NUMBER : TypePart.OPERATION);
				partsNumber.Add(x);
			}

			List<List<int>> priorities = new List<List<int>>(Calculator.MAX_PRIORITY);
			for (int i = 0; i < Calculator.MAX_PRIORITY; i++)
			{
				priorities.Add(new List<int>());
			}


			List<int> order = new List<int>();
			for (int i = 0; i < partsString.Count; i++)
			{
				if (partsType[i] == TypePart.OPERATION)
				{
					int priority = getOperationFromStart(partsString[i]).priority;
					priorities[priority].Add(i);
				}
				order.Add(i);
			}

			for (int i = 0; i < priorities.Count; i++)
			{
				for (int j = 0; j < priorities[i].Count; j++)
				{
					int index = priorities[i][j];
					int indexOperation = order.FindIndex(x => x == index);
					Operation operation = getOperationFromStart(partsString[index]);
					if (index == 0 || partsType[index - 1] == TypePart.OPERATION)
					{
						if (!operation.isUnary && operation != Calculator.sub)
							throw new Exception();
						partsNumber[index] = operation.opeartion(0.0M, partsNumber[order[indexOperation + 1]]);
						order.RemoveAt(indexOperation + 1);
					}
					else
					{
						if (operation.isUnary)
							throw new Exception();
						int indexPrevOperation = order[indexOperation - 1];
						int indexNextOperation = order[indexOperation + 1];
						partsNumber[index] = operation.opeartion(partsNumber[indexPrevOperation], partsNumber[indexNextOperation]);
						order.RemoveAt(indexOperation + 1);
						order.RemoveAt(indexOperation - 1);
					}
				}
			}

			return partsNumber[order[0]].ToString();

			/*for (int i = priorities.Count - 1; i >= 0; i--)
			{
				for (int j = priorities[i].Count - 1; j >= 0; )
				{
					return partsNumber[priorities[i][j]].ToString();
				}
			}*/
			throw new Exception();
		}

		private int startWithOperation(string p)
		{
			for (int i = 0; i < operations.Length; i++)
			{
				for (int j = 0; j < operations[i].names.Count; j++)
				{
					string name = operations[i].names[j];
					if (p.StartsWith(name))
					{
						return name.Length;
					}
				}
			}
			return 0;
		}

		private Operation getFromAllOperations(string p)
		{
			for (int i = 0; i < operations.Length; i++)
			{
				for (int j = 0; j < operations[i].names.Count; j++)
				{
					string name = operations[i].names[j];
					if (p.StartsWith(name))
					{
						return operations[i];
					}
				}
			}
			return null;
		}

		private Operation getOperationFromStart(string p)
		{
			for (int i = 0; i < operations.Length; i++)
			{
				for (int j = 0; j < operations[i].names.Count; j++)
				{
					string name = operations[i].names[j];
					if (p.StartsWith(name))
					{
						return operations[i];
					}
				}
			}
			return null;
		}


	}
}
