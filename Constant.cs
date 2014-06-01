using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{

	class Constant
	{
		public string[] names;
		public decimal value;

		public Constant setNames(params string[] names)
		{
			this.names = names;
			return this;
		}

		public Constant setValue(decimal value)
		{
			this.value = value;
			return this;
		}

	}

}
