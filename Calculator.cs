using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	public class Calculator
	{

		public const int MAX_PRIORITY = 6;
		public const decimal EPSILON = 0.0000001M;

		public static Constant pi = new Constant().setNames("PI", "числоПи", "ПИ").setValue((decimal) Math.PI);
		public static Constant[] constants = { pi };

		public decimal ans = 0.0M;

		public decimal calculate(string question)
		{
			for (int i = 0; i < constants.Length; i++)
			{
				for (int j = 0; j < constants[i].names.Length; j++)
				{
					question = question.Replace(constants[i].names[j], constants[i].value.ToString());
				}
			}

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
						question = question.Remove(brackStart, brackEnd - brackStart + 1);
						question = question.Insert(brackStart, inBracketAnswer);
						break;
					}
				}
			}

			ans = decimal.Parse(question);
			return ans;
		}

	}
}
