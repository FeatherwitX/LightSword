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
using UnitsLib.Surround;

namespace UnitsLib
{
	/// <summary>
	/// Класс Городской центр
	/// </summary>
	public class TCityCenter : TCreateBuilding, IQueue, ICreate, IPoint
	{
		/// <summary>
		/// Создает объект TCityCenter с указанными параметрами
		/// </summary>
		/// <param name="x">Расстояние по оси X</param>
		/// <param name="y">Расстояние по оси Y</param>
		/// <param name="hp">Здоровье городского центра</param>
		/// <param name="armor">Броня городского центра</param>
		/// <param name="side">Сторона, которую занимает городской центр</param>
		/// <param name="image">Изображение городского центра</param>
		public TCityCenter(float x, float y, int hp, int armor, bool side, MultiSprite image)
			: base(x, y, hp, armor, side, image) {
		}
		/// <summary>
		/// Создает новый объект TCityCenter - копию g
		/// </summary>
		public TCityCenter(TCityCenter g)
			: base(g) {
		}

		/// <summary>
		/// Возвращает копию городского центра
		/// </summary>
		public override object Clone() {
			return new TCityCenter(this);
		}

		/// <summary>
		/// Добавляет объект unitName в конец очереди создания, если
		/// в ней есть место, unitName == UnitNames.Worker и хватает ресурсов
		/// </summary>
		public override void ToQueue(UnitNames unitName) {
			if (unitName == UnitNames.Worker && q.Count < World.MAX_QUEUE_SIZE) {
				if (Side && World.CResource.Gold >= World.WORKER_COST) {
					q.Enqueue(unitName);
					World.CResource.Gold -= World.WORKER_COST; 
				}
				if (!Side && World.EResource.Gold >= World.WORKER_COST) {
					q.Enqueue(unitName);
					World.EResource.Gold -= World.WORKER_COST;
				}
			}
		}

		/// <summary>
		/// Удаляет и возвращает объект, находящийся в начале очереди создания
		/// </summary>
		public override UnitNames FromQueue() {
			return q.Dequeue();
		}

		/// <summary>
		/// Устанавливает очередь создания
		/// </summary>
		protected override void SetQ(Queue<UnitNames> Q) {
			if (Q != null) {
				UnitNames[] qArr = Q.ToArray();
				foreach (var name in qArr) {
					if (name != UnitNames.Worker)
						return;
				}
			}
			q = Q;
		}

		/// <summary>
		/// Если прогресс создания полон - создает юнита из очереди создания, иначе
		/// увеличивает прогресс создания
		/// </summary>
		public override void Create() {
			if (q.Count > 0) {
			    TCUnit g;
				if (cprogress == progress) {
					Progress = 0;
					FromQueue();
					g = (Side) ?
						new TCWorker(round(Center.X - 15), round(Center.Y + Height - 15), 100, 4, 0, 10, Vector2.Zero,
                        MultiSprite.CreateSprite(World.Content,
						World.SpriteBatch,
						Fnames.Worker,
						new Vector2(round(Center.X - 15), round(Center.Y + Height - 15)),
						new Vector2(30, 30),
						new Vector2(6, 2),
						World.FPS))
						: //TODO TEnemy
						new TCWorker(round(Center.X - 15), round(Center.Y + Height - 15), 100, 4, 0, 10, Vector2.Zero,
                        MultiSprite.CreateSprite(World.Content,
						World.SpriteBatch,
						Fnames.Worker,
						new Vector2(round(Center.X - 15), round(Center.Y + Height - 15)),
						new Vector2(30, 30),
						new Vector2(6, 2),
						World.FPS));
					if (p != g.Center) {
						g.P = new Vector2(p.X, p.Y);
						g.Aim = Aims.MoveToPoint;
					}
				} else
					Progress += progressIncrement;
			}
		}

		public void CreateBuilding(BuildingNames bname, Vector2 position) {
			switch (bname) {
				case BuildingNames.Farm:
					if ((Side && World.CResource.Wood >= World.FARM_COST_WOOD || !Side && World.EResource.Wood >= World.FARM_COST_WOOD) &&
						(Side && World.CResource.Stone >= World.FARM_COST_STONE || !Side && World.EResource.Stone >= World.FARM_COST_STONE)) {
                            new TFarm(position.X, position.Y, 250, 1, Side, MultiSprite.CreateSprite(World.Content, World.SpriteBatch, Fnames.Farm,
							new Vector2(position.X, position.Y), new Vector2(120, 67), new Vector2(3, 1), World.FPS));
						if (Side) {
							World.CResource.Wood -= World.FARM_COST_WOOD;
							World.CResource.Stone -= World.FARM_COST_STONE;
						} else {
							World.EResource.Wood -= World.FARM_COST_WOOD;
							World.EResource.Stone -= World.FARM_COST_STONE;
						}
					}
					break;
				case BuildingNames.CityCenter:
					break;
				case BuildingNames.Baracks:
					if ((Side && World.CResource.Wood >= World.BARACKS_COST_WOOD || !Side && World.EResource.Wood >= World.BARACKS_COST_WOOD) &&
						(Side && World.CResource.Stone >= World.BARACKS_COST_STONE || !Side && World.EResource.Stone >= World.BARACKS_COST_STONE)) {
                            new TBaracks(position.X, position.Y, 400, 4, Side, MultiSprite.CreateSprite(World.Content, World.SpriteBatch, Fnames.Baracks,
					new Vector2(position.X, position.Y), new Vector2(97, 92), new Vector2(3, 1), World.FPS));
						if (Side) {
							World.CResource.Wood -= World.BARACKS_COST_WOOD;
							World.CResource.Stone -= World.BARACKS_COST_STONE;
						} else {
							World.EResource.Wood -= World.BARACKS_COST_WOOD;
							World.EResource.Stone -= World.BARACKS_COST_STONE;
						}
					}
					break;
				default:
					break;
			}
		}
	}
}
