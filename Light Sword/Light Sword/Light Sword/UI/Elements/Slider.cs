using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Graph;
using Sounds;

namespace Light_Sword.UI
{
	/// <summary>
	/// Класс ползунок
	/// </summary>
	public class Slider : Element
	{
		#region Variables

		Sprite image_;
		float value;

		#endregion
		/// <summary>
		/// Изображение каретки ползунка (только для чтения)
		/// </summary>
		public Sprite Image_ { get { return image_; } }
		/// <summary>
		/// Значение ползунка [0;100]
		/// </summary>
		public decimal Value {
			get { return (decimal)value; }
			set {
				if (value < 0) value = 0;
				if (value > 100) value = 100;
				this.value = (float)value;
				image_.Position = new Vector2(Position.X + 9 + (Image.Size.X - 18) * (this.value / 100) - image_.Size.X / 2, Position.Y);
			}
		}

		/// <summary>
		/// Создает новый ползунок с указанными параметрами
		/// </summary>
		/// <param name="image">Изображение тела ползунка</param>
		/// <param name="image_">Изображение каретки ползунка</param>
		public Slider(MultiSprite image, Sprite image_) : base(image) {
			this.image_ = image_;
			this.image_.Color = new Color(255, 255, 255, 200);
			this.MouseMove += new EventHandler<MouseElementEventArgs>(Slider_MouseMove);

			value = 100;
		}

		/// <summary>
		/// Прорисовка полхунка в текущем кадре
		/// </summary>
		public override void Draw(SpriteBatch spriteBatch) {
			if (Visible) {
				base.Draw(spriteBatch);
				image_.Draw(spriteBatch);
			}
		}

		#region Private Methods


		private void Slider_MouseMove(object sender, MouseElementEventArgs e) {
			if (e.MouseState.LeftButton == ButtonState.Pressed) {
				image_.Position = new Vector2(e.Position.X - image_.Size.X / 2, image_.Position.Y);
				if (image_.Position.X + image_.Size.X / 2 > Image.Position.X + Image.Size.X - 9)
					image_.Position = new Vector2(Image.Position.X + Image.Size.X - 9 - image_.Size.X / 2, image_.Position.Y);
				if (image_.Position.X + image_.Size.X / 2 < Image.Position.X + 9)
					image_.Position = new Vector2(Image.Position.X + 9 - image_.Size.X / 2, image_.Position.Y);
				Value = (decimal)((image_.Position.X - Image.Position.X + 9 + image_.Size.X / 2) / (Image.Size.X - 18) - 0.08f);
				Value *= 100;
			}
		}

		#endregion
	}
}
