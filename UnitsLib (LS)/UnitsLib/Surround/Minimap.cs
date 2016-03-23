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
using UnitsLib.Events;
using UnitsLib.Interfaces;


namespace UnitsLib.Surround
{
	/// <summary>
	/// Миникарта
	/// </summary>
	public class Minimap : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Texture2D[] tiles;
		Texture2D back;
		Rectangle map;
		Rectangle field;
		Vector2 scaleFactor;
		bool focused;

		MouseState ms, oms;
		bool lb, rb;

		SpriteBatch spriteBatch;

		static Texture2D texture = World.Content.Load<Texture2D>(Fnames.Mesh);
		static Texture2D rect = World.Content.Load<Texture2D>(Fnames.MinimapSceneRect);

		/// <summary>
		/// Событие щелчка мыши
		/// </summary>
		public event EventHandler<MinimapMouseEventArgs> Click;

		/// <summary>
		/// Создает новый экземпляр Minimap с указанными параметрами
		/// </summary>
		/// <param name="game">Игра к которой привязан компонент</param>
		/// <param name="fieldRect">Размер поля</param>
		/// <param name="mapRect">Размер миникарты</param>
		/// <param name="tiles">Словарь тайлов карты</param>
		public Minimap(Game game, Rectangle fieldRect, Rectangle mapRect, Dictionary<Sprite, Vector2> tiles)
			: base(game) {
			map = mapRect;
			map.X -= 2;
			field = fieldRect;
			scaleFactor = new Vector2((float)map.Width / (float)field.Width, (float)map.Height / (float)field.Height);
			this.tiles = new Texture2D[tiles.Count];
			int i = 0;
			foreach (var sprite in tiles.Keys) {
				this.tiles[i++] = sprite.GetTexture();
			}
		}

		public override void Initialize() {

			base.Initialize();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			back = RenderInOneTexture(this.tiles);

			base.LoadContent();
		}

		public override void Update(GameTime gameTime) {
			oms = ms;
			ms = Mouse.GetState();
			Vector2 pos = new Vector2(ms.X - map.X, ms.Y - map.Y);
			if (ms.LeftButton == ButtonState.Pressed && oms.LeftButton == ButtonState.Released && map.Contains(new Point(ms.X, ms.Y))) {
				lb = true;
				OnClick(new MinimapMouseEventArgs(pos, ms));
			}
			if (ms.RightButton == ButtonState.Pressed && oms.RightButton == ButtonState.Released && map.Contains(new Point(ms.X, ms.Y))) {
				rb = true;
			}
			if (ms.LeftButton == ButtonState.Released && oms.LeftButton == ButtonState.Pressed) {
				lb = false;
			}
			if (ms.RightButton == ButtonState.Released && oms.RightButton == ButtonState.Pressed) {
				rb = false;
			}
			if (lb && ms.LeftButton == ButtonState.Pressed) {
				if (map.Contains(new Point(ms.X,ms.Y))) {
					pos *= new Vector2(field.Width / map.Width, field.Height / map.Height);
					OnClick(new MinimapMouseEventArgs(pos, ms));
				}
			}
			if (rb && ms.RightButton == ButtonState.Pressed && oms.RightButton == ButtonState.Released) {
				if (map.Contains(new Point(ms.X, ms.Y))) {
					pos *= new Vector2(field.Width / map.Width, field.Height / map.Height);
					OnClick(new MinimapMouseEventArgs(pos, ms));
				}
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			spriteBatch.Begin();
			spriteBatch.Draw(back, map, Color.White);
			foreach (var obj in World.TObjects) {
				Vector2 pos = new Vector2(map.X + obj.Left * scaleFactor.X, map.Y + obj.Top * scaleFactor.Y);
				pos = (pos.X > map.Right) ? new Vector2(map.Right - 2, pos.Y) : pos;
				pos = (pos.Y > map.Bottom) ? new Vector2(pos.X, map.Bottom - 2) : pos;
				if (obj.IsFocused)
					focused = true;
				else
					focused = false;
				if (obj is TCUnit) {
					spriteBatch.Draw(texture, pos, null, (focused) ? Color.White : Color.Blue, 0, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 0);
					continue;
				}
				if (obj is TEnemy) {
					spriteBatch.Draw(texture, pos, null, (focused) ? Color.White : Color.Red, 0, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 0);
					continue;
				}
				if (obj is TBuilding) {
					if ((obj as TBuilding).Side)
						spriteBatch.Draw(texture, pos, null, (focused) ? Color.White : Color.Blue, 0, Vector2.Zero, new Vector2(4, 4), SpriteEffects.None, 0);
					else
						spriteBatch.Draw(texture, pos, null, (focused) ? Color.White : Color.Red, 0, Vector2.Zero, new Vector2(4, 4), SpriteEffects.None, 0);
					continue;
				}
				if (obj is TTree) {
					spriteBatch.Draw(texture, pos, null, (focused) ? Color.White : Color.DarkGreen, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);
					continue;
				}
				if (obj is TStone) {
					spriteBatch.Draw(texture, pos, null, (focused) ? Color.White : Color.DarkGray, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);
				}
			}
			spriteBatch.Draw(rect, new Rectangle((int)(map.X + World.Scene.Rect.X * scaleFactor.X), (int)(map.Y + World.Scene.Rect.Y * scaleFactor.Y - 1), map.Width * World.Scene.Rect.Width / field.Width, map.Height * World.Scene.Rect.Height / field.Height - map.Height * (int)World.Panel.BottomPanelSize.Y / field.Height), Color.Black);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		/// <summary>
		/// Возвращает новую сцену, перенесенную в позицию position
		/// </summary>
		/// <param name="position">Позиция центра новой сцены в мировых координатах</param>
		public Scene SetScene(Vector2 position) {
			Scene result = new Scene(World.Field, World.Panel);
			result.Rect = new Rectangle((int)position.X - result.Rect.Width / 2, (int)position.Y - result.Rect.Height / 2, result.Rect.Width, result.Rect.Height);
			return result;
		}

		/// <summary>
		/// Переводит текстуры из массива в двумерный набор усредненных цветов, затем
		/// рендерит этот набор цветов на текстуру back
		/// </summary>
		private Texture2D RenderInOneTexture(Texture2D[] textures) {
			if (textures.Length == 0)
				return null;
			int w = textures[0].Width;
			int h = textures[0].Height;
			int m = field.Width / w;
			int n = field.Height / h;
			Vector2 scale = new Vector2((float)map.Width / (float)field.Width, (float)map.Height / (float)field.Height);
			Color[,] newColors = new Color[m, n];
			for (int i = 0; i < m; i++) {
				for (int j = 0; j < n; j++) {
					Color[] c = new Color[w * h];
					textures[(m + 1) * j + i].GetData<Color>(c);
					newColors[i, j] = AverrageColor(c);
				}
			}
			RenderTarget2D resultTexture = new RenderTarget2D(GraphicsDevice, map.Width, map.Height);
			GraphicsDevice.SetRenderTarget(resultTexture);
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			for (int i = 0; i < m; i++) {
				for (int j = 0; j < n; j++) {
					spriteBatch.Draw(texture, new Vector2(i * w * scale.X, j * h * scale.Y), null, newColors[i, j], 0, Vector2.Zero, scale * 64, SpriteEffects.None, 0);
				}
			}
			spriteBatch.End();
			GraphicsDevice.SetRenderTarget(null);
			return resultTexture;
		}

		private Color AverrageColor(Color[] colors) {
			float r, g, b;
			r = g = b = 0;
			int l = colors.Length;
			for (int i = 0; i < l; i++) {
				r += colors[i].R;
				g += colors[i].G;
				b += colors[i].B;
			}
			r /= l;
			g /= l;
			b /= l;
			return new Color((int)r, (int)g, (int)b);
		}

		/// <summary>
		/// Позиция мыши относительно мира, а не левого верхенго угла окна
		/// </summary>
		/// <param name="ms">Текущее состояние мыши</param>
		private Vector2 MousePosition(MouseState ms) {
			return new Vector2(ms.X + World.Scene.Rect.X, ms.Y + World.Scene.Rect.Y);
		}

		private void OnClick(MinimapMouseEventArgs e) {
			EventHandler<MinimapMouseEventArgs> click = Click;
			if (click != null)
				click(this, e);
		}

		//private void game_Activated(object sender, EventArgs e) {
		//    Game.Components.RemoveAt(0);
		//    back = RenderInOneTexture(this.tiles);
		//}
	}
}
