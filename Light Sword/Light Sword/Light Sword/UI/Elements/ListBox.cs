using System;
using System.Collections;
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
	public class ListBox : Element
	{
		public const int MAX_ELEMENTS = 10;

		#region Variables

		MultiSprite mesh;
		SpriteFont font;

		object selectedItem = null;
		object[] items = new object[MAX_ELEMENTS];
		IEnumerable<object> list;

		int start = 25;
		int step;
		int count;

		#endregion

		public object this[int index] {
			get {
				if (!indexIsValide(index))
					throw new ArgumentOutOfRangeException("Индекс находится за пределами диапазона значений списка элементов");
				return items[index];
			}
			set {
				if (!indexIsValide(index))
					throw new ArgumentOutOfRangeException("Индекс находится за пределами диапазона значений списка элементов");
				items[index] = value;
			}
		}

		public override bool Visible {
			get {
				return base.Visible;
			}
			set {
				base.Visible = value;
				if (value) {
					selectedItem = null;
					mesh.Visible = false;
				}
			}
		}

		public ListBox(MultiSprite image, MultiSprite meshSprite, SpriteFont font)
			: base(image) {
			mesh = meshSprite;
			mesh.Visible = false;
			step = (int)font.MeasureString("ASDFGHJKL:QWERTYUIOP{}ZXCVBNM<>?1234567890-=").Y + 4;
			this.font = font;
			this.Click += new EventHandler<MouseElementEventArgs>(ListBox_Click);
		}

		public override void Update(GameTime gameTime) {
			list = from item in items
				   where item != null
				   select item;
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			if (Visible) {
				count = 0;
				foreach (var item in list) {
					spriteBatch.DrawString(font, (count + 1).ToString() + ") " + item.ToString(), new Vector2(Position.X + Bounds.Width / 1000f * 100, Position.Y + start + step * count++), Color.White);
				}
				if (selectedItem != null)
					mesh.Draw(spriteBatch);
				//for (int i = 0; i < items.Length; i++) {
				//    if (items[i] == null)
				//        continue;

				//}
			}
		}

		#region Private Methods

		private bool indexIsValide(int index) {
			return index >= 0 && index < MAX_ELEMENTS;
		}

		private void ListBox_Click(object sender, MouseElementEventArgs e) {
			count = 0;
			bool isFind = false;
			object[] arr = list.ToArray();
			for (int i = 0; i < arr.Length; i++) {
				Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y + start + count++ * step, Bounds.Width, step);
				if (rect.Contains((int)e.Position.X, (int)e.Position.Y)) {
					selectedItem = arr[i];
					mesh.Position = new Vector2(rect.X, rect.Y);
					mesh.Size = new Vector2(rect.Width, rect.Height);
					mesh.Visible = true;
					isFind = true;
				}
			}
			if (!isFind) {
				mesh.Visible = false;
				selectedItem = null;
			}
		}

		#endregion
	}
}
