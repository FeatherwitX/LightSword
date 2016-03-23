using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace UnitsLib.Surround
{
	/// <summary>
	/// Прямоугольник здоровья
	/// </summary>
	public struct hpRect
	{
		internal Rectangle Rect;
		internal Rectangle Border;
		internal bool Vis;
		internal Color Color;
		internal Color BorderColor;
		internal int yOffset;

		private int initWidth;

		internal int Width { get { return Rect.Width; } set { Rect.Width = value; Border.Width = value + 2; } }
		internal int Height { get { return Rect.Height; } set { Rect.Height = value; } }
		internal Point Location { get { return Rect.Location; } set { Rect.Location = value; } }
		internal int InitWidth { get { return initWidth; } }

		internal hpRect(Rectangle rect, int yOffset) {
			Rect = rect;
			Vis = true;
			Color = Color.Green;
			Border = new Rectangle(Rect.X - 1, Rect.Y - 1, Rect.Width + 2, Rect.Height + 2);
			BorderColor = Color.Transparent;
			initWidth = rect.Width;
			this.yOffset = yOffset;
		}
		internal hpRect(Rectangle rect) {
			Rect = rect;
			Vis = true;
			Color = Color.Green;
			Border = new Rectangle(Rect.X - 1, Rect.Y - 1, Rect.Width + 2, Rect.Height + 2);
			BorderColor = Color.Transparent;
			initWidth = rect.Width;
			this.yOffset = 0;
		}

		/// <summary>
		/// Перемещает прямоугольник здоровья в точку (x,y)
		/// </summary>
		/// <param name="x">Координата по оси X</param>
		/// <param name="y">Координата по оси Y</param>
		internal void MoveTo(int x, int y) {
			Rect.Offset(x - Rect.Left, y - Rect.Top);
			Border.Offset(x - Border.Left - 1, y - Border.Top - 1);
		}

		internal void MoveOn(Vector2 v) {
			Rect.Offset((int)v.X, (int)v.Y);
			Border.Offset((int)v.X, (int)v.Y);
		}
	}
}
