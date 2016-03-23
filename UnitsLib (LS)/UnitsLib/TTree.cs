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
	public class TTree : TObject, ISource
	{
		int supply, csupply; //max supply, current supply
		SourceTypes sourceType;

		internal static List<TTree> lT = World.TTrees;

		public int Supply {
			get { return csupply; }
			set {
				if (value < 0) value = 0;
				if (value > supply) value = supply;
				csupply = value;
				if (csupply < supply) {
					if (csupply == 0)
						Dead();
					else
						Image.FrameCurrent = new Vector2(1, 0);
				}
			}
		}
		public int MaxSupply { get { return supply; } }
		public SourceTypes SourceType { get { return sourceType; } }

		public TTree(float x, float y, int supply, MultiSprite image)
			: base(x, y, image) {
			if (supply <= 0) supply = 1;
			if (supply > 150) supply = 150;
			this.supply = supply;
			csupply = this.supply;
			sourceType = SourceTypes.Wood;

			lT.Add(this);
		}
		public TTree(TTree g)
			: base(g) {
			supply = g.supply;
			csupply = g.csupply;
			sourceType = SourceTypes.Wood;

			lT.Add(this);
		}

		public override void Dispose() {
			base.Dispose();
			lT.Remove(this);
		}

		public override object Clone() {
			return new TTree(this);
		}

		protected override void Dead() {
			Image.FrameCurrent = new Vector2(2, 0);
			if (!World.DeadTrees.ContainsKey(Image))
				World.DeadTrees.Add(Image, Position);
			base.Dead();
			lT.Remove(this);
		}
	}
}
