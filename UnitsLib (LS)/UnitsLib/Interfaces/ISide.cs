using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitsLib.Interfaces
{
	/// <summary>
	/// Интерфейс стороны
	/// </summary>
	public interface ISide
	{
		/// <summary>
		/// Сторона, которой принадлежит объект
		/// </summary>
		bool Side { get; }
	}
}
