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
	/// Класс подконтрольный мечник
	/// </summary>
	public class TCSwordman : TCUnit
	{
		public TCSwordman(float x, float y, int hp, int attack, int armor, Vector2 v, MultiSprite image)
			: base(x, y, hp, attack, armor, v, image) {
            rect = new hpRect(new Rectangle((int)Center.X - 15, (int)(Top + Height - 5), 30, 5), World.TCSwordmanHPYOffset);
			uname = UnitNames.Swordman;
            ComparePriority = 8;
		}
		public TCSwordman(TCSwordman g)
			: base(g) {
            rect.Location = new Point((int)Center.X - 15, (int)(Top + Height - 5));
			uname = UnitNames.Swordman;
            ComparePriority = 8;
		}

		public override object Clone() {
			return new TCSwordman(this);
		}
	}
}
