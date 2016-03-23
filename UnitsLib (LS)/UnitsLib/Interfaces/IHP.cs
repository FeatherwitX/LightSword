using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitsLib.Interfaces
{
	/// <summary>
	/// Интерфейс очков здоровья
	/// </summary>
	public interface IHP
	{
		/// <summary>
		/// Текущее здоровье
		/// </summary>
		int HP { get; set; }
		/// <summary>
		/// Максимальное здоровье (только для чтения)
		/// </summary>
		int MaxHP { get; }
	}
}
