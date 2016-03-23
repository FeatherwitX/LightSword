using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitsLib.Events
{
	/// <summary>
	/// Данные о событии фокусировки
	/// </summary>
	public class FocusedEventArgs : EventArgs
	{
		/// <summary>
		/// В фокусе ли объект, с которым произошло событие
		/// </summary>
		public bool Focused { get; set; }

		/// <summary>
		/// Создает новый экземпляр класса FocusedEventArgs с указанными параметрами
		/// </summary>
		public FocusedEventArgs(bool focused) {
			Focused = focused;
		}
	}
}
