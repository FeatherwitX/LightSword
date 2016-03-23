using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Graph;
using Sounds;
using UnitsLib;
using UnitsLib.Enums;
using UnitsLib.Events;
using UnitsLib.Interfaces;

namespace UnitsLib.Surround
{
	public class Cell : IComparable<Cell>
	{
		public const int CellWidth = 8;

		#region Variables

		Grid ownerGrid;
		Point index;
		int width = CellWidth;
		Vector2 center;
		bool isPassable;
		int f, g, h;
		Cell ownerCell;
		List<Cell> neighbors;
		int initPassableLayers;
		int passableLayers;

		#endregion

		/// <summary>
		/// Суммарная стоимость продвижения (только для чтения)
		/// </summary>
		public int F { get { return f; } }
		/// <summary>
		/// Стоимость передвижения из стартовой точки до этой
		/// </summary>
		public int G {
			get { return g; }
			set {
				g = value;
				f = g + h;
			}
		}
		/// <summary>
		/// Эвристическая оценка свтоимости передвижения
		/// </summary>
		public int H {
			get { return h; }
			set {
				h = value;
				f = g + h;
			}
		}
		/// <summary>
		/// Родительская клетка
		/// </summary>
		public Cell Owner {
			get { return ownerCell; }
			set { ownerCell = value; }
		}
		/// <summary>
		/// Клетки соседи
		/// </summary>
		public List<Cell> Neighbors { get { return neighbors; } }
		/// <summary>
		/// Добавлена ли клетка в открытый список
		/// </summary>
		public bool IsAddedToOList { get; internal set; }
		/// <summary>
		/// Добавлена ли клетка в закрытый список
		/// </summary>
		public bool IsAddedToCList { get; internal set; }
		public bool IsAddedToBlockOList { get; internal set; }
		public bool IsAddedToBlockCList { get; internal set; }
		public bool IsAddedToBorderOList { get; internal set; }
		/// <summary>
		/// Если True, увеличение наложения слоя объектов на 1, иначе уменьшение на 1
		/// </summary>
		public bool IsPassable {
			get { return isPassable; }
			set {
				passableLayers = (value) ? --passableLayers : ++passableLayers;
				passableLayers = (passableLayers < 0) ? 0 : passableLayers;
				initPassableLayers = passableLayers;
				isPassable = passableLayers == 0;
			}
		}
		/// <summary>
		/// Непосредственная установка проходимости
		/// </summary>
		public bool DirectPassable {
			get { return isPassable; }
			set {
				isPassable = value;
				passableLayers = (isPassable) ? 0 : initPassableLayers;
			}
		}
		/// <summary>
		/// Индекс клетки в сетке (только для чтения)
		/// </summary>
		public Point Index { get { return index; } }
		public Vector2 Center { get { return center; } }

		/// <summary>
		/// Создает клетку с указанными параметрами
		/// </summary>
		/// <param name="ownerGrid">Сетка, в которую входит клетка</param>
		/// <param name="row">Строка, в которой находится клетка</param>
		/// <param name="column">Колонка, в которой находится клетка</param>
		/// <param name="isPassable">Проходима ли клетка</param>
		public Cell(Grid ownerGrid, int row, int column, bool isPassable = true) {
			index = new Point(row, column);
			center = new Vector2(row * CellWidth + (float)CellWidth / 2, column * CellWidth + (float)CellWidth / 2);
			this.isPassable = isPassable;
			this.passableLayers = (isPassable) ? 0 : 1;
			this.initPassableLayers = passableLayers;
			this.ownerGrid = ownerGrid;
			this.ownerCell = null;
			this.neighbors = new List<Cell>();
			IsAddedToOList = false;
			IsAddedToCList = false;
		}
		
		/// <summary>
		/// Устанавливает для клетки соседей в восьми направлениях
		/// </summary>
		public void SetNeighbors() {
			neighbors = new List<Cell>();
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					if (i == 0 && j == 0)
						continue;
					if (ownerGrid.RowIsValide(index.X + i) && ownerGrid.ColumnIsValide(index.Y + j))
						neighbors.Add(ownerGrid.Cells[index.X + i, index.Y + j]);
				}
			}
		}

		/// <summary>
		/// Сравнивает две клетки на основе их F стоимости
		/// </summary>
		public int CompareTo(Cell cell) {
			if (this.f > cell.f)
				return 1;
			else if (this.f < cell.f)
				return -1;
			else
				return 0;
		}
	}

	public class Grid
	{
		Cell[,] cells;
		int cellCountW, cellCountH;
		int cellWidth = Cell.CellWidth;

		/// <summary>
		/// Двумерный массив клеток (только для чтения)
		/// </summary>
		public Cell[,] Cells { get { return cells; } }
		/// <summary>
		/// Ширина одной клетки (только для чтения)
		/// </summary>
		public int CellWidth { get { return cellWidth; } }

		/// <summary>
		/// Создает новую сетку с указанными параметрами
		/// </summary>
		/// <param name="cellCountH">Кол-во клеток в высоту</param>
		/// <param name="cellWidth">Кол-во клеток в ширину</param>
		public Grid(int cellCountH, int cellCountW) {
			this.cellCountH = cellCountH;
			this.cellCountW = cellCountW;
			cells = new Cell[cellCountH, cellCountW];
			for (int i = 0; i < cellCountH; i++) {
				for (int j = 0; j < cellCountW; j++) {
					cells[i, j] = new Cell(this, i, j);
				}
			}
			for (int i = 0; i < cellCountH; i++) {
				for (int j = 0; j < cellCountW; j++) {
					cells[i, j].SetNeighbors();
				}
			}
		}

		/// <summary>
		/// Возвращает клетку, которой принадлежит точка (x,y)
		/// </summary>
		/// <param name="x">Координата по оси X</param>
		/// <param name="y">Координата по оси Y</param>
		public Cell GetCell(float x, float y) {
			int X = (x < 0) ? 0 : (int)x;
			int Y = (y < 0) ? 0 : (int)y;
			return Cells[X / CellWidth, Y / CellWidth];
		}

		internal bool RowIsValide(int row) {
			return row < cellCountH && row >= 0;
		}
		internal bool ColumnIsValide(int column) {
			return column < cellCountW && column >= 0;
		}
	}
}
