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
	/// Класс панель принятия решения
	/// </summary>
	public class ApplyPanel : Subpanel
	{
		string text;
		SpriteFont font;
		SpriteFont bFont;

		/// <summary>
		/// void метод без параметров, который выполняется при нажатии на "Принять"
		/// </summary>
		public Action ApplyAction;
		/// <summary>
		/// void метод без параметров, который выполняется при нажатии на "Выход"
		/// </summary>
		public Action ExitAction;

		/// <summary>
		/// Создает новую панель принятия решения с указанными параметрами
		/// </summary>
		/// <param name="content">Управляющий контентом для этой панели</param>
		/// <param name="sprite">Спрайт, изображение панели</param>
		/// <param name="text">Текст, который отображается на панели</param>
		public ApplyPanel(Screen owner, ContentManager content, Sprite sprite, string text)
			: base(owner, content, sprite) {
			this.text = text;
			font = content.Load<SpriteFont>(Fnames.PanelFont);
			bFont = content.Load<SpriteFont>(Fnames.ButtonFont);

            Button Cancel = new Button(MultiSprite.CreateSprite(content, spriteBatch, Fnames.CancelB, new Vector2(Bounds.Left + (float)Bounds.Width / 5f * 2.8f, Bounds.Top + (float)Bounds.Height / 4f * 2.5f), new Vector2(128 * (float)Bounds.Width / 390f, 35 * (float)Bounds.Height / 240f), Vector2.One),
									   CreateSound3D(content, Fnames.UIButtonClick),
									   CreateSound3D(content, Fnames.UIButtonSelect),
									   bFont);
			Cancel.Text = "";
			Cancel.Visible = false;
			Cancel.Click += new EventHandler<MouseElementEventArgs>(Cancel_Click);
			Cancel.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			Cancel.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

            Button Apply = new Button(MultiSprite.CreateSprite(content, spriteBatch, Fnames.ApplyB, new Vector2(Bounds.Left + (float)Bounds.Width / 5f * 0.6f, Bounds.Top + (float)Bounds.Height / 4f * 2.5f), new Vector2(128 * (float)Bounds.Width / 390f, 35 * (float)Bounds.Height / 240f), Vector2.One),
									  CreateSound3D(content, Fnames.UIButtonClick),
									  CreateSound3D(content, Fnames.UIButtonSelect),
									  bFont);
			Apply.Text = "";
			Apply.Visible = false;
			Apply.Click += new EventHandler<MouseElementEventArgs>(Apply_Click);
			Apply.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			Apply.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

			Buttons.Add(Cancel);
			Buttons.Add(Apply);

			Components.AddRange(Buttons);
		}

		/// <summary>
		/// Прорисовка панели в текущем кадре
		/// </summary>
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			if (Visible)
				spriteBatch.DrawString(font, text, new Vector2(Bounds.Left + Bounds.Width / 10f, Bounds.Top + Bounds.Height / 3.5f), Color.Azure);
		}

		#region Private Methods

		private void Apply_Click(object sender, MouseElementEventArgs e) {
			if (ApplyAction != null)
				ApplyAction();
		}

		private void Cancel_Click(object sender, MouseElementEventArgs e) {
			if (ExitAction != null)
				ExitAction();
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
