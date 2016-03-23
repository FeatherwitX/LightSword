using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitsLib.Events
{
	/// <summary>
	/// Данные о событии иконки объекта
	/// </summary>
	public class ObjectIconsEventArgs : EventArgs
	{
		/// <summary>
		/// Объект, с иконкой которого произошло событие
		/// </summary>
		public TObject Object { get; set; }

		/// <summary>
		/// Создает новый экземпляр ObjectIconsEventArgs с указанными параметрами
		/// </summary>
		public ObjectIconsEventArgs(TObject obj) {
			Object = obj;
		}
	}
}
