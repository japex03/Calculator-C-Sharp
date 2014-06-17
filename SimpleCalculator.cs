using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{

	public class SimpleCalculator
	{

		public string calculate(string question)
		{
			for (int i = 0; i < question.Length; i++)
			{
				int offset = startWithOperation(question.Substring(i));
				if (offset > 0)
				{
					question = question.Insert(i + offset, " ");
					question = question.Insert(i, " ");
					i += 2;
				}
			}

			question = question.Trim();
			while (question.Contains("  "))
				question = question.Replace("  ", " ");

			List<string> partsString = question.Split(' ').ToList();
			List<decimal> partsNumber = new List<decimal>();
			List<Operation> partsOperation = new List<Operation>();
			List<int> partsOrder = new List<int>();
			int partsCount = partsString.Count;

			List<List<int>> priorities = new List<List<int>>(Calculator.MAX_PRIORITY);
			for (int i = 0; i < Calculator.MAX_PRIORITY; i++)
			{
				priorities.Add(new List<int>());
			}

			for (int i = 0; i < partsCount; i++)
			{
				decimal x;
				bool f = decimal.TryParse(partsString[i], out x);
				if (!f)
				{
					double x1;
					f = double.TryParse(partsString[i], out x1);
					x = (decimal) x1;
				}

				if (!f)
				{
					Operation operationUnary = getFromAllOperations(op => op.names.Contains(partsString[i]) && op.IsUnary);
					Operation operationPostUnary = getFromAllOperations(op => op.names.Contains(partsString[i]) && op.IsPostUnary);
					Operation operationBinary = getFromAllOperations(op => op.names.Contains(partsString[i]) && op.IsBinary);

					if (operationUnary != null && ((operationPostUnary == null && operationBinary == null) || i == 0 || (partsOperation[i - 1] != null && (partsOperation[i - 1].IsBinary || partsOperation[i - 1].IsUnary))))
					{
						priorities[operationUnary.priority].Insert(0, i);
						partsOperation.Add(operationUnary);
					}
					else if (operationBinary != null && ((operationUnary == null && operationPostUnary == null) || (partsOperation[i - 1] == null || partsOperation[i - 1].IsPostUnary)))
					{
						priorities[operationBinary.priority].Add(i);
						partsOperation.Add(operationBinary);
					}
					else if (operationPostUnary != null && ((operationUnary == null && operationBinary == null) || partsOperation[i - 1] == null || partsOperation[i - 1].IsPostUnary))
					{
						priorities[operationPostUnary.priority].Add(i);
						partsOperation.Add(operationPostUnary);
					}
					else
						throw new Exception();
				}
				else
					partsOperation.Add(null);
				partsNumber.Add(x);
				partsOrder.Add(i);
			}



			for (int i = 0; i < priorities.Count; i++)
			{
				for (int j = 0; j < priorities[i].Count; j++)
				{
					int index = priorities[i][j];
					int indexOperation = partsOrder.FindIndex(x => x == index);
					if (partsOperation[index].IsUnary)
					{
						partsNumber[index] = partsOperation[index].operation(0, partsNumber[partsOrder[indexOperation + 1]]);
						partsOrder.RemoveAt(indexOperation + 1);
					}
					else if (partsOperation[index].IsPostUnary)
					{
						partsNumber[index] = partsOperation[index].operation(partsNumber[partsOrder[indexOperation - 1]], 0);
						partsOrder.RemoveAt(indexOperation - 1);
					}
					else if (partsOperation[index].IsBinary)
					{
						int indexPrevOperation = partsOrder[indexOperation - 1];
						int indexNextOperation = partsOrder[indexOperation + 1];
						partsNumber[index] = partsOperation[index].operation(partsNumber[indexPrevOperation], partsNumber[indexNextOperation]);
						partsOrder.RemoveAt(indexOperation + 1);
						partsOrder.RemoveAt(indexOperation - 1);
					}
				}
			}

			return partsNumber[partsOrder[0]].ToString();

			throw new Exception();
		}

		private int startWithOperation(string p)
		{
			for (int i = 0; i < Operation.operations.Length; i++)
			{
				for (int j = 0; j < Operation.operations[i].names.Count; j++)
				{
					string name = Operation.operations[i].names[j];
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
			for (int i = 0; i < Operation.operations.Length; i++)
			{
				if (check(Operation.operations[i]))
				{
					return Operation.operations[i];
				}
			}
			return null;
		}

	}
}