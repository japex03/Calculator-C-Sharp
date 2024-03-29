﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	public class Program
	{
		static void Main(string[] args)
		{
			new Program().start();
		}

		public void start()
		{
			calculator = new Calculator();

			//	Вначале юнит тесты
			runAndDisplayTime(Program.Tests);
			Console.WriteLine();

			//	Потом ользовательский ввод
			runAndDisplayTime(this.run);
			Console.ReadKey();

			//	Создаем новые тесты на основе пользовательчкого ввода
			for (int i = 0; i < question.Count; i++)
				createTests(question[i], answer[i]);
		}

		private void runAndDisplayTime(Action action)
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			action();
			stopWatch.Stop();

			long frequency = Stopwatch.Frequency;
			long ts = 1000000L * stopWatch.ElapsedTicks / frequency;
			Console.WriteLine("ОК {0} мс", ts / 1000.0);
		}

		public static Calculator calculator;
		public static List<string> question = new List<string>();
		public static List<decimal> answer = new List<decimal>();

		private void run()
		{
			//	Пользовательский ввод
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

		public const string TestPath = "Tests/";

		private static void Tests()
		{
			if (!Directory.Exists(TestPath))
				return;

			bool allIsGood = true;
			int i = 1;
			while (File.Exists(TestPath + i + ".in"))
			{
				StreamReader inputQuestion = new StreamReader(TestPath + i + ".in");
				StreamReader inputAnswer = new StreamReader(TestPath + i + ".out");

				string question = inputQuestion.ReadLine();
				decimal answer = decimal.Parse(inputAnswer.ReadLine());

				decimal answer1 = calculator.calculate(question);
				Console.Write("Тест {0}: {1} = {2} ", i, question, answer);
				if (Math.Abs(answer - answer1) < Calculator.EPSILON)
					Console.WriteLine("ОК");
				else
				{
					Console.WriteLine("ПЛОХО, != {0}", answer1);
					allIsGood = false;
				}

				i++;
			}
			Console.Write("\n------------\nИтого все тесты: {0}\n", allIsGood ? "ОК" : "ПЛОХО");
		}

		private static void createTests(string question, decimal answer)
		{
			string pathIn;
			string pathOut;

			int i = 1;
			if (!Directory.Exists(TestPath))
				Directory.CreateDirectory(TestPath);
			else
			{
				while (File.Exists(TestPath + i + ".in"))
				{
					pathIn = TestPath + i + ".in";
					pathOut = TestPath + i + ".out";

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

			pathIn = TestPath + i + ".in";
			pathOut = TestPath + i + ".out";

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
