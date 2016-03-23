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
using UnitsLib.Interfaces;
using UnitsLib.Surround;
using UnitsLib.Enums;
using UnitsLib.Events;

namespace UnitsLib
{
	/// <summary>
	/// Класс здание. Предок всех зданий
	/// </summary>
	public abstract class TBuilding : TObject, IHP, IArmor, ISide
	{
		bool side; //True - Controlled. False - Enemys
		int hp, chp; //Max hp, current hp
		int armor;

		hpRect rect;
		static Texture2D hpTexture = World.Content.Load<Texture2D>(Fnames.Hp);
		static Texture2D hpBorder = World.Content.Load<Texture2D>(Fnames.HpBorder);

		internal static List<TBuilding> lB = World.TBuildings;

		/// <summary>
		/// Сторона, которую занимает здание
		/// True - подконтрольно
		/// False - нет
		/// </summary>
		public bool Side { get { return side; } }
		/// <summary>
		/// Броня здания
		/// </summary>
		public int Armor {
			get { return armor; }
			set {
				if (value < 0)
					value = 0;
				armor = value;
			}
		}
		/// <summary>
		/// Текущее здоровье здания
		/// </summary>
		public int HP {
			get { return chp; }
			set {
				if (value < 0)
					value = 0;
				if (value > hp)
					value = hp;
				chp = value;
				if (chp > 0) {
					rect.Width = (int)((chp * rect.InitWidth) / hp);
					if (chp < hp / 2)
						Image.FrameCurrent = new Vector2(1, 0);
				} else {
					rect.Vis = false;
					Dead();
				}
				if (rect.Width < (rect.InitWidth / 2))
					rect.Color = Color.Red;
			}
		}
		/// <summary>
		/// Максимальное здоровье здания
		/// </summary>
		public int MaxHP { get { return hp; } }

		/// <summary>
		/// Создает новое здание с указанными параметрами
		/// </summary>
		/// <param name="x">Расстояние по оси X</param>
		/// <param name="y">Расстояние по оси Y</param>
		/// <param name="hp">Здоровье здания</param>
		/// <param name="armor">Броня здания</param>
		/// <param name="side">Сторона, которую занимает здание</param>
		/// <param name="image">Изображение здания</param>
		public TBuilding(float x, float y, int hp, int armor, bool side, MultiSprite image)
			: base(x, y, image) {
			if (hp <= 0)
				hp = 1;
			if (armor < 0)
				armor = 0;
			this.hp = hp;
			chp = this.hp;
			this.armor = armor;
			this.side = side;
			rect = new hpRect(new Rectangle((int)Center.X - 15, (int)(Top + Height + 1), 30, 5));
			this.Focused += new EventHandler<FocusedEventArgs>(TBuilding_Focused);

			lB.Add(this);
		}
		/// <summary>
		/// Создает новое здание - копию g
		/// </summary>
		public TBuilding(TBuilding g)
			: base(g) {
			hp = g.hp;
			chp = g.chp;
			side = g.side;
			armor = g.armor;
			rect = g.rect;
			this.Focused += new EventHandler<FocusedEventArgs>(TBuilding_Focused);

			lB.Add(this);
		}

		/// <summary>
		/// Удаляет здание
		/// </summary>
		public override void Dispose() {
			base.Dispose();
			lB.Remove(this);
		}

		/// <summary>
		/// Смерть здания
		/// </summary>
		protected override void Dead() {
			Image.FrameCurrent = new Vector2(Image.FrameCount.X - 1, 0);
			if (!World.DeadBuildings.ContainsKey(Image))
				World.DeadBuildings.Add(Image, Position);
			base.Dead();
			lB.Remove(this);
		}

		/// <summary>
		/// Визуализация здания
		/// </summary>
		public override void Draw(SpriteBatch spriteBatch, GameTime time, Scene scene) {
			if (scene.Contains(this)) {
				Visible = true;
				rect.Vis = true;
				InternalDraw(scene);
			} else {
				Visible = false;
				rect.Vis = false;
			}
			Image.Draw(spriteBatch);
			if (rect.Vis) {
				spriteBatch.Draw(hpBorder, rect.Border, new Rectangle(0, 0, rect.Border.Width, rect.Border.Height), rect.BorderColor);
				spriteBatch.Draw(hpTexture, rect.Rect, new Rectangle(0, 0, rect.Width, rect.Height), rect.Color);
			}
		}

		/// <summary>
		/// Внутренняя визуализация
		/// </summary>
		protected override void InternalDraw(Scene scene) {
			base.InternalDraw(scene);
			rect.MoveTo((int)(drawPosition.X + (Width - rect.InitWidth) / 2), (int)(drawPosition.Y + Height + 1));
		}

		private void TBuilding_Focused(object sender, FocusedEventArgs e) {
			if (e.Focused) {
				rect.BorderColor = Color.White;
			} else {
				rect.BorderColor = Color.Transparent;
			}
		}
	}
}
