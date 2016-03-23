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
	public class TFarm : TCreateBuilding, IQueue, ICreate, IPoint
	{
		int goldIncrement = World.START_GOLD_INCREMENT;
		int goldIncrementTimer = 0; //Increment timer (local)

		public TFarm(float x, float y, int hp, int armor, bool side, MultiSprite image)
			: base(x, y, hp, armor, side, image) {
		}
		public TFarm(TFarm g)
			: base(g) {
		}

		public override object Clone() {
			return new TFarm(this);
		}

		public override void ToQueue(UnitNames unitName) {
			if (unitName == UnitNames.Peasant && q.Count < World.MAX_QUEUE_SIZE) {
				if (Side && World.CResource.Gold >= World.PEASANT_COST) {
					q.Enqueue(unitName);
					World.CResource.Gold -= World.PEASANT_COST;
				}
				if (!Side && World.EResource.Gold >= World.WORKER_COST) {
					q.Enqueue(unitName);
					World.EResource.Gold -= World.PEASANT_COST;
				}
			}
		}

		public override UnitNames FromQueue() {
			return q.Dequeue();
		}

		protected override void SetQ(Queue<UnitNames> Q) {
			if (Q != null) {
				UnitNames[] qArr = Q.ToArray();
				foreach (var name in qArr) {
					if (name != UnitNames.Peasant)
						return;
				}
			}
			q = Q;
		}

		public override void Create() {
			if (q.Count > 0) {
				TCUnit g;
				if (cprogress == progress) {
					Progress = 0;
					FromQueue();
					g = (Side) ?
						new TCPeasant(round(Center.X - 15), round(Center.Y + Height - 15), 120, 4, 0, Vector2.Zero,
						MultiSprite.CreateSprite(World.Content,
						World.SpriteBatch,
						Fnames.Peasant,
						new Vector2(round(Center.X - 15), round(Center.Y + Height - 15)),
						new Vector2(30, 30),
						new Vector2(6, 2),
						World.FPS))
						: //TODO TEPeasant
						new TCPeasant(round(Center.X - 15), round(Center.Y + Height - 15), 120, 4, 0, Vector2.Zero,
                        MultiSprite.CreateSprite(World.Content,
						World.SpriteBatch,
						Fnames.Peasant,
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
			ProduceGold();
		}

		private void ProduceGold() {
			if (goldIncrementTimer == World.GOLD_INCREMENT_TIMER) {
				if (Side)
					World.CResource.Gold += goldIncrement;
				else
					World.EResource.Gold += goldIncrement;
				goldIncrementTimer = 0;
			} else
				goldIncrementTimer++;
		}
	}
}
