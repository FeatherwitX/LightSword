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

namespace UnitsLib.Surround
{
	/// <summary>
	/// Статичный класс, содержащий глобальные (мировые) переменные и константы
	/// </summary>
	public static class World
    {

        #region Списки
        /// <summary>
		/// Список объектов TObject
		/// </summary>
		public static List<TObject> TObjects = new List<TObject>();
		/// <summary>
		/// Список объектов TUnit
		/// </summary>
		public static List<TUnit> TUnits = new List<TUnit>();
		/// <summary>
		/// Список объектов TCUnit
		/// </summary>
		public static List<TCUnit> TCUnits = new List<TCUnit>();
		/// <summary>
		/// Список объектов TEnemy
		/// </summary>
		public static List<TEnemy> TEnemys = new List<TEnemy>();
		/// <summary>
		/// Список объектов TBuilding
		/// </summary>
		public static List<TBuilding> TBuildings = new List<TBuilding>();
		/// <summary>
		/// Список объектов TTree
		/// </summary>
		public static List<TTree> TTrees = new List<TTree>();
		/// <summary>
		/// Список объектов TStone
		/// </summary>
		public static List<TStone> TStones = new List<TStone>();
        #endregion

        #region Словари
        /// <summary>
		/// Словарь изображений мертвых юнитов
		/// </summary>
		public static Dictionary<MultiSprite, Vector2> DeadUnits = new Dictionary<MultiSprite, Vector2>();
		/// <summary>
		/// Словарь изображений мертвых деревьев
		/// </summary>
		public static Dictionary<MultiSprite, Vector2> DeadTrees = new Dictionary<MultiSprite, Vector2>();
		/// <summary>
		/// Словарь изображений мертвых зданий
		/// </summary>
		public static Dictionary<MultiSprite, Vector2> DeadBuildings = new Dictionary<MultiSprite, Vector2>();
        #endregion

        #region Поля
        /// <summary>
		/// Глобальный Контент менеджер
		/// </summary>
		public static ContentManager Content;
		/// <summary>
		/// Глобальный спрайт батч
		/// </summary>
		public static SpriteBatch SpriteBatch;
		/// <summary>
		/// Глобальная сцена
		/// </summary>
		public static Scene Scene;
		/// <summary>
		/// Глобальная скорость прокрутки
		/// </summary>
		public static int SceneMoveSpeed = 15;
		/// <summary>
		/// Флаг, который ставится в точку, куда идет юнит в фокусе
		/// </summary>
		public static Flag Flag = new Flag();
		/// <summary>
		/// UI
		/// </summary>
		public static Panel Panel;

		/// <summary>
		/// Глобальное поле
		/// </summary>
        public static readonly Rectangle Field = new Rectangle(0, 0, 7000, 5000);

        /// <summary>
        /// Смещение прямоугольника хп по оси y для TCSwordman
        /// </summary>
        public static int TCSwordmanHPYOffset = -15;

        /// <summary>
        /// Цвет красного строительного прямоугольника
        /// </summary>
        public static Color RedBuildingRectangle = new Color(125, 0, 0, 150);
        /// <summary>
        /// Цвет зеленого строительного прямоугольника
        /// </summary>
        public static Color GreenBuildingRectangle = new Color(0, 125, 0, 150);
        #endregion

        #region Сетки
        /// <summary>
		/// Регулярная сеть объектов
		/// </summary>
		internal static Grid<TObject> Grid;
		/// <summary>
		/// Регулярная сеть Controlled
		/// </summary>
		internal static Grid<TCUnit> CUGrid;
		/// <summary>
		/// Регулярная сеть Enemy
		/// </summary>
		internal static Grid<TEnemy> EGrid;
		/// <summary>
		/// Сетка A* граф
		/// </summary>
		internal static Grid AGrid;
        #endregion

        #region Ресурсы
        /// <summary>
		/// Подконтрольный ресурс
		/// </summary>
		internal static Resource CResource;
		/// <summary>
		/// Ресурс врага
		/// </summary>
		internal static Resource EResource;
        #endregion

        #region Константы
        /// <summary>
		/// Таймер атаки (глобальный)
		/// </summary>
		public const int ATTACK_TIMER = 40;
		/// <summary>
		/// Таймер добычи ресурса (глобальный)
		/// </summary>
		public const int PRODUCE_TIMER = 70;
		/// <summary>
		/// Таймер проверки территории (глобальный)
		/// </summary>
		public const int CHECK_AREA_TIMER = 15;
		/// <summary>
		/// Стартовый прогресс
		/// </summary>
		public const int INIT_PROGRESS = 300;
		/// <summary>
		/// Таймер добычи золота (глобальный)
		/// </summary>
		public const int GOLD_INCREMENT_TIMER = 1000;
		/// <summary>
		/// Начальный уровень добычи золота фермами
		/// </summary>
		public const int START_GOLD_INCREMENT = 25;
		/// <summary>
		/// Максимально возможный размер сумки рабочего
		/// </summary>
		public const int MAX_BAG = 45;
		/// <summary>
		/// Глобальное FPS для создания анимированных спрайтов
		/// </summary>
		public const int FPS = 10;
		/// <summary>
		/// Максимально возможный размер очереди создания
		/// </summary>
		internal const int MAX_QUEUE_SIZE = 10;
        /// <summary>
        /// Максимальное кол-во юнитов в фокусе
        /// </summary>
        public const int MAX_FOCUSED_OBJECTS = 40;
		/// <summary>
		/// Радиус поиска ресурсов
		/// </summary>
		internal const int FIND_RADIUS = 500;

		/// <summary>
		/// Цена рабочего
		/// </summary>
		public const int WORKER_COST = 100;
		/// <summary>
		/// Цена крестьянина
		/// </summary>
		public const int PEASANT_COST = 60;
		/// <summary>
		/// Цена мечника
		/// </summary>
		public const int SWORDMAN_COST = 150;

		/// <summary>
		/// Цена фермы (дерево)
		/// </summary>
		public const int FARM_COST_WOOD = 250;
		/// <summary>
		/// Цена фермы (камень)
		/// </summary>
		public const int FARM_COST_STONE = 200;
		/// <summary>
		/// Цена бараков (дерево)
		/// </summary>
		public const int BARACKS_COST_WOOD = 400;
		/// <summary>
		/// Цена бараков (камень)
		/// </summary>
		public const int BARACKS_COST_STONE = 500;

        #region Размеры клетки
        /// <summary>
        /// Ширина клетки
        /// </summary>
        internal const int CELL_W = 200;
        /// <summary>
        /// Высота клетки
        /// </summary>
        internal const int CELL_H = CELL_W;
        #endregion

        #endregion

        #region Методы
        /// <summary>
		/// Создает ресурс для выбранной стороны
		/// </summary>
		public static void SetResource(int gold, int wood, int stone, bool side) {
			if (side)
				CResource = new Resource(gold, wood, stone);
			else
				EResource = new Resource(gold, wood, stone);
		}

		/// <summary>
		/// Инициализирует сети
		/// </summary>
		public static void InitCells() {
			Grid = new Grid<TObject>(World.Field.Width / World.CELL_W, World.Field.Height / World.CELL_H, World.CELL_W, World.CELL_H);
			CUGrid = new Grid<TCUnit>(World.Field.Width / World.CELL_W, World.Field.Height / World.CELL_H, World.CELL_W, World.CELL_H);
			EGrid = new Grid<TEnemy>(World.Field.Width / World.CELL_W, World.Field.Height / World.CELL_H, World.CELL_W, World.CELL_H);
			AGrid = new Grid(World.Field.Width / Cell.CellWidth + 50, World.Field.Height / Cell.CellWidth + 50);
		}

		/// <summary>
		/// Возведение в квадрат
		/// </summary>
        internal static double sqr(double a) {
            return a * a;
        }

		#region костыли
		internal static TObject FindClosestTObject(TObject obj, int radius) {
			TObject tO = null;
			int minR = radius + 1;
			int r = 0;
			for (int i = 0; i <= TObjects.Count - 1; i++) {
				r = (int)Math.Round(Math.Sqrt(sqr(obj.Center.X - TObjects[i].Center.X) + sqr(obj.Center.Y - TObjects[i].Center.Y)));
				if (r <= radius && r < minR) {
					tO = TObjects[i];
					minR = r;
				}
			}
			return tO;
		}

		internal static TUnit FindClosestTUnit(TObject obj, int radius) {
			TUnit tO = null;
			int minR = radius + 1;
			int r = 0;
			for (int i = 0; i <= TUnits.Count - 1; i++) {
				r = (int)Math.Round(Math.Sqrt(sqr(obj.Center.X - TUnits[i].Center.X) + sqr(obj.Center.Y - TUnits[i].Center.Y)));
				if (r <= radius && r < minR) {
					tO = TUnits[i];
					minR = r;
				}
			}
			return tO;
		}

		internal static TCUnit FindClosestTCUnit(TEnemy obj, int radius) {
			TCUnit tO = null;
			List<Cell<TCUnit>> cells = CUGrid.GetCells<TEnemy>(obj);
			int minR = radius + 1;
			int r = 0;
			for (int i = 0; i < cells.Count; i++) {
				for (int j = 0; j < cells[i].Objects.Count; j++) {
					if (cells[i].Objects[j] is TCUnit) {
						r = (int)Math.Round(Math.Sqrt(sqr(obj.Center.X - cells[i].Objects[j].Center.X) + sqr(obj.Center.Y - cells[i].Objects[j].Center.Y)));
						if (r <= radius && r < minR) {
							tO = (TCUnit)cells[i].Objects[j];
							minR = r;
						}
					}
				}
			}
			return tO;
		}

		internal static TEnemy FindClosestTEnemy(TCUnit obj, int radius) {
			TEnemy tO = null;
			List<Cell<TEnemy>> cells = EGrid.GetCells<TCUnit>(obj);
			int minR = radius + 1;
			int r = 0;
			for (int i = 0; i < cells.Count; i++) {
				for (int j = 0; j < cells[i].Objects.Count; j++) {
					if (cells[i].Objects[j] is TEnemy) {
						r = (int)Math.Round(Math.Sqrt(sqr(obj.Center.X - cells[i].Objects[j].Center.X) + sqr(obj.Center.Y - cells[i].Objects[j].Center.Y)));
						if (r <= radius && r < minR) {
							tO = (TEnemy)cells[i].Objects[j];
							minR = r;
						}
					}
				}
			}
			return tO;
		}

		internal static TBuilding FindClosestTBuilding(TObject obj, int radius) {
			TBuilding tO = null;
			int minR = radius + 1;
			int r = 0;
			for (int i = 0; i <= TBuildings.Count - 1; i++) {
				r = (int)Math.Round(Math.Sqrt(sqr(obj.Center.X - TBuildings[i].Center.X) + sqr(obj.Center.Y - TBuildings[i].Center.Y)));
				if (r <= radius && r < minR) {
					tO = TBuildings[i];
					minR = r;
				}
			}
			return tO;
		}

		internal static TTree FindClosestTTree(TObject obj, int radius) {
			TTree tO = null;
			int minR = radius + 1;
			int r = 0;
			for (int i = 0; i <= TTrees.Count - 1; i++) {
				r = (int)Math.Round(Math.Sqrt(sqr(obj.Center.X - TTrees[i].Center.X) + sqr(obj.Center.Y - TTrees[i].Center.Y)));
				if (r <= radius && r < minR) {
					tO = TTrees[i];
					minR = r;
				}
			}
			return tO;
		}

		internal static TStone FindClosestTStone(TObject obj, int radius) {
			TStone tO = null;
			int minR = radius + 1;
			int r = 0;
			for (int i = 0; i <= TStones.Count - 1; i++) {
				r = (int)Math.Round(Math.Sqrt(sqr(obj.Center.X - TStones[i].Center.X) + sqr(obj.Center.Y - TStones[i].Center.Y)));
				if (r <= radius && r < minR) {
					tO = TStones[i];
					minR = r;
				}
			}
			return tO;
		}
		#endregion
        #endregion
    }
}
