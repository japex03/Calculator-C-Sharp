using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{

	class SimpleCalculator
	{

		enum TypePart
		{
			OPERATION,
			OPERATION_BINARY,
			OPERATION_UNARY,
			OPERATION_POST_UNARY,
			NUMBER
		}

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
			List<TypePart> partsType = new List<TypePart>();
			List<int> partsOrder = new List<int>();
			int partsCount = partsString.Count;

			for (int i = 0; i < partsCount; i++)
			{
				decimal x = 0.0M;
				partsType.Add(decimal.TryParse(partsString[i], out x) ? TypePart.NUMBER : TypePart.OPERATION);
				partsNumber.Add(x);
				partsOrder.Add(i);
			}

			List<List<int>> priorities = new List<List<int>>(Calculator.MAX_PRIORITY);
			for (int i = 0; i < Calculator.MAX_PRIORITY; i++)
			{
				priorities.Add(new List<int>());
			}

			for (int i = 0; i < partsCount; i++)
			{
				if (partsType[i] == TypePart.OPERATION)
				{
					Operation operationUnary = getFromAllOperations(op => op.names.Contains(partsString[i]) && op.IsUnary);
					Operation operationPostUnary = getFromAllOperations(op => op.names.Contains(partsString[i]) && op.IsPostUnary);
					Operation operationBinary = getFromAllOperations(op => op.names.Contains(partsString[i]) && !(op.IsUnary || op.IsPostUnary));

					if (operationUnary != null && ((operationPostUnary == null && operationBinary == null) || i == 0 || partsType[i - 1] == TypePart.OPERATION_BINARY || partsType[i - 1] == TypePart.OPERATION_UNARY))
					{
						priorities[operationUnary.priority].Insert(0, i);
						partsType[i] = TypePart.OPERATION_UNARY;
					}
					else if (operationBinary != null && ((operationUnary == null && operationPostUnary == null) || (partsType[i - 1] == TypePart.NUMBER || partsType[i - 1] == TypePart.OPERATION_POST_UNARY)))
					{
						priorities[operationBinary.priority].Add(i);
						partsType[i] = TypePart.OPERATION_BINARY;
					}
					else if (operationPostUnary != null && ((operationUnary == null && operationBinary == null) || partsType[i - 1] == TypePart.NUMBER || partsType[i - 1] == TypePart.OPERATION_POST_UNARY))
					{
						priorities[operationPostUnary.priority].Insert(0, i);
						partsType[i] = TypePart.OPERATION_POST_UNARY;
					}
					else
						throw new Exception();
				}
			}



			for (int i = 0; i < priorities.Count; i++)
			{
				for (int j = 0; j < priorities[i].Count; j++)
				{
					int index = priorities[i][j];
					int indexOperation = partsOrder.FindIndex(x => x == index);
					if (partsType[index] == TypePart.OPERATION_UNARY)
					{
						Operation operationUnary = getFromAllOperations(op => op.names.Contains(partsString[index]) && op.IsUnary);
						partsNumber[index] = operationUnary.operation(0, partsNumber[partsOrder[indexOperation + 1]]);
						partsOrder.RemoveAt(indexOperation + 1);
					}
					else if (partsType[index] == TypePart.OPERATION_POST_UNARY)
					{
						Operation operationPostUnary = getFromAllOperations(op => op.names.Contains(partsString[index]) && op.IsPostUnary);
						partsNumber[index] = operationPostUnary.operation(partsNumber[partsOrder[indexOperation - 1]], 0);
						partsOrder.RemoveAt(indexOperation - 1);
					}
					else if (partsType[index] == TypePart.OPERATION_BINARY)
					{
						Operation operationBinary = getFromAllOperations(op => op.names.Contains(partsString[index]) && !op.IsUnary);
						int indexPrevOperation = partsOrder[indexOperation - 1];
						int indexNextOperation = partsOrder[indexOperation + 1];
						partsNumber[index] = operationBinary.operation(partsNumber[indexPrevOperation], partsNumber[indexNextOperation]);
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

		private Operation getOperationFromStart(string p)
		{
			for (int i = 0; i < Operation.operations.Length; i++)
			{
				for (int j = 0; j < Operation.operations[i].names.Count; j++)
				{
					string name = Operation.operations[i].names[j];
					if (p.StartsWith(name))
					{
						return Operation.operations[i];
					}
				}
			}
			return null;
		}

	}
}