using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitsLib.Surround
{
	/// <summary>
	/// Представляет ресурсы игрока
	/// </summary>
	public struct Resource
	{
		int gold, wood, stone;

		/// <summary>
		/// Золото игрока
		/// </summary>
		public int Gold {
			get { return gold; }
			set {
				if (value < 0) value = gold;
				if (value > 9999) value = 9999;
				gold = value;
			}
		}
		/// <summary>
		/// Дерево игрока
		/// </summary>
		public int Wood {
			get { return wood; }
			set {
				if (value < 0) value = wood;
				if (value > 9999) value = 9999;
				wood = value;
			}
		}
		/// <summary>
		/// Камень игрока
		/// </summary>
		public int Stone {
			get { return stone; }
			set {
				if (value < 0) value = stone;
				if (value > 9999) value = 9999;
				stone = value;
			}
		}

		/// <summary>
		/// Создает новый ресурс игрока с указанными параметры
		/// </summary>
		public Resource(int gold, int wood, int stone) {
			this.gold = gold;
			this.wood = wood;
			this.stone = stone;
		}
	}
}
