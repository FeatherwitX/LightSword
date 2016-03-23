using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitsLib.Interfaces
{
	/// <summary>
	/// Интерфейс создания
	/// </summary>
	public interface ICreate
	{
		/// <summary>
		/// Текущий прогресс создания
		/// </summary>
		int Progress { get; set; }
		/// <summary>
		/// Максимальный прогресс создания (только для чтения)
		/// </summary>
		int MaxProgress { get; }
		/// <summary>
		/// Создание
		/// </summary>
		void Create();
	}
}
