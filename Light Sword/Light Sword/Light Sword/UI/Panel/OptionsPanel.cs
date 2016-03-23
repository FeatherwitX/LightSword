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
	/// Класс панель опций
	/// </summary>
	public class OptionsPanel : Subpanel
	{
		#region Variables

		Settings settings;
		decimal initMusicV;

		List<Checkbox> checkboxes;
		List<Slider> sliders;

		SpriteFont font;
		SpriteFont bFont;

		Checkbox musicMute;
		Checkbox soundMute;
		Slider musicSlider;
		Slider soundSlider;

		public Action ApplyAction;

		#endregion

		public List<Slider> Sliders {
			get { return sliders; }
			protected set { sliders = value; }
		}

		public List<Checkbox> Checkboxes {
			get { return checkboxes; }
			protected set { checkboxes = value; }
		}

		public override bool Visible {
			get {
				return base.Visible;
			}
			set {
				base.Visible = value;
				for (int i = Components.Count - 1; i >= 0; i--) {
					if (Components[i] is IDrawableComponent)
						(Components[i] as IDrawableComponent).Visible = value;
				}
				if (Visible) {
					ApplyedSettings = (Settings)settings.Clone();					
				}
			}
		}

		public Settings ApplyedSettings { get; protected set; }

		/// <summary>
		/// Создает новый экземпляр CreditsPanel с указанными параметрами
		/// </summary>
		/// <param name="content">Управляющий контентом для этой панели</param>
		/// <param name="sprite">Спрайт, изображение панели</param>
		/// <param name="spriteBatch">Спрайт батч, нужный для создания чекбоксов</param>
		/// <param name="settings">Настройки игры</param>
		public OptionsPanel(Screen owner, ContentManager content, Sprite sprite, Settings settings)
			: base(owner, content, sprite) {
			checkboxes = new List<Checkbox>();
			sliders = new List<Slider>();
			font = content.Load<SpriteFont>(Fnames.PanelFont);
			bFont = content.Load<SpriteFont>(Fnames.ButtonFont);
			this.settings = settings;
			initMusicV = settings.MusicVolume;

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

			#region Checkboxes

            musicMute = new Checkbox(MultiSprite.CreateSprite(content, spriteBatch, Fnames.Checkbox1, new Vector2(Bounds.Left + (float)Bounds.Width / 5f * 1f, Bounds.Top + (float)Bounds.Height / 4f * 1f), new Vector2(42, 39), new Vector2(2, 1)),
									 CreateSound(content, Fnames.UIButtonClick));
			musicMute.Visible = false;
			musicMute.Checked = !settings.MusicIsMuted;
			musicMute.Click += new EventHandler<MouseElementEventArgs>(musicMute_Click);

            soundMute = new Checkbox(MultiSprite.CreateSprite(content, spriteBatch, Fnames.Checkbox1, new Vector2(Bounds.Left + (float)Bounds.Width / 5f * 1f, Bounds.Top + (float)Bounds.Height / 4f * 1.8f), new Vector2(42, 39), new Vector2(2, 1)),
									 CreateSound(content, Fnames.UIButtonClick));
			soundMute.Visible = false;
			soundMute.Checked = !settings.SoundIsMuted;
			soundMute.Click += new EventHandler<MouseElementEventArgs>(soundMute_Click);

			Checkboxes.Add(musicMute);
			Checkboxes.Add(soundMute);

			#endregion

			#region Sliders

            musicSlider = new Slider(MultiSprite.CreateSprite(content, spriteBatch, Fnames.Slider1, new Vector2(Bounds.Left + (float)Bounds.Width / 5f * 1f, Bounds.Top + (float)Bounds.Height / 4f * 1.4f), new Vector2(238, 26), Vector2.One),
                                     Sprite.CreateSprite(content, Fnames.Slider_, new Vector2(Bounds.Left + (float)Bounds.Width / 5f * 1f, Bounds.Top + (float)Bounds.Height / 4f * 1.4f), new Vector2(12, 26)));
			musicSlider.Visible = false;
			musicSlider.MouseMove += new EventHandler<MouseElementEventArgs>(musicSlider_MouseMove);
			musicSlider.Value = settings.MusicVolume;

            soundSlider = new Slider(MultiSprite.CreateSprite(content, spriteBatch, Fnames.Slider1, new Vector2(Bounds.Left + (float)Bounds.Width / 5f * 1f, Bounds.Top + (float)Bounds.Height / 4f * 2.2f), new Vector2(238, 26), Vector2.One),
                                     Sprite.CreateSprite(content, Fnames.Slider_, new Vector2(Bounds.Left + (float)Bounds.Width / 5f * 1f, Bounds.Top + (float)Bounds.Height / 4f * 2.2f), new Vector2(12, 26)));
			soundSlider.Visible = false;
			soundSlider.MouseMove += new EventHandler<MouseElementEventArgs>(soundSlider_MouseMove);
			soundSlider.Value = settings.SoundVolume;

			Sliders.Add(musicSlider);
			Sliders.Add(soundSlider);

			#endregion

			Components.AddRange(Buttons);
			Components.AddRange(Checkboxes);
			Components.AddRange(Sliders);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			if (Visible) {
				spriteBatch.DrawString(font, String.Format("{0} {1}", "Music: ", (musicMute.Checked) ? "On" : "Off"), new Vector2(musicMute.Position.X + musicMute.Size.X + (float)Bounds.Width / 50f, musicMute.Position.Y + musicMute.Size.Y / 8f), Color.Azure);
				spriteBatch.DrawString(font, String.Format("{0} {1:0.00}","Music Volume: ", musicSlider.Value), new Vector2(musicSlider.Position.X + musicSlider.Size.X + (float)Bounds.Width / 50f, musicSlider.Position.Y + musicSlider.Size.Y / 6f), Color.Azure);
				spriteBatch.DrawString(font, String.Format("{0} {1}", "Sound: ", (soundMute.Checked) ? "On" : "Off"), new Vector2(soundMute.Position.X + soundMute.Size.X + (float)Bounds.Width / 50f, soundMute.Position.Y + soundMute.Size.Y / 8f), Color.Azure);
				spriteBatch.DrawString(font, String.Format("{0} {1:0.00}", "Sound Volume: ", soundSlider.Value), new Vector2(soundSlider.Position.X + soundSlider.Size.X + (float)Bounds.Width / 50f, soundSlider.Position.Y + soundSlider.Size.Y / 6f), Color.Azure);
			}
		}

		#region Private Methods

		private void ExitLocal_Click(object sender, MouseElementEventArgs e) {
			Visible = false;
			settings = (Settings)ApplyedSettings.Clone();
			settings.MusicVolume = (settings.MusicIsMuted) ? 0 : settings.MusicVolume;
			settings.SoundVolume = (settings.SoundIsMuted) ? 0 : settings.SoundVolume;
			if (settings.MusicIsMuted) {
				Owner.Player.Pause();
			} else
				Owner.Player.Start();
			SoundPlayer.IsMuted = settings.SoundIsMuted;
			InitSettings();
		}

		private void Apply_Click(object sender, MouseElementEventArgs e) {
			ApplyedSettings = (Settings)settings.Clone();
			ApplyedSettings.Save();
			Visible = false;
			if (ApplyAction != null)
				ApplyAction();
		}

		private void Button_MouseMove(object sender, MouseElementEventArgs e) {
			(sender as Button).Animation = true;
		}

		private void Button_MouseMoveOut(object sender, MouseElementEventArgs e) {
			(sender as Button).Animation = false;
		}

		private void musicMute_Click(object sender, MouseElementEventArgs e) {
			settings.MusicIsMuted = !musicMute.Checked;
			settings.MusicVolume = (settings.MusicIsMuted) ? 0 : 100;
			if (settings.MusicIsMuted) {
				Owner.Player.Pause();
			} else {
				Owner.Player.Start();
			}
			InitSettings();
		}


		private void musicSlider_MouseMove(object sender, MouseElementEventArgs e) {
			if (musicMute.Checked) {
				settings.MusicVolume = musicSlider.Value;
				Owner.Player.Volume = (float)settings.MusicVolume / 100f;
			} else {
				musicSlider.Value = 0;
			}
		}

		private void soundMute_Click(object sender, MouseElementEventArgs e) {
			settings.SoundIsMuted = !soundMute.Checked;
			settings.SoundVolume = (settings.SoundIsMuted) ? 0 : 100;
			SoundPlayer.IsMuted = settings.SoundIsMuted;
			InitSettings();
		}

		private void soundSlider_MouseMove(object sender, MouseElementEventArgs e) {
			if (soundMute.Checked) {
				settings.SoundVolume = soundSlider.Value;
				SoundPlayer.Volume = (float)settings.SoundVolume / 100f;
			} else {
				soundSlider.Value = 0;
			}
		}

		private void InitSettings() {
			Owner.Player.Volume = (float)settings.MusicVolume / 100f;
			SoundPlayer.Volume = (float)settings.SoundVolume / 100f;
			musicSlider.Value = settings.MusicVolume;
			soundSlider.Value = settings.SoundVolume;
			musicMute.Checked = !settings.MusicIsMuted;
			soundMute.Checked = !settings.SoundIsMuted;
		}

		#endregion
	}
}
