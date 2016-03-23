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
	public class SavePanel : Subpanel
	{
		public const int MaxSave = ListBox.MAX_ELEMENTS;

		#region Variables

		SpriteFont bFont;
		SpriteFont font;
		ListBox lbox;

		#endregion

		public override bool Visible {
			get {
				return base.Visible;
			}
			set {
				base.Visible = value;
				lbox.Visible = value;
			}
		}

		public SavePanel(Screen owner, ContentManager content, Sprite sprite)
			: base(owner, content, sprite) {
			bFont = content.Load<SpriteFont>(Fnames.ButtonFont);
			font = content.Load<SpriteFont>(Fnames.SavePanelFont);

			#region Buttons

            Button ExitLocal = new Button(MultiSprite.CreateSprite(content, spriteBatch, Fnames.ExitLocalB, new Vector2(Bounds.Left + (float)Bounds.Width / 5f * 3.2f, Bounds.Top + (float)Bounds.Height / 4f * 3f), new Vector2(128, 35), Vector2.One),
										  CreateSound3D(content, Fnames.UIButtonClick),
										  CreateSound3D(content, Fnames.UIButtonSelect),
										  bFont);
			ExitLocal.Text = "";
			ExitLocal.Visible = false;
			ExitLocal.Click += new EventHandler<MouseElementEventArgs>(ExitLocal_Click);
			ExitLocal.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			ExitLocal.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

            Button Apply = new Button(MultiSprite.CreateSprite(content, spriteBatch, Fnames.ApplyB, new Vector2(Bounds.Left + (float)Bounds.Width / 5f * 1f, Bounds.Top + (float)Bounds.Height / 4f * 3f), new Vector2(128, 35), Vector2.One),
									  CreateSound3D(content, Fnames.UIButtonClick),
									  CreateSound3D(content, Fnames.UIButtonSelect),
									  bFont);
			Apply.Text = "";
			Apply.Visible = false;
			Apply.Click += new EventHandler<MouseElementEventArgs>(Apply_Click);
			Apply.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			Apply.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

			Buttons.Add(ExitLocal);
			Buttons.Add(Apply);

			#endregion

			#region ListBox

            MultiSprite mesh = MultiSprite.CreateSprite(content, spriteBatch, Fnames.Mesh, Vector2.One, Vector2.One, Vector2.One);
			mesh.Color = new Color(0, 0, 0, 80);
            lbox = new ListBox(MultiSprite.CreateSprite(content, spriteBatch, Fnames.ListBox, new Vector2(Bounds.Left + Bounds.Width / 1000f * 120, Bounds.Top + Bounds.Height / 1000f * 100), new Vector2(Bounds.Width / 1.3f, Bounds.Height / 1.6f), Vector2.One),
							   mesh,
							   font);
			lbox[0] = this;
			lbox[1] = this;
			lbox[4] = "Kirill the superman!";
			lbox[9] = this;
			lbox[2] = bFont;
			lbox[3] = this;
			lbox[5] = this;
			//lbox[6] = lbox[7] = lbox[8] = this;

			#endregion

			Components.Add(lbox);
			Components.AddRange(Buttons);
		}

		#region Private Methods

		private void Button_MouseMove(object sender, MouseElementEventArgs e) {
			(sender as Button).Animation = true;
		}

		private void Button_MouseMoveOut(object sender, MouseElementEventArgs e) {
			(sender as Button).Animation = false;
		}

		private void ExitLocal_Click(object sender, MouseElementEventArgs e) {
			Visible = false;
		}

		private void Apply_Click(object sender, MouseElementEventArgs e) {
			Visible = false;
		}

		#endregion
	}
}
