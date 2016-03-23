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
	/// Класс кнопка
	/// </summary>
	public class Button : Element
	{
		#region Variables

		Vector2 initPos, initSize;

		SpriteFont font;
		string text;
		Vector2 textPos;
		Color textColor = Color.White;

		Sound3D soundSelection;
		Sound3D soundClick;
		int animationCount = 0;

		#endregion

		/// <summary>
		/// Анимируется ли кнопка
		/// </summary>
		public bool Animation { get; set; }
		/// <summary>
		/// Текст кнопки
		/// </summary>
		public string Text {
			get { return text; }
			set {
				text = value;
				textPos = new Vector2(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2) - font.MeasureString(text) / 2;
			}
		}
		/// <summary>
		/// Цвет текста кнопки
		/// </summary>
		public Color TextColor { get { return textColor; } set { textColor = value; } }
		/// <summary>
		/// Шрифт, которым отображается текст
		/// </summary>
		public SpriteFont Font {
			get { return font; }
			set {
				font = value;
				textPos = new Vector2(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2) - font.MeasureString(text) / 2;
			}
		}

		/// <summary>
		/// Создает кнопку с указанным изображением
		/// </summary>
		public Button(MultiSprite image, Sound3D soundClick, Sound3D soundSelection, SpriteFont font)
			: base(image) {
			this.soundSelection = soundSelection;
			this.soundClick = soundClick;
			this.font = font;
			this.text = "button";
			this.textPos = new Vector2(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2) - font.MeasureString(text) / 2;
			Animation = false;
			this.MouseMoveIn += new EventHandler<MouseElementEventArgs>(Button_MouseMoveIn);
			this.Click += new EventHandler<MouseElementEventArgs>(Button_Click);

			initPos = Position;
			initSize = Size;
		}

		/// <summary>
		/// Возвращает кнопку в начальное состояние, заданное при создании
		/// </summary>
		public void ToNormal() {
			Position = initPos;
			Size = initSize;
			animationCount = 0;
			Animation = false;
		}

		/// <summary>
		/// Прорисовка кнопки в текущем кадре
		/// </summary>
		public override void Draw(SpriteBatch spriteBatch) {
			if (Visible) {
				if (Animation) {
					Position = (animationCount < 7) ? Position - new Vector2(2.5f, 2.5f * Image.Size.Y / Image.Size.X) : Position;
					Size = (animationCount < 7) ? Size + new Vector2(5, 5 * Image.Size.Y / Image.Size.X) : Size;
					animationCount++;
					if (animationCount > 7)
						animationCount = 7;
				} else {
					Position = (animationCount > 0) ? Position + new Vector2(2.5f, 2.5f * Image.Size.Y / Image.Size.X) : Position;
					Size = (animationCount > 0) ? Size - new Vector2(5, 5 * Image.Size.Y / Image.Size.X) : Size;
					animationCount--;
					if (animationCount < 0)
						animationCount = 0;
				}
				base.Draw(spriteBatch);
				spriteBatch.DrawString(font, text, textPos, textColor);
			}
		}

		#region Private Methods

		private void Button_Click(object sender, MouseElementEventArgs e) {
			if (soundClick != null)
				SoundPlayer.PlaySound(soundClick);
		}

		private void Button_MouseMoveIn(object sender, MouseElementEventArgs e) {
			if (soundSelection != null)
				SoundPlayer.PlaySound(soundSelection);
		}

		#endregion
	}
}
