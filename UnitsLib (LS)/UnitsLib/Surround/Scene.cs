using System;
using System.Collections.Generic;
using System.Linq;
using Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sounds;
using UnitsLib.Surround;

namespace UnitsLib.Surround
{
	/// <summary>
	/// Сцена
	/// </summary>
	public struct Scene
	{
		private Rectangle rect;
		private Rectangle field;
		private Panel panel;

		/// <summary>
		/// Прямоугольник сцены
		/// </summary>
		public Rectangle Rect {
			get { return rect; }
			internal set {
				if (value.Left < field.Left) value.X = field.Left;
				if (value.Top < field.Top) value.Y = field.Top;
				if (value.Left + value.Width > field.Width) value.X = field.Width - value.Width;
				if (value.Top + value.Height > field.Height + panel.BottomPanelSize.Y) value.Y = field.Height - value.Height + (int)panel.BottomPanelSize.Y;
				rect = value;
			}
		}

		/// <summary>
		/// Возвращает True, если объект TObject пересекает границы сцены или лежит в ее пределах
		/// </summary>
		/// <param name="obj">Проверяемый объект типа TObject</param>
		public bool Contains(TObject obj) {
			return Contains(obj.bounds);
		}
		public bool Contains(Rectangle bounds) {
			return rect.Intersects(bounds) || rect.Contains(bounds);
		}
		public bool Contains(Vector2 v) {
			return rect.Contains((int)v.X, (int)v.Y);
		}

		/// <summary>
		/// Создает сцену на указанном глобальном поле
		/// </summary>
		/// <param name="field">Глобальное поле</param>
		public Scene(Rectangle field, Panel panel) {
			rect = new Rectangle(640, 512, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
			this.field = field;
			this.panel = panel;
		}

		/// <summary>
		/// Перемещает сцену на указанный вектор
		/// </summary>
		/// <param name="v">Вектор, на который перемещается сцена</param>
		public void MoveOn(Vector2 v) {
			Rect = new Rectangle(Rect.Left + (int)v.X, Rect.Top + (int)v.Y, Rect.Width, Rect.Height);
		}
	}
}
