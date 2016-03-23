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
using UnitsLib.Surround;
using SettingsLib;

namespace Light_Sword.UI
{
	/// <summary>
	/// Класс панель "Об авторах"
	/// </summary>
	public class CreditsPanel : Subpanel
	{
		string text;
		SpriteFont font;
		SpriteFont bFont;

		/// <summary>
		/// Создает новый экземпляр CreditsPanel с указанными параметрами
		/// </summary>
		/// <param name="content">Управляющий контентом для этой панели</param>
		/// <param name="sprite">Спрайт, изображение панели</param>
		/// <param name="text">Текст, прописанный в панели "Об авторах"</param>
		public CreditsPanel(Screen owner, ContentManager content, Sprite sprite, string text)
			: base(owner, content, sprite) {
			this.text = text;
			font = content.Load<SpriteFont>(Fnames.BigFont);
			bFont = content.Load<SpriteFont>(Fnames.ButtonFont);

            Button ExitLocal = new Button(MultiSprite.CreateSprite(content, spriteBatch, Fnames.ExitLocalB, new Vector2(sprite.Bounds.Left + (float)sprite.Bounds.Width / 5f * 3.2f, sprite.Bounds.Top + (float)sprite.Bounds.Height / 4f * 3.2f), new Vector2(128, 35), Vector2.One),
										  CreateSound3D(content, Fnames.UIButtonClick),
										  CreateSound3D(content, Fnames.UIButtonSelect),
										  bFont);
			ExitLocal.Text = "";
			ExitLocal.Visible = false;
			ExitLocal.Click += new EventHandler<MouseElementEventArgs>(ExitLocal_Click);
			ExitLocal.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			ExitLocal.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

			Buttons.Add(ExitLocal);

			Components.AddRange(Buttons);
		}

		/// <summary>
		/// Прорисовка панели в текущем кадре
		/// </summary>
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			if (Visible)
				spriteBatch.DrawString(font, text, new Vector2(Bounds.Left + Bounds.Width / 8f, Bounds.Top + Bounds.Height / 6f), Color.Azure);
		}

		#region Private Methods

		private void ExitLocal_Click(object sender, MouseElementEventArgs e) {
			Visible = false;
		}

		private void Button_MouseMove(object sender, MouseElementEventArgs e) {
			(sender as Button).Animation = true;
		}

		private void Button_MouseMoveOut(object sender, MouseElementEventArgs e) {
			(sender as Button).Animation = false;
		}

		#endregion
	}
}
