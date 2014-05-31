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
			calculator = new Calculator();

			//	Вначале юнит тесты
			Program.unitTests();
			Console.Write("\n\n------------\nИтого все тесты: {0}\n\n", allIsGood ? "ОК" : "ПЛОХО");

			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			//new Program().generate();
			new Program().run();
			stopWatch.Stop();

			long frequency = Stopwatch.Frequency;
			long ts = 1000000L * stopWatch.ElapsedTicks / frequency;
			Console.WriteLine("ОК {0} мс", ts / 1000.0);
			Console.ReadKey();

			for (int i = 0; i < question.Count; i++)
				createUnitTest(question[i], answer[i]);
		}

		public static Calculator calculator;
		public static bool allIsGood = true;
		public static List<string> question = new List<string>();
		public static List<decimal> answer = new List<decimal>();

		private void run()
		{
			//	Потом пользовательский ввод
			using (StreamReader input = new StreamReader("input.txt"))
			{
				while (!input.EndOfStream)
				{
					string currentQuestion = input.ReadLine();
					decimal currentAnswer = calculator.calculate(currentQuestion);
					currentAnswer = decimal.Round(currentAnswer, 7);

					Console.WriteLine("{0, 30} = {1, 20}", currentQuestion, currentAnswer);

					question.Add(currentQuestion);
					answer.Add(currentAnswer);
				}
			}
		}

		public const string unitTestPath = "UnitTests/";

		private static void unitTests()
		{
			if (!Directory.Exists(unitTestPath))
				return;

			//Process currentProcess = Process.GetCurrentProcess();
			//currentProcess.StartInfo.UseShellExecute = false;
			//currentProcess.StartInfo.RedirectStandardInput = true;
			//currentProcess.StartInfo.RedirectStandardOutput = true;
			//currentProcess.StandardInput = input;
			//currentProcess.StandardOutput = output;

			int i = 1;
			while (File.Exists(unitTestPath + i + ".in"))
			{
				StreamReader inputQuestion = new StreamReader(unitTestPath + i + ".in");
				StreamReader inputAnswer = new StreamReader(unitTestPath + i + ".out");

				string question = inputQuestion.ReadLine();
				string answer = inputAnswer.ReadLine();

				string answer1 = calculator.calculate(question).ToString();
				Console.Write("{0} = {1} ", question, answer);
				if (answer == answer1)
					Console.WriteLine("ОК");
				else
				{
					Console.WriteLine("ПЛОХО, != {0}", answer1);
					allIsGood = false;
				}

				i++;
			}
		}

		private static void createUnitTest(string question, decimal answer)
		{
			string pathIn;
			string pathOut;

			int i = 1;
			if (!Directory.Exists(unitTestPath))
				Directory.CreateDirectory(unitTestPath);
			else
			{
				while (File.Exists(unitTestPath + i + ".in"))
				{
					pathIn = unitTestPath + i + ".in";
					pathOut = unitTestPath + i + ".out";

					string questionI, answerI;
					using (StreamReader input = new StreamReader(pathIn))
						questionI = input.ReadLine();
					using (StreamReader input = new StreamReader(pathOut))
						answerI = input.ReadLine();

					if ((question == questionI) && (answer.ToString() == answerI))
						return;

					i++;
				}
			}

			pathIn = unitTestPath + i + ".in";
			pathOut = unitTestPath + i + ".out";

			using (StreamWriter output = new StreamWriter(pathIn))
				output.Write(question);
			using (StreamWriter output = new StreamWriter(pathOut))
				output.Write(answer);
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
					output.Close();
				}

			}
		}

	}
}
