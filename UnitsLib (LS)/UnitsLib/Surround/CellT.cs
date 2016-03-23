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

namespace UnitsLib.Surround
{
    /// <summary>
    /// Класс клетка
    /// </summary>
    /// <typeparam name="T">Тип объектов, находящихся внутри клетки, где T : TObject</typeparam>
	internal class Cell<T> where T : TObject 
    {
        List<T> objects;
		int cellX, cellY;
        int cellW, cellH;
		Rectangle cellRect;
		Point index;
		Grid<T> ownerGrid;
		List<Cell<T>> neighbors;

		/// <summary>
		/// Прямоугольник клетки
		/// </summary>
		internal Rectangle GridRect {
			get { return cellRect; }
		}

        /// <summary>
        /// Массив объектов, находящихся в клетке
        /// </summary>
		internal List<T> Objects {
            get { return objects; }
            set {
                objects = value;
            }
        }

		/// <summary>
		/// Клетки соседи + сама клетка (только для чтения)
		/// </summary>
		internal List<Cell<T>> Neighbors { get { return neighbors; } }

        /// <summary>
        /// Создает новую клетку, обнуляя все объекты
        /// </summary>
        /// <param name="objCount">Максимально возможное кол-во объектов в клетке</param>
		internal Cell(Grid<T> ownerGrid, int cellX, int cellY, int cellW, int cellH, Point index) {
			this.cellX = cellX;
			this.cellY = cellY;
			this.cellW = cellW;
			this.cellH = cellH;
			this.index = index;
			this.ownerGrid = ownerGrid;
			neighbors = new List<Cell<T>>();
			cellRect = new Rectangle(cellX, cellY, cellW, cellH);
			objects = new List<T>();
        }

        /// <summary>
        /// Добавляет объект в клетку
        /// </summary>
        /// <param name="obj">Объект, который добавляется в клетку</param>
		internal Cell<T> AddObject(T obj) {
			objects.Add(obj);
			return this;
        }

        /// <summary>
        /// Удаляет первое вхождение объекта из клетки
        /// </summary>
		internal Cell<T> RemoveObject(T obj) {
			objects.Remove(obj);
			return this;
        }

		/// <summary>
		/// Устанавливает соседей для клетки в восьми направлениях и включает туда саму клетку
		/// </summary>
		internal void SetNeighbors() {
			neighbors = new List<Cell<T>>();
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					if (ownerGrid.RowIsValide(index.Y + i) && ownerGrid.ColumnIsValide(index.X + j))
						neighbors.Add(ownerGrid.Cells[index.Y + i][index.X + j]);
				}
			}
		}
    }

    /// <summary>
    /// Класс сетка
    /// </summary>
    /// <typeparam name="T">Тип объектов, находящихся в сетке, где T : TObject</typeparam>
	internal class Grid<T> where T : TObject
    {
        List<List<Cell<T>>> cells;
        int cellCountW, cellCountH;
        int cellW, cellH;

        /// <summary>
        /// Двумерный список клеток (только для чтения)
        /// </summary>
		internal List<List<Cell<T>>> Cells { get { return cells; } private set { cells = value; } }

        /// <summary>
        /// Создает новую сетку размером gridCountW x gridCountH.
        /// </summary>
        /// <param name="gridCountW">Кол-во клеток в одной строке</param>
        /// <param name="gridCountH">Кол-во строк</param>
		/// <param name="gridW">Ширина одной клетки</param>
		/// <param name="gridH">Высота одной клетки</param>
		internal Grid(int cellCountW, int cellCountH, int cellW, int cellH) {
			this.cellCountW = cellCountW;
			this.cellCountH = cellCountH;
			this.cellW = cellW;
			this.cellH = cellH;
			cells = new List<List<Cell<T>>>();
			for (int i = 0; i < cellCountH; i++) {
				cells.Add(new List<Cell<T>>());
				for (int j = 0; j < cellCountW; j++) {
					cells[i].Add(new Cell<T>(this, j * cellW, i * cellH, cellW, cellH, new Point(j, i)));
                }
            }
			for (int i = 0; i < cells.Count; i++) {
				for (int j = 0; j < cells[i].Count; j++) {
					cells[i][j].SetNeighbors();
				}
			}
        }

        /// <summary>
        /// Возвращает клетку под индексом [row,column]
        /// </summary>
        /// <param name="row">Число должно быть меньше кол-ва строк</param>
        /// <param name="column">Число должно быть меньше кол-ва клеток в одной строке</param>
		internal Cell<T> GetCell(int row, int column) {
            CheckValues(row, column);
			return Cells[row][column];
        }

        /// <summary>
        /// Возвращает массив объектов, заключенных в клетке под индексом [row,column]
        /// </summary>
        /// <param name="row">Число должно быть меньше кол-ва строк</param>
        /// <param name="column">Число должно быть меньше кол-ва клеток в одной строке</param>
		internal List<T> GetObjects(int row, int column) {
            CheckValues(row, column);
			return cells[row][column].Objects;
        }

        /// <summary>
        /// Устанавливает клетку в ячейку под индексом [row,column]
        /// </summary>
        /// <param name="row">Число должно быть меньше кол-ва строк</param>
        /// <param name="column">Число должно быть меньше кол-ва клеток в одной строке</param>
        /// <param name="grid">Клетка, которая устанавливается в сетку</param>
		internal void SetCell(int row, int column, Cell<T> cell) {
            CheckValues(row, column);
			cells[row][column] = cell;
        }

        /// <summary>
        /// Добавляет объект в клетку под индексом [row,column]
        /// </summary>
        /// <param name="row">Число должно быть меньше кол-ва строк</param>
        /// <param name="column">Число должно быть меньше кол-ва клеток в одной строке</param>
        /// <param name="obj">Объект, который добавляется в клетку</param>
		internal Cell<T> AddObject(T obj) {
			int x = (int)obj.Position.X;
			int y = (int)obj.Position.Y;
			int row = y / cellH + ((y % cellH == 0) ? 0 : 1) - ((y == 0) ? 0 : 1);
			int column = x / cellW + ((x % cellW == 0) ? 0 : 1) - ((x == 0) ? 0 : 1);
			try {
				return cells[row][column].AddObject(obj);
			} catch (ArgumentOutOfRangeException) {
				if (row == cells.Count)
					row -= 1;
				if (row == -1)
					row += 1;
				if (column == cells[row].Count)
					column -= 1;
				if (column == -1)
					column += 1;
			}
			return cells[row][column].AddObject(obj);
        }

        /// <summary>
        /// Удаляет объект из клетки под индексом [row, column]
        /// </summary>
        /// <param name="row">Число должно быть меньше кол-ва строк</param>
        /// <param name="column">Число должно быть меньше кол-ва клеток в одной строке</param>
        /// <param name="obj">Объект, который удаляется из клетки</param>
		internal Cell<T> RemoveObject(T obj) {
			int x = (int)obj.Position.X;
			int y = (int)obj.Position.Y;
			int row = y / cellH + ((y % cellH == 0) ? 0 : 1) - ((y == 0) ? 0 : 1);
			int column = x / cellW + ((x % cellW == 0) ? 0 : 1) - ((x == 0) ? 0 : 1);
			return cells[row][column].RemoveObject(obj);
        }

		internal List<Cell<T>> GetCells<TFinder>(TFinder obj) where TFinder : TObject {
			int x = (int)obj.Position.X;
			int y = (int)obj.Position.Y;
			int row = y / cellH + ((y % cellH == 0) ? 0 : 1) - ((y == 0) ? 0 : 1);
			int column = x / cellW + ((x % cellW == 0) ? 0 : 1) - ((x == 0) ? 0 : 1);
			return Cells[row][column].Neighbors;
        }
		
		internal bool RowIsValide(int row) {
			return row < cellCountH && row >= 0;
		}
		internal bool ColumnIsValide(int column) {
			return column < cellCountW && column >= 0;
		}

        private void CheckValues(int row, int column) {
			if (row >= cellCountH || row <= -1)
                throw new ArgumentOutOfRangeException("row", "Номер строки должен быть меньше кол-ва строк и больше -1");
			if (column >= cellCountW || column <= -1)
                throw new ArgumentOutOfRangeException("collumn", "Номер столбца должен быть меньше кол-ва элементов в одной строке и больше -1");
        }		
    }
}
