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
	/// Класс фермер
	/// </summary>
	public class TCPeasant : TCUnit
	{
		public TCPeasant(float x, float y, int hp, int attack, int armor, Vector2 v, MultiSprite image)
			: base(x, y, hp, attack, armor, v, image) {
            rect = new hpRect(new Rectangle((int)Center.X - 15, (int)(Top + Height + 1), 30, 5));
			uname = UnitNames.Peasant;
            ComparePriority = 7;
		}

		public TCPeasant(TCPeasant g)
			: base(g) {
			uname = UnitNames.Peasant;
            ComparePriority = 7;
		}

		public override object Clone() {
			return new TCPeasant(this);
		}
	}
}
