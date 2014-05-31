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
		public const decimal EPSILON = 0.00000001M;

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

	}
}
