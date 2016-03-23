using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitsLib.Exceptions
{
	public class UndefinedUnitNameException : Exception
	{
		public UndefinedUnitNameException() : base() { }

		public UndefinedUnitNameException(string message) : base(message) { }

		public UndefinedUnitNameException(string message, Exception innerException) : base(message, innerException) { }
	}
}
