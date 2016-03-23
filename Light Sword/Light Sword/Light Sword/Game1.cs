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
using UnitsLib;
using UnitsLib.Enums;
using UnitsLib.Surround;
using UnitsLib.Events;
using UnitsLib.Interfaces;
using SettingsLib;

namespace Light_Sword
{
	public partial class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		internal Screen CurrentScreen;
		Settings settings;
		AnalysedSettings cfg;

		bool isActive;
		bool isFullScreen = false;

		public bool IsFullSreen {
			get { return isFullScreen; }
			set {
				isFullScreen = value;
				if (isFullScreen)
					Init.FullScreen(graphics);
				else
					Init.NotFullScreen(graphics);
			}
		}

		public Game1(Settings settings) {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			World.Content = Content;
			this.settings = settings;
			this.Activated += new EventHandler<EventArgs>(Game1_Activated);
			this.Deactivated += new EventHandler<EventArgs>(Game1_Deactivated);
			isActive = true;
		}

		protected override void Initialize() {
			cfg = new AnalysedSettings(settings);
			//Init.SetResolution(graphics, 800, 800);
			Init.FullScreen(graphics);
			Init.SetResolution(graphics, cfg.ResolutionWidth, cfg.ResolutionHeight);
			CurrentScreen = new MainMenuScreen(this, settings);

			base.Initialize();
		}

		protected override void LoadContent() {

			base.LoadContent();
		}

		protected override void UnloadContent() {

		}

		protected override void Update(GameTime gameTime) {
			if (isActive) {
				CurrentScreen.Update(gameTime);
				base.Update(gameTime);
			}
		}

		protected override void Draw(GameTime gameTime) {			
			if (isActive) {
				CurrentScreen.Draw(gameTime);
				base.Draw(gameTime);
			}
		}

		#region Private Methods

		private void Game1_Activated(object sender, EventArgs e) {
			isActive = true;
		}

		private void Game1_Deactivated(object sender, EventArgs e) {
			isActive = false;
		}

		#endregion
	}
}
