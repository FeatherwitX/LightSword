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
	/// Класс бараки
	/// </summary>
	public class TBaracks : TCreateBuilding, ICreate, IPoint
	{
		/// <summary>
		/// Создает бараки с указанными параметрами
		/// </summary>
		/// <param name="x">Расстояние по оси X</param>
		/// <param name="y">Расстояние по оси Y</param>
		/// <param name="hp">Здоровье бараков</param>
		/// <param name="armor">Броня бараков</param>
		/// <param name="side">Сторона, которую занимают бараки</param>
		/// <param name="image">Изображение бараков</param>
		public TBaracks(float x, float y, int hp, int armor, bool side, MultiSprite image)
		    : base(x, y, hp, armor, side, image) {
				progress += 100;
		}
		/// <summary>
		/// Создает новые бараки - копию бараков g
		/// </summary>
		public TBaracks(TBaracks g)
		    : base(g) {
		}

		/// <summary>
		/// Возвращает копию бараков
		/// </summary>
		public override object Clone() {
			return new TBaracks(this);
		}

		/// <summary>
		/// Добавляет объект unitName в конец очереди создания, если
		/// в ней есть место, unitName == UnitNames.Swordman и хватает ресурсов
		/// </summary>
		public override void ToQueue(UnitNames unitName) {
			if (unitName == UnitNames.Swordman && q.Count < World.MAX_QUEUE_SIZE) {
				if (Side && World.CResource.Gold >= World.SWORDMAN_COST) {
					q.Enqueue(unitName);
					World.CResource.Gold -= World.SWORDMAN_COST;
				}
				if (!Side && World.EResource.Gold >= World.WORKER_COST) {
					q.Enqueue(unitName);
					World.EResource.Gold -= World.SWORDMAN_COST;
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
					if (name != UnitNames.Swordman)
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
						new TCSwordman(round(Center.X) - 36, round(Center.Y + Height - 36), 150, 10, 2, Vector2.Zero,
                        MultiSprite.CreateSprite(World.Content,
						World.SpriteBatch,
						Fnames.Swordman,
						new Vector2(round(Center.X - 36), round(Center.Y + Height - 36)),
						new Vector2(72, 72),
						new Vector2(5, 24),
						World.FPS))
						: //TODO TEnemy
						new TCSwordman(round(Center.X - 36), round(Center.Y + Height - 36), 150, 10, 2, Vector2.Zero,
                        MultiSprite.CreateSprite(World.Content,
						World.SpriteBatch,
						Fnames.Swordman,
						new Vector2(round(Center.X - 36), round(Center.Y + Height - 36)),
						new Vector2(72, 72),
						new Vector2(5, 24),
						World.FPS));
					if (p != g.Center) {
						g.P = new Vector2(p.X, p.Y);
						g.Aim = Aims.MoveToPoint;
					}
				} else
					Progress += progressIncrement;
			}
		}
	}
}
