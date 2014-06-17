using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	public class Operation
	{

		// + - * / mod div pow
		public static Operation add = new Operation().addNames("+", "плюс").setPriority(2).setOperation((a, b) => a + b).setBinary();
		public static Operation sub = new Operation().addNames("-", "минус").setPriority(2).setOperation((a, b) => a - b).setBinary();
		public static Operation multi = new Operation().addNames("*", "умножить").setPriority(1).setOperation((a, b) => a * b).setBinary();
		public static Operation div = new Operation().addNames("/", "разделить").setPriority(1).setOperation((a, b) => a / b).setBinary();
		public static Operation mod = new Operation().addNames("%", "mod", "остатокотделения").setPriority(1).setOperation((a, b) => a % b).setBinary();
		public static Operation wholeDiv = new Operation().addNames("div", "целаячастьотделения").setPriority(1).setOperation((a, b) => Math.Floor(a / b)).setBinary();
		public static Operation pow = new Operation().addNames("^", "встепени").setPriority(2).setOperation((a, b) => (decimal) Math.Pow((double) a, (double) b)).setBinary();

		// + - log ln abs round sign sqrt sqr factorial
		public static Operation addUnary = new Operation().addNames("+").setPriority(0).setOperation((decimal a, decimal b) => +b).setUnary();
		public static Operation subUnary = new Operation().addNames("-").setPriority(0).setOperation((decimal a, decimal b) => -b).setUnary();
		public static Operation log = new Operation().addNames("log", "десятичныйлогарифм").setOperation((a, b) => (decimal) Math.Log10((double) b)).setUnary();
		public static Operation ln = new Operation().addNames("ln", "натуральныйлогарифм").setOperation((a, b) => (decimal) Math.Log((double) b)).setUnary();
		public static Operation abs = new Operation().addNames("abs", "модуль").setOperation((a, b) => (decimal) Math.Abs((double) b)).setUnary();
		public static Operation round = new Operation().addNames("round", "округлить").setOperation((a, b) => (decimal) Math.Round(b)).setUnary();
		public static Operation sign = new Operation().addNames("sign", "знак").setOperation((a, b) => (decimal) Math.Sign((double) b)).setUnary();
		public static Operation sqrt = new Operation().addNames("sqrt", "корень").setOperation((a, b) => (decimal) Math.Sqrt((double) b)).setUnary();
		public static Operation sqr = new Operation().addNames("sqr", "квадрат").setOperation((a, b) => b * b).setUnary();
		public static Operation factorial = new Operation().addNames("!", "факториал").setOperation(
			(a, b) =>
			{
				long f = 1;
				for (int i = 2; i < a + Calculator.EPSILON; i++)
				{
					f *= i;
				}
				return f;
			}
		).setPostUnary();

		// sin cos tg ctg asin acos atg actg sinh cosh tgh ctgh
		public static Operation sin = new Operation().addNames("sin", "синус").setOperation((a, b) => (decimal) Math.Sin((double) b)).setUnary();
		public static Operation cos = new Operation().addNames("cos", "косинус").setOperation((a, b) => (decimal) Math.Cos((double) b)).setUnary();
		public static Operation tg = new Operation().addNames("tg", "тангенс").setOperation((a, b) => (decimal) Math.Tan((double) b)).setUnary();
		public static Operation ctg = new Operation().addNames("ctg", "котангенс").setOperation((a, b) => 1 / ((decimal) Math.Tan((double) b))).setUnary();
		public static Operation aSin = new Operation().addNames("aSin", "арксинус").setOperation((a, b) => (decimal) Math.Asin((double) b)).setUnary();
		public static Operation aCos = new Operation().addNames("aCos", "арккосинус").setOperation((a, b) => (decimal) Math.Acos((double) b)).setUnary();
		public static Operation aTg = new Operation().addNames("aTg", "арктангенс").setOperation((a, b) => (decimal) Math.Atan((double) b)).setUnary();
		public static Operation aCtg = new Operation().addNames("aCtg", "арккотангенс").setOperation((a, b) => 1 / ((decimal) (Math.PI / 2 - Math.Atan((double) b)))).setUnary();
		public static Operation sinh = new Operation().addNames("sinh", "гиперболический синус").setOperation((a, b) => (decimal) Math.Sinh((double) b)).setUnary();
		public static Operation cosh = new Operation().addNames("cosh", "гиперболический косинус").setOperation((a, b) => (decimal) Math.Cosh((double) b)).setUnary();
		public static Operation tgh = new Operation().addNames("tgh", "гиперболический тангенс").setOperation((a, b) => (decimal) Math.Tanh((double) b)).setUnary();
		public static Operation ctgh = new Operation().addNames("ctgh", "гиперболический котангенс").setOperation((a, b) => 1 / ((decimal) Math.Tanh((double) b))).setUnary();

		public static Operation[] operations =	{
													add, sub, multi, div, mod, wholeDiv, pow, 
													addUnary, subUnary, log, ln, abs, round, sign, sqrt, sqr, factorial, 
													sin, cos, tg, ctg, aSin, aCos, aTg, aCtg, sinh, cosh, tgh, ctgh
												};


		public List<string> names = new List<string>();
		public int priority = 0;	// 0 - самый высокий

		public enum TypeOperation
		{
			OPERATION_BINARY,
			OPERATION_UNARY,
			OPERATION_POST_UNARY
		}

		public TypeOperation type;

		public delegate decimal operationDelegate(decimal a, decimal b);
		public operationDelegate operation;

		public Operation addNames(params string[] names)
		{
			this.names.AddRange(names.ToList());
			return this;
		}

		public Operation setPriority(int priority)
		{
			this.priority = priority;
			return this;
		}

		public Operation setOperation(operationDelegate operation)
		{
			this.operation = operation;
			return this;
		}

		public Operation setUnary()
		{
			this.type = TypeOperation.OPERATION_UNARY;
			return this;
		}

		public bool IsUnary
		{
			get
			{
				return type == TypeOperation.OPERATION_UNARY;
			}
		}

		public Operation setBinary()
		{
			this.type = TypeOperation.OPERATION_BINARY;
			return this;
		}

		public bool IsBinary
		{
			get
			{
				return type == TypeOperation.OPERATION_BINARY;
			}
		}

		public Operation setPostUnary()
		{
			this.type = TypeOperation.OPERATION_POST_UNARY;
			return this;
		}

		public bool IsPostUnary
		{
			get
			{
				return type == TypeOperation.OPERATION_POST_UNARY;
			}
		}

	}
}
