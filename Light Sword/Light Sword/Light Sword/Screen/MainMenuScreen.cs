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
using Light_Sword.UI;

namespace Light_Sword
{
	/// <summary>
	/// Класс экран главное меню
	/// </summary>
	public class MainMenuScreen : Screen
	{
		#region Variables

		Texture2D background;
		SpriteFont backgroundFont;
		SpriteFont bFont;
		string backgroundText = "Light Sword";
		bool backgroundTextVisible = true;

		OptionsPanel optionsPanel;
		CreditsPanel creditsPanel;
		ApplyPanel exitApplyPanel;
		SavePanel savePanel;
		LoadPanel loadPanel;

		Settings settings;
		AnalysedSettings cfg;
		List<Button> buttons;

		bool isContinue;
		Button continueB;

		Random random;

		#endregion

		/// <summary>
		/// Создает новый экран главное меню с указанными параметрами
		/// </summary>
		/// <param name="game">Игра, к которой привязан экран</param>
		/// <param name="settings">Настройки игры</param>
		public MainMenuScreen(Game1 game, Settings settings)
			: base(game) {
			this.settings = settings;
			cfg = new AnalysedSettings(this.settings);
			Initialize();
		}

		/// <summary>
		/// Инициализация экрана (вызывается до LoadContent())
		/// </summary>
		protected override void Initialize() {
			buttons = new List<Button>();
			random = new Random();
			player.Stop();
			player.Clear();
			player.Volume = cfg.MusicVolume;
			SoundPlayer.Volume = cfg.SoundVolume;

			base.Initialize();
		}

		/// <summary>
		/// Загрузка контента (вызывается после Initialize())
		/// </summary>
		protected override void LoadContent() {

			backgroundFont = Content.Load<SpriteFont>(Fnames.OldEnglishTextMT);
			bFont = Content.Load<SpriteFont>(Fnames.ButtonFont);

			#region Buttons

			int start = 300;
			int step = (cfg.ResolutionHeight == 600) ? 65 : 50;
			int count = 0;

			Button Continue = new Button(MultiSprite.CreateSprite(Content, spriteBatch, Fnames.ContinueB, new Vector2(Window.ClientBounds.Width / 2f - 122, Window.ClientBounds.Height / 1000f * (start + step * count++)), new Vector2(244, 36), Vector2.One),
									CreateSound3D(Content, Fnames.UIButtonClick),
									CreateSound3D(Content, Fnames.UIButtonSelect),
									bFont);
			Button New = new Button(MultiSprite.CreateSprite(Content, spriteBatch, Fnames.NewB, new Vector2(Window.ClientBounds.Width / 2f - 122, Window.ClientBounds.Height / 1000f * (start + step * count++)), new Vector2(244, 36), Vector2.One),
									CreateSound3D(Content, Fnames.UIButtonClick),
									CreateSound3D(Content, Fnames.UIButtonSelect),
									bFont);
			Button Save = new Button(MultiSprite.CreateSprite(Content, spriteBatch, Fnames.SaveB, new Vector2(Window.ClientBounds.Width / 2f - 122, Window.ClientBounds.Height / 1000f * (start + step * count++)), new Vector2(244, 36), Vector2.One),
									CreateSound3D(Content, Fnames.UIButtonClick),
									CreateSound3D(Content, Fnames.UIButtonSelect),
									bFont);
			Button Load = new Button(MultiSprite.CreateSprite(Content, spriteBatch, Fnames.LoadB, new Vector2(Window.ClientBounds.Width / 2f - 122, Window.ClientBounds.Height / 1000f * (start + step * count++)), new Vector2(244, 36), Vector2.One),
									CreateSound3D(Content, Fnames.UIButtonClick),
									CreateSound3D(Content, Fnames.UIButtonSelect),
									bFont);
			Button Options = new Button(MultiSprite.CreateSprite(Content, spriteBatch, Fnames.OptionsB, new Vector2(Window.ClientBounds.Width / 2f - 122, Window.ClientBounds.Height / 1000f * (start + step * count++)), new Vector2(244, 36), Vector2.One),
										CreateSound3D(Content, Fnames.UIButtonClick),
										CreateSound3D(Content, Fnames.UIButtonSelect),
										bFont);
			Button Credits = new Button(MultiSprite.CreateSprite(Content, spriteBatch, Fnames.CreditsB, new Vector2(Window.ClientBounds.Width / 2f - 122, Window.ClientBounds.Height / 1000f * (start + step * count++)), new Vector2(244, 36), Vector2.One),
										CreateSound3D(Content, Fnames.UIButtonClick),
										CreateSound3D(Content, Fnames.UIButtonSelect),
										bFont);
			Button Exit = new Button(MultiSprite.CreateSprite(Content, spriteBatch, Fnames.ExitGlobalB, new Vector2(Window.ClientBounds.Width / 2f - 122, Window.ClientBounds.Height / 1000f * (start + step * count++)), new Vector2(244, 36), Vector2.One),
									 CreateSound3D(Content, Fnames.UIButtonClick),
									 CreateSound3D(Content, Fnames.UIButtonSelect),
									 bFont);
			Continue.Text = "";
			Continue.Click += new EventHandler<MouseElementEventArgs>(Continue_Click);
			Continue.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			Continue.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);
			Continue.Visible = isContinue = false;
			continueB = Continue;

			New.Text = "";
			New.Click += new EventHandler<MouseElementEventArgs>(New_Click);
			New.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			New.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

			Save.Text = "";
			Save.Click += new EventHandler<MouseElementEventArgs>(Save_Click);
			Save.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			Save.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

			Load.Text = "";
			Load.Click += new EventHandler<MouseElementEventArgs>(Load_Click);
			Load.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			Load.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

			Options.Text = "";
			Options.Click += new EventHandler<MouseElementEventArgs>(Options_Click);
			Options.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			Options.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

			Credits.Text = "";
			Credits.Click += new EventHandler<MouseElementEventArgs>(Credits_Click);
			Credits.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			Credits.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

			Exit.Text = "";
			Exit.Click += new EventHandler<MouseElementEventArgs>(Exit_Click);
			Exit.MouseMove += new EventHandler<MouseElementEventArgs>(Button_MouseMove);
			Exit.MouseMoveOut += new EventHandler<MouseElementEventArgs>(Button_MouseMoveOut);

			buttons.Add(Continue);
			buttons.Add(New);
			buttons.Add(Save);
			buttons.Add(Load);
			buttons.Add(Options);
			buttons.Add(Credits);
			buttons.Add(Exit);

			#endregion

			#region Panels

			string text = "Do you really want to exit?";
			exitApplyPanel = new ApplyPanel(this, Content, Sprite.CreateSprite(Content, Fnames.Panel, new Vector2(Window.ClientBounds.Width / 3f, Window.ClientBounds.Height / 3f), new Vector2(780 * (float)Window.ClientBounds.Width / 1280f / 2f, 640 * (float)Window.ClientBounds.Height / 1024f / 3f)), text);
			exitApplyPanel.ApplyAction += new Action(() => Game.Exit());
			exitApplyPanel.ExitAction += new Action(() => exitApplyPanel.Visible = false);
			exitApplyPanel.Enabled += new EventHandler<EventArgs>(panel_Enabled);
			exitApplyPanel.Disabled += new EventHandler<EventArgs>(panel_Disabled);

            optionsPanel = new OptionsPanel(this, Content, Sprite.CreateSprite(Content, Fnames.Panel, new Vector2(Window.ClientBounds.Width / 5.2f, Window.ClientBounds.Height / 6.8f), new Vector2(780 * (float)Window.ClientBounds.Width / 1280f, 640 * (float)Window.ClientBounds.Height / 1024f)), settings);
			optionsPanel.Enabled += new EventHandler<EventArgs>(panel_Enabled);
			optionsPanel.Disabled += new EventHandler<EventArgs>(panel_Disabled);
			optionsPanel.Disabled += new EventHandler<EventArgs>((object sender, EventArgs e) => settings = (Settings)optionsPanel.ApplyedSettings.Clone());
			optionsPanel.ApplyAction += new Action(() => { if (optionsPanel.ApplyedSettings.MusicIsMuted)  player.Stop(); else player.Start(); });

			text = String.Format("{0}\n{1}\n{2}", "Credits: ", "1) Bolshakov Kirill", "http://vk.com/overlordff");
            creditsPanel = new CreditsPanel(this, Content, Sprite.CreateSprite(Content, Fnames.Panel, new Vector2(Window.ClientBounds.Width / 5.2f, Window.ClientBounds.Height / 6.8f), new Vector2(780 * (float)Window.ClientBounds.Width / 1280f, 640 * (float)Window.ClientBounds.Height / 1024f)), text);
			creditsPanel.Enabled += new EventHandler<EventArgs>(panel_Enabled);
			creditsPanel.Disabled += new EventHandler<EventArgs>(panel_Disabled);

            savePanel = new SavePanel(this, Content, Sprite.CreateSprite(Content, Fnames.Panel, new Vector2(Window.ClientBounds.Width / 5.2f, Window.ClientBounds.Height / 6.8f), new Vector2(780 * (float)Window.ClientBounds.Width / 1280f, 640 * (float)Window.ClientBounds.Height / 1024f)));
			savePanel.Enabled += new EventHandler<EventArgs>(panel_Enabled);
			savePanel.Disabled += new EventHandler<EventArgs>(panel_Disabled);

            loadPanel = new LoadPanel(this, Content, Sprite.CreateSprite(Content, Fnames.Panel, new Vector2(Window.ClientBounds.Width / 5.2f, Window.ClientBounds.Height / 6.8f), new Vector2(780 * (float)Window.ClientBounds.Width / 1280f, 640 * (float)Window.ClientBounds.Height / 1024f)));
			loadPanel.Enabled += new EventHandler<EventArgs>(panel_Enabled);
			loadPanel.Disabled += new EventHandler<EventArgs>(panel_Disabled);

			#endregion

			Song deathSong = Content.Load<Song>(Fnames.DeathSong);
			Song epicScore = Content.Load<Song>(Fnames.EpicScore);

			if (random.NextDouble() >= 0.5) {
				background = Content.Load<Texture2D>(Fnames.BackgroundSword);
				player.Add(deathSong);
				optionsPanel.Color = new Color(70, 70, 255);
				creditsPanel.Color = new Color(70, 70, 255);
				exitApplyPanel.Color = new Color(70, 70, 255);
				savePanel.Color = new Color(70, 70, 255);
				loadPanel.Color = new Color(70, 70, 255);
			} else {
				background = Content.Load<Texture2D>(Fnames.BackgroundKnight);
				player.Add(epicScore);
				optionsPanel.Color = new Color(174, 160, 75);
				creditsPanel.Color = new Color(174, 160, 75);
				exitApplyPanel.Color = new Color(174, 160, 75);
				savePanel.Color = new Color(174, 160, 75);
				loadPanel.Color = new Color(174, 160, 75);
			}

			Game.Components.Clear();
			Game.Components.Add(new FPS(Game, Fnames.Arial14, new Vector2(Window.ClientBounds.Width / 2f - 25, 0)));
			Game.Components.Add(new GameCursor(Game, Fnames.Cursor));

			Components.Add(exitApplyPanel);
			Components.Add(optionsPanel);
			Components.Add(creditsPanel);
			Components.Add(savePanel);
			Components.Add(loadPanel);
			Components.AddRange(buttons);

			if (!cfg.MusicIsMuted)
				player.Start();
		}

		/// <summary>
		/// Выгрузка контента
		/// </summary>
		protected override void UnloadContent() {

		}

		/// <summary>
		/// Обновление экрана в текущем кадре
		/// </summary>
		public override void Update(GameTime gameTime) {
			OMS = MS;
			MS = Mouse.GetState();
			MouseHandler(MS, OMS, MouseStates.Normal);

			base.Update(gameTime);
		}

		/// <summary>
		/// Прорисовка экрана в текущем кадре
		/// </summary>
		public override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Green);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, Color.White);
			if (backgroundTextVisible)
				spriteBatch.DrawString(backgroundFont, backgroundText, new Vector2(GraphicsDevice.Viewport.Width / 2 - backgroundFont.MeasureString(backgroundText).X / 2, GraphicsDevice.Viewport.Height / 10), Color.Black);
			base.Draw(gameTime);

			spriteBatch.End();
		}

		/// <summary>
		/// Обработка мыши
		/// </summary>
		/// <param name="ms">Текущее состояние мыши</param>
		/// <param name="oms">Предыдущее состояние мыши</param>
		/// <param name="state">Состояние MouseStates мыши</param>
		protected override void MouseHandler(MouseState ms, MouseState oms, MouseStates state) {

		}

		/// <summary>
		/// Обработчик клавиатуры
		/// </summary>
		/// <param name="kbs">Текущее состояние клавиатуры</param>
		/// <param name="okbs">Предыдущее состояние клавиатуры</param>
		protected override void KeyboardHandler(KeyboardState kbs, KeyboardState okbs) {

		}

		#region Private Methods

		private void Continue_Click(object sender, MouseElementEventArgs e) {
			
		}

		private void New_Click(object sender, MouseElementEventArgs e) {
			buttons.Clear();
			Game.CurrentScreen = new GameScreen(Game, settings);
		}

		private void Save_Click(object sender, MouseElementEventArgs e) {
			savePanel.Visible = true;
		}

		private void Load_Click(object sender, MouseElementEventArgs e) {
			loadPanel.Visible = true;
		}

		private void Options_Click(object sender, MouseElementEventArgs e) {
			optionsPanel.Visible = true;
		}

		private void Credits_Click(object sender, MouseElementEventArgs e) {
			creditsPanel.Visible = true;
		}

		private void Exit_Click(object sender, MouseElementEventArgs e) {
			exitApplyPanel.Visible = true;
		}

		private void Button_MouseMove(object sender, MouseElementEventArgs e) {
			(sender as Button).Animation = true;
		}

		private void Button_MouseMoveOut(object sender, MouseElementEventArgs e) {
			(sender as Button).Animation = false;
		}

		private void panel_Enabled(object sender, EventArgs e) {
			for (int i = buttons.Count - 1; i >= 0; i--) {
				buttons[i].Visible = false;
			}
			backgroundTextVisible = false;
		}

		private void panel_Disabled(object sender, EventArgs e) {
			for (int i = buttons.Count - 1; i >= 0; i--) {
				buttons[i].Visible = true;
				continueB.Visible = isContinue;
				buttons[i].ToNormal();
			}
			backgroundTextVisible = true;
		}

		private static Sound3D CreateSound3D(ContentManager content, string fname) {
			Sound sound = new Sound(content);
			sound.LoadSound(fname);
			return sound.Create3D();
		}

		private static Sound CreateSound(ContentManager content, string fname) {
			Sound sound = new Sound(content);
			sound.LoadSound(fname);
			return sound;
		}

		#endregion
	}
}
