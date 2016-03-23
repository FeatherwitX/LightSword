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
using UnitsLib.Interfaces;
using UnitsLib.Enums;
using UnitsLib.Events;

namespace UnitsLib.Surround
{
	public class IcoWithHP
	{
		Sprite image;
		hpRect rect;
		bool vis;

		static Texture2D hpTexture = World.Content.Load<Texture2D>(Fnames.Hp);
		static Texture2D hpBorder = World.Content.Load<Texture2D>(Fnames.HpBorder);

		internal Sprite Image { get { return image; } }
		internal Vector2 Position {
			get { return image.Position; }
			set {
				Vector2 prevPos = image.Position;
				image.Position = value;
				rect.MoveOn(value - prevPos);
			}
		}
		internal Vector2 Center {
			get { return Position + (Size / 2); }
			set { Position = value - (Size / 2); }
		}
		internal Vector2 Size {
			get { return image.Size; }
			set { image.Size = value; }
		}
		internal bool Visible {
			get { return vis; }
			set {
				vis = value;
				image.Visible = vis;
			}
		}
		internal hpRect HPRect {
			get { return rect; }
			set { rect = value; }
		}

		internal IcoWithHP(Sprite image) {
			this.image = image;
			rect = new hpRect();
			vis = true;
		}
		internal IcoWithHP(Sprite image, hpRect rect) {
			this.image = image;
			this.rect = rect;
			vis = true;
		}

		internal void SetHp(int max, int current) {
			rect.Width = (current * rect.InitWidth) / max;
		}

		internal void Draw(SpriteBatch spriteBatch) {
			if (vis) {
				image.Draw(spriteBatch);
				if (rect.Vis) {
					spriteBatch.Draw(hpBorder, rect.Border, new Rectangle(0, 0, rect.Border.Width, rect.Border.Height), rect.BorderColor);
					spriteBatch.Draw(hpTexture, rect.Rect, new Rectangle(0, 0, rect.Width, rect.Height), rect.Color);
				}
			}
		}
	}
}
