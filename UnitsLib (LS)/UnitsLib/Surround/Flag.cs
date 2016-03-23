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
using UnitsLib.Enums;
using UnitsLib.Interfaces;

namespace UnitsLib.Surround
{
	/// <summary>
	/// Флажок
	/// </summary>
	public sealed class Flag
	{
		private Vector2 position;
		private bool visible;

		/// <summary>
		/// Позиция флажка, относительно левого верхнего угла экрана
		/// </summary>
		public Vector2 Position {
			get { return position; }
			set {
				position = value;
				Image.Position = value;
			}
		}
		/// <summary>
		/// Виден ли флажок
		/// </summary>
		public bool Visible {
			get { return visible; }
			set {
				visible = value;
				Image.Visible = visible;
			}
		}
		/// <summary>
		/// Изображение флажка
		/// </summary>
		public Sprite Image;

		/// <summary>
		/// Инициализирует новый флажок
		/// </summary>
		public Flag() {	}
	}
}
