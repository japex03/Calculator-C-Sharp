using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	public class Operation
	{

		public List<string> names = new List<string>();
		public int priority = 0;	// 0 - самый высокий
		public bool isUnary = false;

		public delegate decimal operationDelegate(decimal a, decimal b);
		public operationDelegate opeartion;

		public Operation()
		{

		}

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
			this.opeartion = operation;
			return this;
		}

		public Operation setUnary(bool unary)
		{
			this.isUnary = unary;
			return this;
		}

	}
}
