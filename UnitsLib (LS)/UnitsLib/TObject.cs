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
using UnitsLib.Surround;
using UnitsLib.Interfaces;
using UnitsLib.Events;

namespace UnitsLib
{
	/// <summary>
	/// Базовый класс всех игровых объектов в игре Light Sword
	/// </summary>
	public abstract class TObject : IDisposable, ICloneable, IComparable<TObject> 
	{
		public static readonly TObjectComboComparer Comparer = new TObjectComboComparer();

		float fx, fy;
		float fw, fh;
		bool vis;
		MultiSprite image;
		bool focused;
		protected BoundingBox box;
		internal Rectangle bounds;
        int comparePriority = 0;

		protected Vector2 drawPosition;

		internal static List<TObject> l = World.TObjects;

		/// <summary>
		/// Координата X левой границы объекта
		/// </summary>
		public float Left {
			get { return fx; }
			set { MoveTo(value, fy); }
		}
		/// <summary>
		/// Координата Y верхней границы объекта
		/// </summary>
		public float Top {
			get { return fy; }
			set { MoveTo(fx, value); }
		}
		/// <summary>
		/// Ширина объекта (только для чтения)
		/// </summary>
		public float Width {
			get { return fw; }
		}
		/// <summary>
		/// Высота объекта (только для чтения)
		/// </summary>
		public float Height {
			get { return fh; }
		}
		/// <summary>
		/// Координата X правой границы объекта (только для чтения)
		/// </summary>
		public float Right {
			get { return fx + fw; }
		}
		/// <summary>
		/// Координата Y нижней границы объекта (только для чтения)
		/// </summary>
		public float Bottom {
			get { return fy + fh; }
		}
		/// <summary>
		/// Центр объекта
		/// </summary>
		public Vector2 Center {
			get { return new Vector2(fx + fw / 2, fy + fh / 2); }
			set { MoveTo(value.X - fw / 2, value.Y - fh / 2); }
		}
		/// <summary>
		/// Позиция верхнего левого угла объекта, относительно левого верхнего угла
		/// игрового логического поля
		/// </summary>
		public Vector2 Position {
			get { return new Vector2(fx, fy); }
			set { MoveTo(value.X, value.Y); }
		}
		/// <summary>
		/// Виден ли объект
		/// </summary>
		public bool Visible {
			get { return vis; }
			set {
				vis = value;
				Image.Visible = vis;
			}
		}
		/// <summary>
		/// Изображение объекта (только для чтения)
		/// </summary>
		public MultiSprite Image {
			get { return image; }
			protected set { image = value; }
		}
		/// <summary>
		/// Ограничивающая структура объекта (только для чтения)
		/// </summary>
		public BoundingBox Box {
			get { return box; }
		}
		/// <summary>
		/// Находится ли объект в фокусе?
		/// </summary>
		public virtual bool IsFocused {
			get { return focused; }
			set {
				focused = value;
				OnFocused(new FocusedEventArgs(focused));
			}
		}
        /// <summary>
        /// Приоритет сравнения, определяемый в классах наследниках
        /// </summary>
        public int ComparePriority {
            get { return comparePriority; }
            protected set {
                comparePriority = value;
            }
        }

		/// <summary>
		/// Объект умер
		/// </summary>
		public event EventHandler Death;
		/// <summary>
		/// Объект попал в фокус
		/// </summary>
		internal event EventHandler<FocusedEventArgs> Focused;

		/// <summary>
		/// Создает новый объект TObject с указанными параметрами
		/// </summary>
		/// <param name="x">Позиция по оси X</param>
		/// <param name="y">Позиция по оси Y</param>
		/// <param name="image">Изображение</param>
		public TObject(float x, float y, MultiSprite image) {
			fx = x; fy = y;
			this.image = image;
			fw = image.Size.X;
			fh = image.Size.Y;
			box = new BoundingBox(new Vector3(fx, fy, 0), new Vector3(fx + fw, fy + fh, 0));
			vis = true;
			bounds = new Rectangle((int)fx, (int)fy, (int)fw, (int)fh);
			this.Death += new EventHandler(TObject_Death);
			this.Focused += new EventHandler<FocusedEventArgs>(TObject_Focused);

			World.Grid.AddObject(this);

			if (!(this is TUnit))
				SetAGridCellsPassability(false);

			l.Add(this);
		}
		/// <summary>
		/// Создает новый объект TObject - точную копию g
		/// </summary>
		public TObject(TObject g) {
			fx = g.fx;
			fy = g.fy;
			fw = g.fw;
			fh = g.fh;
			image = (MultiSprite)g.image.Clone();
			box = g.box;
			vis = g.vis;
			bounds = g.bounds;
			this.Death += new EventHandler(TObject_Death);

			World.Grid.AddObject(this);

			if (!(this is TUnit))
				SetAGridCellsPassability(false);

			l.Add(this);
		}

		/// <summary>
		/// Освобождает все внутренние ресурсы объекта и удаляет его
		/// </summary>
		public virtual void Dispose() {
			Visible = false;
			if (!(this is TUnit))
				SetAGridCellsPassability(true);
			l.Remove(this);
			World.Grid.RemoveObject(this);
		}

		/// <summary>
		/// Перемещает объект в точку (x,y)
		/// </summary>
		/// <param name="x">Координата по оси X (мировая)</param>
		/// <param name="y">Координата по оси Y (мировая)</param>
		public virtual void MoveTo(float x, float y) {
			SetCoords(x, y);
			box.Max = new Vector3(Position + new Vector2(Width, Height), 0);
			box.Min = new Vector3(Position, 0);
			bounds = new Rectangle((int)x, (int)y, bounds.Width, bounds.Height);
		}

		/// <summary>
		/// Пеермещает объект на указанный вектор
		/// </summary>
		/// <param name="v">Вектор, на который перемещается объект</param>
		public void MoveOn(Vector2 v) {
			MoveTo(fx + v.X, fy + v.Y);
		}

		/// <summary>
		/// Возвращает True, если ограничивающая структура объекта пересекается или
		/// лежит в ограничивающей структуре указанного объекта
		/// </summary>
		/// <param name="g">Объект, пересечение с которым нужно проверить</param>
		public virtual bool Intersect(TObject g) {
			return box.Contains(g.box) != ContainmentType.Disjoint;
		}

		/// <summary>
		/// Возвращает True, если точка (x,y) находится в пределах ограничивающей структуры
		/// объекта
		/// </summary>
		/// <param name="x">Координата по оси X</param>
		/// <param name="y">Координата по оси Y</param>
		public virtual bool PtInside(float x, float y) {
			return box.Contains(new Vector3(x, y, 0)) != ContainmentType.Disjoint;
		}

		/// <summary>
		/// Переносит объект на передний план
		/// </summary>
		public void ToFront() {
			int ind = l.IndexOf(this);
			if (ind == l.Count - 1 || ind == -1)
				return;
			l.RemoveAt(ind);
			l.Add(this);
		}

		/// <summary>
		/// Переносит объект на задний план
		/// </summary>
		public void ToBack() {
			int ind = l.IndexOf(this);
			if (ind == 0 || ind == -1)
				return;
			l.RemoveAt(ind);
			l.Insert(0, this);
		}

        public int CompareTo(TObject obj) {//TODO установить приоритеты для всех классов
            if (this.comparePriority > obj.comparePriority)
                return 1;
            else if (this.comparePriority < obj.comparePriority)
                return -1;
            else
                return 0;
        }

		/// <summary>
		/// Возвращает точную копию объекта
		/// </summary>
		public abstract object Clone();

		/// <summary>
		/// Визуализация объекта
		/// </summary>
		public virtual void Draw(SpriteBatch spriteBatch, GameTime time, Scene scene) {
			if (scene.Contains(this)) {
				Visible = true;
				InternalDraw(scene);
				Image.Draw(spriteBatch);
			} else
				Visible = false;
		}

		/// <summary>
		/// Смерть объекта
		/// </summary>
		protected virtual void Dead() {
			OnDeath(new EventArgs());
			if (!(this is TUnit))
				SetAGridCellsPassability(true);
			l.Remove(this);
			World.Grid.RemoveObject(this);
		}

		/// <summary>
		/// Внутренняя прорисовка, реализующая перемещение и/или прорисовку некоторых 
		/// специальных деталей, а также приведение изображения из мировых координат к экранным
		/// </summary>
		protected virtual void InternalDraw(Scene scene) {
			drawPosition = new Vector2(Left - scene.Rect.X, Top - scene.Rect.Y);
			Image.Position = drawPosition;
		}

		private void SetCoords(float x, float y) {
			fx = x;
			fy = y;
		}

		private void SetAGridCellsPassability(bool pass) {
			int xMin = ((int)Left >= 0) ? (int)Left / World.AGrid.CellWidth : 0;
			int yMin = ((int)Top >= 0) ? (int)Top / World.AGrid.CellWidth : 0;
			int xMax = ((int)Right >= 0) ? (int)Right / World.AGrid.CellWidth : -1;
			int yMax = ((int)Bottom >= 0) ? (int)Bottom / World.AGrid.CellWidth : -1;
			if (xMax != -1 && yMax != -1) {
				for (int i = xMin; i <= xMax; i++) {
					for (int j = yMin; j <= yMax; j++) {
						World.AGrid.Cells[i, j].IsPassable = pass;
					}
				}
			}
		}
		private Point[] Cells() {
			Point[] result = new Point[2];
			int xMin = ((int)Left >= 0) ? (int)Left / World.AGrid.CellWidth : 0;
			int yMin = ((int)Top >= 0) ? (int)Top / World.AGrid.CellWidth : 0;
			int xMax = ((int)Right >= 0) ? (int)Right / World.AGrid.CellWidth : -1;
			int yMax = ((int)Bottom >= 0) ? (int)Bottom / World.AGrid.CellWidth : -1;
			result[0] = new Point(xMin, yMin);
			result[1] = new Point(xMax, yMax);
			return result;
		}

		protected void OnFocused(FocusedEventArgs e) {
			EventHandler<FocusedEventArgs> focused = Focused;
			if (focused != null)
				focused(this, e);
		}
		protected void OnDeath(EventArgs e) {
			EventHandler death = Death;
			if (death != null)
				death(this, e);
		}

		private void TObject_Death(object sender, EventArgs e) {
            if (World.Panel.Objects.Contains(this))
                World.Panel.Objects.Remove(this);
		}
		private void TObject_Focused(object sender, FocusedEventArgs e) {
            if (e.Focused)
                World.Panel.Objects.Add(this);
            else
                if (World.Panel.Objects.Contains(this))
                    World.Panel.Objects.Remove(this);
		}

		protected float round(float a) {
			return (float)Math.Round(a);
		}
	}

	/// <summary>
	/// Специальный компаратор для объектов TOBject
	/// </summary>
	public class TObjectComboComparer : IComparer<TObject>
	{
		/// <summary>
		/// Сначала сравнивает приоритет, если он равный и объекты реализуют IHP, то сравнивает кол-во текущего хп
		/// </summary>
		public int Compare(TObject x, TObject y) {
			int result = x.CompareTo(y);
			if (x is IHP && y is IHP) {
				if (result == 0 && (x as IHP).HP < (y as IHP).HP)
					result = -1;
			}
			return result;
		}
	}
}