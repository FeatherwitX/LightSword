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
	/// <summary>
	/// Класс подконтрольный юнит
	/// </summary>
	public abstract class TCUnit : TUnit
	{
		private Cell<TCUnit> cell;
		internal static List<TCUnit> lCU = World.TCUnits;

		/// <summary>
		/// Создает объект TCUnit с указанными параметрами
		/// </summary>
		/// <param name="x">Расстояние по оси X</param>
		/// <param name="y">Расстояние по оси Y</param>
		/// <param name="hp">Здоровье юнита</param>
		/// <param name="attack">Атака юнита</param>
		/// <param name="armor">броня юнита</param>
		/// <param name="v">Вектор скорости юнита</param>
		/// <param name="image">Изображение юнита</param>
		public TCUnit(float x, float y, int hp, int attack, int armor, Vector2 v, MultiSprite image)
			: base(x, y, hp, v, image) {
			if (attack <= 0) attack = 1;
			if (armor < 0) armor = 0;
			this.attack = attack;
            this.armor = armor;

            ComparePriority = 5;

			cell = World.CUGrid.AddObject(this);
			lCU.Add(this);
		}
		/// <summary>
		/// Создает новый объекта TCUnit - копию g
		/// </summary>
		public TCUnit(TCUnit g)
			: base(g) {
			attack = g.attack;
            armor = g.armor;

            ComparePriority = 5;

			cell = World.CUGrid.AddObject(this);
			lCU.Add(this);
		}

		/// <summary>
		/// Удаляет подконтрольного юнита
		/// </summary>
		public override void Dispose() {
			base.Dispose();
			World.CUGrid.RemoveObject(this);
			lCU.Remove(this);
		}

		/// <summary>
		/// Юнит атакует свой Target или BTarget
		/// </summary>
		public override void Attack() {
			if (Target is TEnemy || (BTarget is TBuilding && !BTarget.Side))
				base.Attack();
		}

		/// <summary>
		/// Юнит проверяет территорию вокруг себя в радиусе агрессии и, если находит врага,
		/// атакует его
		/// </summary>
		public override void CheckArea() {
			if (checkAreaT == World.CHECK_AREA_TIMER) {
				TEnemy tE = null;
				if (AgrMode == AgrModes.Aggressive) {
					tE = World.FindClosestTEnemy(this, AgrR);
				}
				if (tE != null) {
					Target = tE;
					P = new Vector2(Target.Center.X, Target.Center.Y);
					Aim = Aims.Attack;
				}
				checkAreaT = 0;
			} else
				checkAreaT++;
		}

		protected override void CheckGridChange() {
			if (!cell.GridRect.Contains(new Point((int)Position.X, (int)Position.Y))) {
				cell.RemoveObject(this);
				cell = World.CUGrid.AddObject(this);
			}
		}

		/// <summary>
		/// Смерть подконтрольного юнита юнита
		/// </summary>
		protected override void Dead() {
			base.Dead();
			World.CUGrid.RemoveObject(this);
			lCU.Remove(this);
		}
	}
}
