using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UnitsLib.Events
{
	/// <summary>
	/// Данные о событии, связанном с миникартой и мышью
	/// </summary>
	public class MinimapMouseEventArgs : EventArgs
	{
		/// <summary>
		/// Позиция мыши
		/// </summary>
		public Vector2 Position { get; set; }
		/// <summary>
		/// Состояние мыши
		/// </summary>
		public MouseState MouseState { get; set; }

		/// <summary>
		/// Создает новый экземпляр MinimapMouseEventArgs с указанными паарметрами
		/// </summary>
		public MinimapMouseEventArgs(Vector2 position, MouseState mouseState) {
			Position = position;
			MouseState = mouseState;
		}
	}
}
