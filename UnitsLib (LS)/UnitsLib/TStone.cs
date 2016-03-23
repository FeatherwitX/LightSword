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
using UnitsLib.Enums;

namespace UnitsLib
{
	public class TStone : TObject, ISource
	{
		int supply, csupply; //max supply, current supply
		SourceTypes sourceType;

		internal static List<TStone> lS = World.TStones;

		public int Supply {
			get { return csupply; }
			set {
				if (value < 0) value = 0;
				if (value > supply) value = supply;
				csupply = value;
				if (csupply < supply) {
					if (csupply == 0)
						Dead();
					else if (csupply < 250)
						Image.FrameCurrent = new Vector2(0, 1);
				}
			}
		}
		public int MaxSupply { get { return supply; } }
		public SourceTypes SourceType { get { return sourceType; } }

		public TStone(float x, float y, int supply, MultiSprite image)
			: base(x, y, image) {
			if (supply <= 0) supply = 1;
			if (supply > 500) supply = 500;
			this.supply = supply;
			csupply = this.supply;
			if (csupply < 250) Image.FrameCurrent = new Vector2(0, 1);
			sourceType = SourceTypes.Stone;

			lS.Add(this);
		}
		public TStone(TStone g)
			: base(g) {
			supply = g.supply;
			csupply = g.csupply;
			Image.FrameCurrent = g.Image.FrameCurrent;
			sourceType = SourceTypes.Stone;

			lS.Add(this);
		}

		public override void Dispose() {
			base.Dispose();
			lS.Remove(this);
		}

		public override object Clone() {
			return new TStone(this);
		}

		protected override void Dead() {
			Visible = false;
			base.Dead();
			lS.Remove(this);
		}
	}
}
