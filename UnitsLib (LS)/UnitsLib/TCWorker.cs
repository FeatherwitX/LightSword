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

namespace UnitsLib
{
	/// <summary>
	/// Класс подконтрольный рабочий
	/// </summary>
	public class TCWorker : TCUnit, IBag, IWorker
	{
		int bag, cbag; //Max bag, current bag
		SourceTypes sourceType;
		ISource source;
		TCityCenter cityCenter;
		int findR = World.FIND_RADIUS;

		/// <summary>
		/// Текущий размер сумки рабочего
		/// </summary>
		public int Bag {
			get { return cbag; }
			set {
				if (value < 0)
					value = 0;
				if (value > bag)
					value = bag;
				cbag = value;
			}
		}
		/// <summary>
		/// Максимальный размер сумки рабочего (только для чтения)
		/// </summary>
		public int MaxBag {
			get { return bag; }
			set {
				if (value < 0)
					value = 0;
				if (value > World.MAX_BAG)
					value = World.MAX_BAG;
				bag = value;
			}
		}
		/// <summary>
		/// Тип ресурсов в сумке рабочего
		/// </summary>
		public SourceTypes SourceType { get { return sourceType; } }
		/// <summary>
		/// Городской центр, к которому привязан рабочий
		/// </summary>
		public TCityCenter CityCenter {
			get { return cityCenter; }
			set {
				if (value != null && value.Side) {
					if (cityCenter != null)
						cityCenter.Death -= cityCenter_Death;
					cityCenter = value;
					if (cityCenter != null)
						cityCenter.Death += new EventHandler(cityCenter_Death);
				} else
					cityCenter = null;
			}
		}
		/// <summary>
		/// Источник, который добывает рабочий
		/// </summary>
		public ISource Source {
			get { return source; }
			set {
				if (source != null)
					source.Death -= source_Death;
				source = value;
				if (source != null)
					source.Death += new EventHandler(source_Death);
				if (cbag == 0)
					sourceType = (source == null) ? SourceTypes.None : (source is TTree) ? SourceTypes.Wood : SourceTypes.Stone;
				if (cityCenter == null)
					FindCityCenter();
			}
		}

		/// <summary>
		/// Создает новый объект TCWorker с указанными параметрами
		/// </summary>
		/// <param name="x">Расстояние по оси X</param>
		/// <param name="y">Расстояние по оси Y</param>
		/// <param name="hp">Здоровье рабочего</param>
		/// <param name="attack">Атака рабочего</param>
		/// <param name="armor">Броня рабочего</param>
		/// <param name="maxBag">Максимальный размер сумки рабочего</param>
		/// <param name="v">Вектор скорости рабочего</param>
		/// <param name="image">Изображение рабочего</param>
		public TCWorker(float x, float y, int hp, int attack, int armor, int maxBag, Vector2 v, MultiSprite image)
			: base(x, y, hp, attack, armor, v, image) {
			MaxBag = maxBag;
			cbag = 0;
			sourceType = SourceTypes.None;
			this.AimChanged +=new EventHandler<AimEventArgs>(TCWorker_AimChanged);
            rect = new hpRect(new Rectangle((int)Center.X - 15, (int)(Top + Height - 5), 30, 5));
			uname = UnitNames.Worker;
            ComparePriority = 6;
		}

		/// <summary>
		/// Создает новый объект TCWorker - копию g
		/// </summary>
		public TCWorker(TCWorker g)
			: base(g) {
			bag = g.bag;
			cbag = g.cbag;
			sourceType = g.sourceType;
			this.AimChanged += new EventHandler<AimEventArgs>(TCWorker_AimChanged);
            rect = new hpRect(new Rectangle((int)Center.X - 15, (int)(Top + Height - 5), 30, 5));
			uname = UnitNames.Worker;
            ComparePriority = 6;
		}

		/// <summary>
		/// Удаляет рабочего
		/// </summary>
		public override void Dispose() {
			base.Dispose();
			cityCenter = null;
			source = null;
		}

		/// <summary>
		/// Возвращает копию рабочего
		/// </summary>
		public override object Clone() {
			return new TCWorker(this);
		}

		/// <summary>
		/// Рабочий загружает свою сумку ресурсом из источника
		/// </summary>
		public void Download() {
			if (attackT == World.PRODUCE_TIMER) {
				if (cbag < bag) {
					if (source != null && sourceType == source.SourceType) {
						Bag++;
						source.Supply--;
					} else if (source == null) {
						Aim = Aims.FindResorse;
					} else if (sourceType != source.SourceType) {
						if (cityCenter != null) {
							P = new Vector2(cityCenter.Center.X, cityCenter.Center.Y);
							Aim = Aims.Upload;
						} else {
							source = null;
							Aim = Aims.Stand;
						}
					}
				} else if (cityCenter != null) {
					P = new Vector2(cityCenter.Center.X, cityCenter.Center.Y);
					Aim = Aims.Upload;
				} else {
					source = null;
					Aim = Aims.Stand;
				}
				attackT = 0;
			} else
				attackT++;
		}

		/// <summary>
		/// Рабочий выгружает ресурс из своей сумки в городской центр
		/// </summary>
		public void Upload() {
			switch (sourceType) {
				case SourceTypes.None:
					break;
				case SourceTypes.Wood:
					World.CResource.Wood += Bag;
					break;
				case SourceTypes.Stone:
					World.CResource.Stone += Bag;
					break;
			}
			Bag = 0;
			Stop();		
			Aim = Aims.Stand;
		}

		/// <summary>
		/// Если источник жив - рабочий отправляется добывать из него ресурсы
		/// </summary>
		public override void CheckArea() { 
			if (source != null) {
				sourceType = source.SourceType;
				Aim = Aims.Download;
				P = new Vector2(source.Center.X, source.Center.Y);
			}
		}

		/// <summary>
		/// Рабочий ищет близжайший источник в определенном радиусе, который содержит ресурс
		/// SourceType рабочего
		/// </summary>
		public void FindResource() { 
			ISource s = null;
			switch (sourceType) {
				case SourceTypes.None:
					break;
				case SourceTypes.Wood:
					s = World.FindClosestTTree(this, findR);
					break;
				case SourceTypes.Stone:
					s = World.FindClosestTStone(this, findR);
					break;
			}
			if (s != null) {
				Aim = Aims.Download;
				Source = s;
				P = s.Center;
				Target = null;
				BTarget = null;
			} else {
				Source = null;
				Aim = Aims.Stand;
				Stop();
				Center = P;
			}
		}

		private void FindCityCenter() {
			if (source != null && cityCenter == null) {
				for (int i = World.TBuildings.Count - 1; i >= 0; i--) {
					if (World.TBuildings[i] is TCityCenter && World.TBuildings[i].Side) {
						CityCenter = World.TBuildings[i] as TCityCenter;
						return;
					}
				}
			} 
		}

		private void cityCenter_Death(object sender, EventArgs e) {
			if (cityCenter == sender) {
				cityCenter = null;
			}
		}

		private void source_Death(object sender, EventArgs e) {
			if (source == sender) {
				source = null;
				Aim = Aims.FindResorse;
			}
		}

		private void TCWorker_AimChanged(object sender, AimEventArgs e) {
			if (e.Aim == Aims.MoveToPoint || e.Aim == Aims.Attack) {
				if (source != null)
					source.Death -= source_Death;
				Source = null;
			}
		}
	}
}
