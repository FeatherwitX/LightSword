using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace UnitsLib.Events
{
	/// <summary>
	/// Данные о событии UI панели
	/// </summary>
	public class UIPanelEventArgs : EventArgs
	{
		/// <summary>
		/// Индекс элемента UI панели, с которым произошло событие
		/// </summary>
		public Point Index { get; set; }

		/// <summary>
		/// Создает новый экземпляр класса UIPanelEventArgs с указанными параметрами
		/// </summary>
		public UIPanelEventArgs(Point index) {
			Index = index;
		}
	}
}
