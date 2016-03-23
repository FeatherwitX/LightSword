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
using UnitsLib.Enums;
using UnitsLib.Interfaces;
using UnitsLib.Surround;

namespace UnitsLib
{
	public class TEnemy : TUnit
	{
		private Cell<TEnemy> cell;
		internal static List<TEnemy> lE = World.TEnemys;

		public TEnemy(float x, float y, int hp, int attack, int armor, Vector2 v, MultiSprite image)
			: base(x, y, hp, v, image) {
			if (attack <= 0) attack = 1;
			if (armor < 0) armor = 0;
			this.attack = attack;
			this.armor = armor;

			rect = new hpRect(new Rectangle((int)Center.X - 15, (int)(Top + Height - 5), 30, 5)); //TODO убрать после реализации производных
            
            ComparePriority = 4;

			cell = World.EGrid.AddObject(this);
			lE.Add(this);
		}
		public TEnemy(TEnemy g)
			: base(g) {
			attack = g.attack;
            armor = g.armor;

            ComparePriority = 4;

			cell = World.EGrid.AddObject(this);
			lE.Add(this);
		}

		public override void Dispose() {
			base.Dispose();
			World.EGrid.RemoveObject(this);
			lE.Remove(this);
		}

		public override void Attack() {
			if (Target is TCUnit || (BTarget is TBuilding && BTarget.Side))
				base.Attack();
		}

		public override void CheckArea() {
			if (checkAreaT == World.CHECK_AREA_TIMER) {
				TCUnit tCU = null;
				if (AgrMode == AgrModes.Aggressive) {
					tCU = World.FindClosestTCUnit(this, AgrR);
				}
				if (tCU != null) {
					Target = tCU;
					P = new Vector2(Target.Center.X, Target.Center.Y);
					Aim = Aims.Attack;
				}
				checkAreaT = 0;
			} else
				checkAreaT++;
		}

		public override object Clone() {
			return new TEnemy(this);
		}

		protected override void Dead() {
			base.Dead();
			World.EGrid.RemoveObject(this);
			lE.Remove(this);
		}

		protected override void CheckGridChange() {
			if (!cell.GridRect.Contains(new Point((int)Position.X, (int)Position.Y))) {
				cell.RemoveObject(this);
				cell = World.EGrid.AddObject(this);
			}
		}
	}
}
