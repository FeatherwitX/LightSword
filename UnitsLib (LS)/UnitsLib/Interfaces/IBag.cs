using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitsLib.Enums;

namespace UnitsLib.Interfaces
{
	/// <summary>
	/// Интерфейс сумки
	/// </summary>
	public interface IBag
	{
		/// <summary>
		/// Текущий размер сумки
		/// </summary>
		int Bag { get; set; }
		/// <summary>
		/// Максимальный размер сумки
		/// </summary>
		int MaxBag { get; set; }
		/// <summary>
		/// Тип ресурсов в сумке (только для чтения)
		/// </summary>
		SourceTypes SourceType { get; }
		/// <summary>
		/// Загрузка сумки
		/// </summary>
		void Download();
		/// <summary>
		/// Выгрузка сумки
		/// </summary>
		void Upload();
	}
}
