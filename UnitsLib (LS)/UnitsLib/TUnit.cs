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
using UnitsLib.Events;
using UnitsLib.Surround.Algorithms;

namespace UnitsLib
{
	/// <summary>
	/// Класс юнит. Предок всех юнитов
	/// </summary>
	public abstract class TUnit : TObject, IAttack, IArmor, IMove, IHP, IQueuePoint
	{
		bool isBypass;

		Vector2 p;
		Vector2 currentP;
		Queue<Vector2> qp = new Queue<Vector2>();
		float length;
		int hp, chp; //Max hp, current hp
		protected int attack, armor;
		protected Vector2 v;
		protected float speed = 1.7f;
		protected int attackT; //Attack timer (local)
		protected int checkAreaT; //Check timer (local)
		protected float direction;
        /// <summary>
        /// Must be defined in derived classes
        /// </summary>
        protected UnitNames uname = UnitNames.Undefined;
		TUnit target;
		TBuilding bTarget;
		Vector2 targetPos;
		Aims aim;
		AgrModes agrMode;
		int agrR = World.CELL_W;
		Flag flag = World.Flag;

		protected hpRect rect;
		static Texture2D hpTexture = World.Content.Load<Texture2D>(Fnames.Hp);
		static Texture2D hpBorder = World.Content.Load<Texture2D>(Fnames.HpBorder);

		internal static List<TUnit> lU = World.TUnits;

		/// <summary>
		/// Имя юнита (только для чтения)
		/// </summary>
		public UnitNames UnitName { get { return uname; } }
		/// <summary>
		/// Цель юнита
		/// </summary>
		public virtual Aims Aim {
			get { return aim; }
			set {
				Aims prevAim = aim;
				aim = value;
				if (prevAim != aim)
					OnAimChanged(new AimEventArgs(aim));
				if (prevAim == Aims.AttackNow && aim != Aims.AttackNow) {
					Image.FrameCurrent = new Vector2(Image.FrameCurrent.X, Image.FrameCurrent.Y - 1);
					Image.FramesPerSecond += 3;
				}
				if (aim == Aims.AttackNow && prevAim != Aims.AttackNow) {
					Image.FrameCurrent = new Vector2(Image.FrameCurrent.X, Image.FrameCurrent.Y + 1);
					Image.FramesPerSecond -= 3;
				}
			}
		}
		/// <summary>
		/// Мод агрессии юнита
		/// </summary>
		public AgrModes AgrMode {
			get { return agrMode; }
			set { agrMode = value; }
		}
		/// <summary>
		/// Радиус агрессии юнита (только для чтения)
		/// </summary>
		public int AgrR {
			get { return agrR; }
		}
		/// <summary>
		/// Атака юнита
		/// </summary>
		public int Atack {
			get { return attack; }
			set {
				if (value < 0)
					value = 0;
				attack = value;
			}
		}
		/// <summary>
		/// Броня юнита
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
		/// Вектор скорости юнита
		/// </summary>
		public Vector2 Velocity {
			get { return v; }
			set {
				v = value;
				if (v == Vector2.Zero && aim != Aims.AttackNow)
					Image.Animation = false;
				else
					Image.Animation = true;
			}
		}
		/// <summary>
		/// Коэффициент скорости юнита
		/// </summary>
		public float Speed {
			get { return speed; }
			set { speed = value; }
		}
		/// <summary>
		/// Target юнита
		/// </summary>
		public TUnit Target {
			get { return target; }
			set {
				if (value is TUnit) {
					BTarget = null;
					if (target != null)
						target.Death -= target_Death;
					target = value;
					targetPos = new Vector2(target.Center.X, target.Center.Y);
					target.Death += new EventHandler(target_Death);
				} else
					target = null;
			}
		}
		/// <summary>
		/// BTarget юнита
		/// </summary>
		public TBuilding BTarget {
			get { return bTarget; }
			set {
				if (value is TBuilding) {
					Target = null;
					if (bTarget != null)
						bTarget.Death -= bTarget_Death;
					bTarget = value;
					targetPos = new Vector2(bTarget.Center.X, bTarget.Center.Y);
					bTarget.Death +=new EventHandler(bTarget_Death);
				} else
					bTarget = null;
			}
		}
		/// <summary>
		/// Здоровье юнита
		/// </summary>
		public int HP {
			get { return chp; }
			set {
				if (value < 0)
					value = 0;
				if (value > hp)
					value = hp;
				chp = value;
				if (chp > 0)
					rect.Width = (chp * rect.InitWidth) / hp;
				else {
					rect.Vis = false;
					Dead();
				}
				if (rect.Width < (rect.InitWidth / 2))
					rect.Color = Color.Red;
			}
		}
		/// <summary>
		/// Максимальное хп юнита, установленное при создании (только для чтения)
		/// </summary>
		public int MaxHP { get { return hp; } }
		/// <summary>
		/// Точка, куда движется юнит
		/// </summary>
		public Vector2 P { //TODO установка точки не на непроходимый объект, а в близжайшее проходимое место
			get { return p; }
			set {
				if (!World.AGrid.GetCell(p.X, p.Y).IsPassable)
					value = ModifiedAStar.GetCellWithMinH(World.AGrid.GetCell(p.X, p.Y)).Center;
				p = value;
				qp.Clear();
				qp.Enqueue(p);
				currentP = qp.Dequeue();
			}
		}
		/// <summary>
		/// Очередь точек движения юнита
		/// </summary>
		public Queue<Vector2> QP {
			get { return qp; }
			set { qp = value; }
		}
		/// <summary>
		/// Угол направления юнита в радианах
		/// </summary>
		public float Direction {
			get { return direction; }
			set {
				float prevDirection = direction;
				direction = value;
				if (prevDirection != direction)
					ChangeImageOfDirection(direction);
			}
		}

		/// <summary>
		/// Событие изменения цели
		/// </summary>
		public event EventHandler<AimEventArgs> AimChanged;

		/// <summary>
		/// Создает объект TUnit с указанными параметрами
		/// </summary>
		/// <param name="x">Позиция по оси X</param>
		/// <param name="y">Позиция по оси Y</param>
		/// <param name="hp">Максимальное здоровье</param>
		/// <param name="v">Скорость</param>
		/// <param name="image">Изображение</param>
		public TUnit(float x, float y, int hp, Vector2 v, MultiSprite image)
			: base(x, y, image) {
			if (hp <= 0)
				hp = 1;
			if (v.X < 0 || v.Y < 0)
				v = Vector2.Zero;
			this.hp = hp;
			chp = hp;
			aim = Aims.Stand;
			agrMode = AgrModes.Aggressive;
			this.v = v;
			//rect = new hpRect(new Rectangle((int)Center.X - 15, (int)(Top + Height + 1), 30, 5));
			p = new Vector2(Left + Width / 2, Top + Height / 2);
			currentP = p;
			qp.Enqueue(p);
			this.Focused += new EventHandler<FocusedEventArgs>(TUnit_Focused);
            this.Death += new EventHandler(TUnit_Death);

            ComparePriority = 3;

			lU.Add(this);
		}
		/// <summary>
		/// Создает объект TUnit - точную копию g
		/// </summary>
		public TUnit(TUnit g)
			: base(g) {
			hp = g.hp;
			chp = g.chp;
			rect = g.rect;
			rect.Location = new Point((int)Left, (int)(Top + Height + 1));
			v = g.v;
			agrMode = g.agrMode;
			aim = g.aim;
			agrR = g.agrR;
			p = new Vector2(Left + Width / 2, Top + Height / 2);
			currentP = p;
			qp = g.qp;
			this.Focused += new EventHandler<FocusedEventArgs>(TUnit_Focused);
            this.Death += new EventHandler(TUnit_Death);

            ComparePriority = 3;

			lU.Add(this);
		}

		/// <summary>
		/// Освобождает все внутренние ресурсы юнита и удаляет его
		/// </summary>
		public override void Dispose() {
			base.Dispose();
			lU.Remove(this);
			target = null;
			bTarget = null;
		}

		/// <summary>
		/// Смерть юнита
		/// </summary>
		protected override void Dead()  {
			this.Center = new Vector2((float)Math.Round(this.Center.X), (float)Math.Round(this.Center.Y));
			if (aim == Aims.AttackNow)
				Image.FrameCurrent = new Vector2(0, Image.FrameCurrent.Y - 1);
			Image.FrameCurrent = new Vector2(0, Image.FrameCurrent.Y + 2);
			if (!World.DeadUnits.ContainsKey(Image))
				World.DeadUnits.Add(Image, Position);
			base.Dead();
			target = null;
			bTarget = null;
			lU.Remove(this);
		}

		/// <summary>
		/// Визуализация юнита
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
			if (v != Vector2.Zero)
				Image.DrawAnimation(time, spriteBatch);
			else if (aim == Aims.AttackNow)
				Image.DrawAnimation(time, 4);
			else
				Image.Draw(spriteBatch);
			if (rect.Vis) {
				spriteBatch.Draw(hpBorder, rect.Border, new Rectangle(0, 0, rect.Border.Width, rect.Border.Height), rect.BorderColor);
				spriteBatch.Draw(hpTexture, rect.Rect, new Rectangle(0, 0, rect.Width, rect.Height), rect.Color);
			}
			this.CheckFlag(scene);
		}

		/// <summary>
		/// Юнит наносит урон Target или BTarget
		/// </summary>
		public virtual void Attack() {    //TODO моды агрессии
			int dmg = 0;
			if (target != null) {      //target
				if (TargetCheck()) {
					dmg = (target.armor >= attack) ? 1 : attack - target.armor;
					if (dmg < 1) dmg = 1;
					if (attackT == World.ATTACK_TIMER) {
						target.HP -= dmg;
						attackT = 0;
					} else
						attackT++;
				} else {
					if (Aim == Aims.AttackNow)
						Aim = Aims.Attack;
					P = new Vector2(target.Center.X, target.Center.Y);
					targetPos = P;
				}
			} else if (bTarget != null) {     //bTarget
				dmg = (bTarget.Armor >= attack) ? 1 : attack - bTarget.Armor;
				if (dmg < 1) dmg = 1;
				if (attackT == World.ATTACK_TIMER) {
					bTarget.HP -= dmg;
					attackT = 0;
				} else
					attackT++;
			}
		}

		/// <summary>
		/// Проверяет территорию вокруг себя в радиусе AgrR
		/// </summary>
		public abstract void CheckArea();

		/// <summary>
		/// Перемещает юнита на вектор Velocity
		/// </summary>
		public virtual void Move(Scene scene) {			
			MoveOn(v);
		}

		/// <summary>
		/// Перемещает юнит в направлении следующей точки очереди QP на вектор Velocity
		/// </summary>
		public virtual void MoveToNextPoint(Scene scene) {
			CheckMoving();

			length = sqrt(sqr(currentP.X - Center.X) + sqr(currentP.Y - Center.Y));
			if (length != 0) {
				float X = ((currentP.X - Center.X) * Speed) / length;
				float Y = ((currentP.Y - Center.Y) * Speed) / length;
				Velocity = new Vector2(X, Y);
			} else
				Velocity = Vector2.Zero;

			bool posChanged = v != Vector2.Zero;
			Move(scene);
			if (target != null && !TargetCheck()) {
				Aim = Aims.Attack;
				P = new Vector2(target.Center.X, target.Center.Y);
				targetPos = P;
			}
			if (posChanged)
				CheckGridChange();
			if (scene.Contains(this))
				SetDirection();
		}

		/// <summary>
		/// Вычисляет направление юнита
		/// </summary>
		public virtual void SetDirection() {
			if (currentP.X == Center.X && currentP.Y == Center.Y)
				return;
			float l = sqrt(sqr(currentP.X - Center.X) + sqr(currentP.Y - Center.Y));
			float sin = -(currentP.Y - Center.Y) / l;
			float cos = (currentP.X - Center.X) / l;
			Quaters quat = CalculateQuater(sin, cos);
			switch (quat) {
				case Quaters.First:
					Direction = (float)Math.Asin(sin);
					break;
				case Quaters.Second:
					Direction = (float)Math.Acos(cos);
					break;
				case Quaters.Third:
					Direction = -(float)Math.Acos(cos);
					break;
				case Quaters.Fourth:
					Direction = (float)Math.Asin(sin);
					break;
			}
		}

		/// <summary>
		/// Останавливает юнита
		/// </summary>
		public virtual void Stop() {
			Image.FrameCurrent = new Vector2(0, Image.FrameCurrent.Y);
			Velocity = Vector2.Zero;
			//P = Center;
		}

		/// <summary>
		/// Проверяет установку флага и, если нужно, устанавливает
		/// </summary>
		public void CheckFlag(Scene scene) {
			if (IsFocused && Velocity != Vector2.Zero && aim == Aims.MoveToPoint) {
				flag.Position = new Vector2(p.X - 5 - scene.Rect.X, p.Y - 23 - scene.Rect.Y);
				flag.Visible = true;
			} else if (IsFocused)
				flag.Visible = false;
		}

		/// <summary>
		/// Сверяет положение target.Center и targetPos.
		/// Возвращает false, если target == null или target.Center != targetPos
		/// </summary>
		protected bool TargetCheck() {
			if (target != null)
				if (targetPos == target.Center)
					return true;
			return false;
		}

		/// <summary>
		/// Внутренняя прорисовка
		/// </summary>
		protected override void InternalDraw(Scene scene) {
			base.InternalDraw(scene);
			rect.MoveTo((int)(drawPosition.X + (Width - rect.InitWidth) / 2), (int)(drawPosition.Y + Height + 1 + rect.yOffset));
		}

		protected abstract void CheckGridChange();


		protected const float pi = (float)Math.PI;
		protected virtual void ChangeImageOfDirection(float direction) {
            if (direction >= 0 && direction < pi / 8 || direction >= -pi / 8 && direction < 0)
                Image.FrameCurrent = new Vector2(Image.FrameCurrent.X, 0);
            else if (direction >= pi / 8 && direction < 3 * pi / 8)
                Image.FrameCurrent = new Vector2(Image.FrameCurrent.X, 21);
            else if (direction >= 3 * pi / 8 && direction < 5 * pi / 8)
                Image.FrameCurrent = new Vector2(Image.FrameCurrent.X, 18);
            else if (direction >= 5 * pi / 8 && direction < 7 * pi / 8)
                Image.FrameCurrent = new Vector2(Image.FrameCurrent.X, 15);
            else if (direction >= 7 * pi / 8 && direction < pi || direction >= -pi && direction < -7 * pi / 8)
                Image.FrameCurrent = new Vector2(Image.FrameCurrent.X, 12);
            else if (direction >= -7 * pi / 8 && direction < -5 * pi / 8)
                Image.FrameCurrent = new Vector2(Image.FrameCurrent.X, 9);
            else if (direction >= -5 * pi / 8 && direction < -3 * pi / 8)
                Image.FrameCurrent = new Vector2(Image.FrameCurrent.X, 6);
            else if (direction >= -3 * pi / 8 && direction < -pi / 8)
                Image.FrameCurrent = new Vector2(Image.FrameCurrent.X, 3);
			Image.Animation = true;
		}

		private void CheckMoving() {
			if (!World.AGrid.GetCell(currentP.X,currentP.Y).IsPassable) {
				P = World.AGrid.GetCell(currentP.X, currentP.Y).Center;
			}
			if ((abs(currentP.X - Center.X) <= abs(Velocity.X) ||
				 abs(currentP.Y - Center.Y) <= abs(Velocity.Y))){
				if (currentP == p) {
					if (Aim == Aims.FindResorse && this is IWorker)
						(this as IWorker).FindResource();
					if (Aim == Aims.MoveToPoint) {
						Stop();
						Center = new Vector2(round(currentP.X), round(currentP.Y));
					}
					Aim = Aims.Stand;
				}
				if (qp.Count != 0)
					currentP = qp.Dequeue();
			} else if (!World.AGrid.GetCell(Center.X + Velocity.X, Center.Y + Velocity.Y).IsPassable) {
				Cell nextCell = World.AGrid.GetCell(Center.X + Velocity.X, Center.Y + Velocity.Y);
				isBypass = true;
				bool unitInBlock = false;
				qp = ModifiedAStar.SearchPath(this, nextCell, out unitInBlock).ToQueue<Vector2>(); //TODO юнита "замыкает в препятствии, что приводит к жестким лагам
				if (unitInBlock) {
					Vector2 xy = ModifiedAStar.GetCellWithMinH(World.AGrid.GetCell(round(Left), round(Top))).Center;
					MoveTo(xy.X, xy.Y);
					Stop();
					Aim = Aims.Stand;
					P = Center;				
				}
				qp.Enqueue(p);
				currentP = qp.Dequeue();
			}
			if (isBypass) {
				isBypass = false;
				Cell currentCell = World.AGrid.GetCell(Center.X, Center.Y);
				for (int i = 0; i < currentCell.Neighbors.Count; i++) {
					if (!currentCell.Neighbors[i].IsPassable) {
						isBypass = true;
						break;
					}
				}
				if (!isBypass) {
					qp.Clear();
					qp.Enqueue(p);
					currentP = qp.Dequeue();
				}
			}
		}

		private void OnAimChanged(AimEventArgs e) {
			EventHandler<AimEventArgs> aimChanged = AimChanged;
			if (aimChanged != null)
				aimChanged(this, e);
		}

		private void TUnit_Focused(object sender, FocusedEventArgs e) {
			if (e.Focused) {
				rect.BorderColor = Color.White;
				ToFront();
			} else {
				rect.BorderColor = Color.Transparent;
				flag.Visible = false;
			}
		}
		private void TUnit_Death(object sender, EventArgs e) {
			if (IsFocused)
				flag.Visible = false;
		}

		private void target_Death(object sender, EventArgs e) {
			if (target == sender) {
				target = null;
				Stop();
				Center = new Vector2((float)Math.Round(Center.X), (float)Math.Round(Center.Y));
				Aim = Aims.Stand;
			}
		}
		private void bTarget_Death(object sender, EventArgs e) {
			if (bTarget == sender) {
				bTarget = null;
				Stop();
				Center = new Vector2((float)Math.Round(Center.X), (float)Math.Round(Center.Y));
				Aim = Aims.Stand;
			}
		}

		private float sqr(float a) {
			return a * a;
		}
		private float sqrt(double a) {
			return (float)Math.Sqrt(a);
		}
		private float abs(float a) {
			return Math.Abs(a);
		}
		private Quaters CalculateQuater(float sin, float cos) {
			if (sin >= 0 && cos > 0)
				return Quaters.First;
			if (sin > 0 && cos <= 0)
				return Quaters.Second;
			if (sin <= 0 && cos < 0)
				return Quaters.Third;
			if (sin < 0 && cos >= 0)
				return Quaters.Fourth;
			return Quaters.First;
		}
    }
}
