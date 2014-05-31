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
<<<<<<< HEAD
		public const decimal EPSILON = 0.00000001M;
=======

		// + - * / mod pow
		public static Operation add = new Operation().addNames("+", "плюс").setPriority(2).setOperation((a, b) => a + b);
		public static Operation sub = new Operation().addNames("-", "минус").setPriority(2).setOperation((a, b) => a - b);
		public static Operation multi = new Operation().addNames("*", "умножить").setPriority(1).setOperation((a, b) => a * b);
		public static Operation div = new Operation().addNames("/", "разделить").setPriority(1).setOperation((a, b) => a / b);
		public static Operation mod = new Operation().addNames("%", "mod", "остаток от деления").setPriority(1).setOperation((a, b) => a % b);
		public static Operation pow = new Operation().addNames("^", "встепени").setPriority(2).setOperation((a, b) => (decimal) Math.Pow((double) a, (double) b));

		// + - log ln abs, round, sign, sqrt, sqr
		public static Operation addUnary = new Operation().addNames("+").setPriority(0).setOperation((decimal a, decimal b) => +b).setUnary(true);
		public static Operation subUnary = new Operation().addNames("-").setPriority(0).setOperation((decimal a, decimal b) => -b).setUnary(true);
		public static Operation log = new Operation().addNames("log", "десятичныйЛогарифм").setOperation((a, b) => (decimal) Math.Log10((double) b)).setUnary(true);
		public static Operation ln = new Operation().addNames("ln", "натуральныйЛогарифм").setOperation((a, b) => (decimal) Math.Log((double) b)).setUnary(true);
		public static Operation abs = new Operation().addNames("abs", "модуль").setOperation((a, b) => (decimal) Math.Abs((double) b)).setUnary(true);
		public static Operation round = new Operation().addNames("round", "округлить").setOperation((a, b) => (decimal) Math.Round(b)).setUnary(true);
		public static Operation sign = new Operation().addNames("sign", "знак").setOperation((a, b) => (decimal) Math.Sign((double) b)).setUnary(true);
		public static Operation sqrt = new Operation().addNames("sqrt", "корень").setOperation((a, b) => (decimal) Math.Sqrt((double) b)).setUnary(true);
		public static Operation sqr = new Operation().addNames("sqr", "квадрат").setOperation((a, b) => b * b).setUnary(true);

		// sin cos tg ctg asin acos atg actg, sinh, cosh, tgh, ctgh
		public static Operation sin = new Operation().addNames("sin", "синус").setOperation((a, b) => (decimal) Math.Sin((double) b)).setUnary(true);
		public static Operation cos = new Operation().addNames("cos", "косинус").setOperation((a, b) => (decimal) Math.Cos((double) b)).setUnary(true);
		public static Operation tg = new Operation().addNames("tg", "тангенс").setOperation((a, b) => (decimal) Math.Tan((double) b)).setUnary(true);
		public static Operation ctg = new Operation().addNames("ctg", "котангенс").setOperation((a, b) => 1 / ((decimal) Math.Tan((double) b))).setUnary(true);
		public static Operation aSin = new Operation().addNames("aSin", "арксинус").setOperation((a, b) => (decimal) Math.Asin((double) b)).setUnary(true);
		public static Operation aCos = new Operation().addNames("aCos", "арккосинус").setOperation((a, b) => (decimal) Math.Acos((double) b)).setUnary(true);
		public static Operation aTg = new Operation().addNames("aTg", "арктангенс").setOperation((a, b) => (decimal) Math.Atan((double) b)).setUnary(true);
		public static Operation aCtg = new Operation().addNames("aCtg", "арккотангенс").setOperation((a, b) => 1 / ((decimal) (Math.PI / 2 - Math.Atan((double) b)))).setUnary(true);
		public static Operation sinh = new Operation().addNames("sinh", "гиперболический синус").setOperation((a, b) => (decimal) Math.Sinh((double) b)).setUnary(true);
		public static Operation cosh = new Operation().addNames("cosh", "гиперболический косинус").setOperation((a, b) => (decimal) Math.Cosh((double) b)).setUnary(true);
		public static Operation tgh = new Operation().addNames("tgh", "гиперболический тангенс").setOperation((a, b) => (decimal) Math.Tanh((double) b)).setUnary(true);
		public static Operation ctgh = new Operation().addNames("ctgh", "гиперболический котангенс").setOperation((a, b) => 1 / ((decimal) Math.Tanh((double) b))).setUnary(true);

		public Operation[] operations = {
											add, sub, multi, div, mod, pow, 
											addUnary, subUnary, log, ln, abs, round, sign, sqrt, sqr, 
											sin, cos, tg, ctg, aSin, aCos, aTg, aCtg, sinh, cosh, tgh, ctgh
										};

		enum TypePart
		{
			OPERATION,
			NUMBER
		}
>>>>>>> 06c9446237ad16595adccae1c792b66bbc983ffe

		public decimal ans = 0.0M;

		public Calculator()
		{
			Math.Round(5.0);
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
						string inBracketAnswer = " " + new SimpleCalculator().calculate(inBracketQuestion) + " ";
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

<<<<<<< HEAD
=======
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
					Operation operation;
					if (i == 0 || partsType[i - 1] == TypePart.OPERATION)
					{
						operation = getFromAllOperations(op => op.names.Contains(partsString[i]) && op.isUnary);
						priorities[operation.priority].Insert(0, i);
					}
					else
					{
						operation = getFromAllOperations(op => op.names.Contains(partsString[i]) && !op.isUnary);
						priorities[operation.priority].Add(i);
					}
					// test
					//priorities[operation.priority].Add(i);
				}
				order.Add(i);
			}

			for (int i = 0; i < priorities.Count; i++)
			{
				for (int j = 0; j < priorities[i].Count; j++)
				{
					int index = priorities[i][j];
					int indexOperation = order.FindIndex(x => x == index);
					//Operation operation = getOperationFromStart(partsString[index]);
					if (index == 0 || partsType[index - 1] == TypePart.OPERATION)
					{
						Operation operation = getFromAllOperations(op => op.names.Contains(partsString[index]) && op.isUnary);
						if (!operation.isUnary && operation != Calculator.sub)
							throw new Exception();
						partsNumber[index] = operation.opeartion(0.0M, partsNumber[order[indexOperation + 1]]);
						order.RemoveAt(indexOperation + 1);
					}
					else
					{
						Operation operation = getFromAllOperations(op => op.names.Contains(partsString[index]) && !op.isUnary);
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

		public delegate bool check(Operation operation);

		private Operation getFromAllOperations(check check)
		{
			for (int i = 0; i < operations.Length; i++)
			{
				if (check(operations[i]))
				{
					return operations[i];
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


>>>>>>> 06c9446237ad16595adccae1c792b66bbc983ffe
	}
}
