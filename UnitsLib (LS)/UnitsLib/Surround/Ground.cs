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
using UnitsLib.Events;
using UnitsLib.Enums;
using UnitsLib.Interfaces;

namespace UnitsLib.Surround
{
	public class Ground
	{
		Rectangle field;
		Dictionary<Sprite, Vector2> tiles;
		Vector2 drawPosition;

		public Dictionary<Sprite, Vector2> Tiles { get { return tiles; } }
		internal Texture2D ground;

		public Rectangle Field {
			get { return field; }
		}
		public GroundTypes GroundType { get; private set; }

		public Ground(Rectangle field, string groundFname) {
			this.field = field;
			ground = World.Content.Load<Texture2D>(groundFname);
			int w = ground.Width;
			int h = ground.Height;
			int countX = field.Width / w;
			int countY = field.Height / h;
			tiles = new Dictionary<Sprite, Vector2>(countX * countY);
			for (int i = 0; i <= countY; i++) {
				for (int j = 0; j <= countX; j++) {
					Sprite sprite = new Sprite(World.Content);
					sprite.LoadTexture(groundFname, new Vector2(j * w, i * h), new Vector2(w, h));
					sprite.Visible = false;
					tiles.Add(sprite, sprite.Position);
				}
			}
			switch (groundFname) {
				case Fnames.Dust:
					GroundType = GroundTypes.Dust;
					break;
				case Fnames.Grass:
					GroundType = GroundTypes.Grass;
					break;
				case Fnames.Snow:
					GroundType = GroundTypes.Snow;
					break;
				default:
					GroundType = GroundTypes.Snow;
					break;
			}
		}

		public void Draw(SpriteBatch spriteBatch, Scene scene) {
			foreach (var tile in tiles.Keys) {
				if (scene.Contains(new Rectangle((int)tiles[tile].X,(int)tiles[tile].Y,ground.Width, ground.Height))) {
					tile.Visible = true;
					InternalDraw(tile, scene);
					tile.Draw(spriteBatch);
				} else
					tile.Visible = false;
			}
		}

		private void InternalDraw(Sprite sprite, Scene scene) {
			drawPosition = new Vector2(tiles[sprite].X - scene.Rect.X, tiles[sprite].Y - scene.Rect.Y);
			sprite.Position = drawPosition;
		}
	}

	/// <summary>
	/// Типы тайла
	/// </summary>
	public enum GroundTypes
	{
		/// <summary>
		/// Грязь
		/// </summary>
		Dust,
		/// <summary>
		/// Трава
		/// </summary>
		Grass,
		/// <summary>
		/// Темная трава
		/// </summary>
		DarkGrass,
		/// <summary>
		/// Снег
		/// </summary>
		Snow,
	}
}
