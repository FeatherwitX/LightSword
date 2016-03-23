using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitsLib.Enums;

namespace UnitsLib.Events
{
	/// <summary>
	/// Данные о событии, связанном с целью
	/// </summary>
	public class AimEventArgs : EventArgs
	{
		/// <summary>
		/// Цель
		/// </summary>
		public Aims Aim { get; set; }

		/// <summary>
		/// Создает новый экземпляр AimEventArgs с указанными параметрами
		/// </summary>
		public AimEventArgs(Aims aim) {
			Aim = aim;
		}
	}
}
