using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	class Program
	{
		static void Main(string[] args)
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			//new Program().generate();
			new Program().run();
			stopWatch.Stop();

			long frequency = Stopwatch.Frequency;
			long ts = 1000000L * stopWatch.ElapsedTicks / frequency;
			Console.WriteLine("ОК {0} мс", ts / 1000.0);
			Console.Read();
		}

		private void run()
		{
			using (StreamReader input = new StreamReader("input.txt"))
			//using (StreamWriter output = new StreamWriter("output.txt"))
			{
				while (!input.EndOfStream)
				{
					string question = input.ReadLine();
					decimal answer = new Calculator().calculate(question);
					answer = decimal.Round(answer, 3);
					Console.WriteLine("{0, 30} = {1, 20}", question, answer);
				}

			}
		}

		public void generate()
		{
			using (StreamWriter output = new StreamWriter("input.txt"))
			{
				Random random = new Random();
				string[] operations = { "+", "-", "*", "/", "mod" };
				int kQuestions = random.Next(5) + 5;

				for (int k = 0; k < kQuestions; k++)
				{
					int kSymbols = random.Next(1, 5) * 2 + 1;
					string question = "";
					bool isNumber = false;
					for (int i = 0; i < kSymbols; i++)
					{
						question += (isNumber ? operations[random.Next(operations.Length)] : (random.Next() % 20).ToString()) + " ";
						isNumber = !isNumber;
					}
					output.WriteLine(question);
				}

			}
		}

	}
}
