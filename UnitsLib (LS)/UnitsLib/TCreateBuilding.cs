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
using UnitsLib.Exceptions;

namespace UnitsLib
{
	/// <summary>
	/// Класс создающее здание
	/// </summary>
	public abstract class TCreateBuilding : TBuilding, IQueue, ICreate, IPoint
	{
		Flag flag = World.Flag;

		protected Queue<UnitNames> q;
		protected int progress, cprogress; //Max progress, current progress
		protected int progressIncrement = 1;
		protected Vector2 p;

		/// <summary>
		/// Очередь создания
		/// </summary>
		public Queue<UnitNames> Q {
			get { return q; }
			set { SetQ(value); }
		}
		/// <summary>
		/// Текущий прогресс создания
		/// </summary>
		public int Progress {
			get { return cprogress; }
			set {
				if (value < 0)
					value = 0;
				if (value > progress)
					value = progress;
				cprogress = value;
			}
		}
		/// <summary>
		/// Максимальный прогресс создания
		/// </summary>
		public int MaxProgress { get { return progress; } }
		/// <summary>
		/// Точка, куда идут созданные юниты после создания
		/// </summary>
		public Vector2 P {
			get { return p; }
			set {
				if (value.X < Position.X - 30
				 || value.X > Position.X + Width + 30
				 || value.Y < Position.Y - 30
				 || value.Y > Position.Y + Height + 30)
					p = value;
			}
		}

		public TCreateBuilding(float x, float y, int hp, int armor, bool side, MultiSprite image)
			: base(x, y, hp, armor, side, image) {
			p = new Vector2(round(Center.X), round(Center.Y + Height));
			q = new Queue<UnitNames>(World.MAX_QUEUE_SIZE);
			progress = World.INIT_PROGRESS;
			cprogress = 0;
			this.Death += new EventHandler(TCreateBuilding_Death);
			this.Focused += new EventHandler<FocusedEventArgs>(TCreateBuilding_Focused);
		}
		public TCreateBuilding(TCreateBuilding g)
			: base(g) {
			p = g.p;
			q = new Queue<UnitNames>(World.MAX_QUEUE_SIZE);
			progress = g.progress;
			cprogress = 0;
			this.Death += new EventHandler(TCreateBuilding_Death);
			this.Focused += new EventHandler<FocusedEventArgs>(TCreateBuilding_Focused);
		}

		public override void Dispose() {
			base.Dispose();
			q.Clear();
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime time, Scene scene) {
			base.Draw(spriteBatch, time, scene);
			if (IsFocused)
				this.CheckFlag(scene);
		}

		protected abstract void SetQ(Queue<UnitNames> Q);
		public abstract UnitNames FromQueue();
		public abstract void ToQueue(UnitNames unitName);
		public abstract void Create();

		public virtual void Clear() {
			if (Q.Count > 0) {
				int gold = QueueAnalyse(q);
				if (Side)
					World.CResource.Gold += gold;
				else
					World.EResource.Gold += gold;
				q.Clear();
				Progress = 0;
			}
		}

		/// <summary>
		/// Проверка установки флага
		/// </summary>
		public void CheckFlag(Scene scene) {
			if (Side && this is IPoint) {
				flag.Position = new Vector2(p.X - 5 - scene.Rect.X, p.Y - 23 - scene.Rect.Y);
				flag.Visible = true;
			} else
				flag.Visible = false;
		}

		private void TCreateBuilding_Focused(object sender, FocusedEventArgs e) {
			if (!e.Focused)
				flag.Visible = false;
		}
		private void TCreateBuilding_Death(object sender, EventArgs e) {
			if (IsFocused)
				flag.Visible = false;
		}

		private int QueueAnalyse(Queue<UnitNames> Q) {
			int result = 0;
			foreach (var unit in Q) {
				switch (unit) {
					case UnitNames.Swordman:
						result += World.SWORDMAN_COST;
						break;
					case UnitNames.Worker:
						result += World.WORKER_COST;
						break;
					case UnitNames.Archer:
						//result += World.ArcherCost;
						break;
					case UnitNames.Peasant:
						result += World.PEASANT_COST;
						break;
					case UnitNames.Undefined:
						throw new UndefinedUnitNameException("Встречено неопределенное имя юнита");
				}
			}
			return result;
		}
	}
}
