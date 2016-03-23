using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace UnitsLib.Surround.Algorithms
{
	internal static class ModifiedAStar //TODO направленная волна из начальной И конечной точки
	{
		static List<Cell> open = new List<Cell>();
		static List<Cell> close = new List<Cell>();
		static List<Cell> borderOpen = new List<Cell>();
		static Grid grid = World.AGrid;
		static int minH = int.MaxValue;
		static Cell cellWithMinH;

		internal static List<Vector2> SearchPath(TUnit unit, Cell startBlockCell, out bool unitInBlock) {
			ToNativeState();
			Cell endCell = grid.GetCell(unit.P.X, unit.P.Y);
			RealizeBlock(startBlockCell, endCell);
            //if (/*!grid.GetCell(unit.Center.X, unit.Center.Y).IsPassable ||*/ !grid.GetCell(unit.P.X, unit.P.Y).IsPassable)
            //    throw new ArgumentException("Клетка юнита или клетка P юнита непроходима");
			if (!grid.GetCell(unit.Center.X, unit.Center.Y).IsPassable) {
				unitInBlock = true;
				return new List<Vector2>();
			}
			unitInBlock = false;
			if (minH == int.MaxValue || cellWithMinH == null)
				return new List<Vector2>();
			if (endCell == cellWithMinH)
				return AStar.SearchPathAsList(unit);
			return AStar.SearchPathAsList(unit, grid.GetCell(unit.Center.X, unit.Center.Y), cellWithMinH);
		}

		internal static Cell GetCellWithMinH(Cell endCell) {
			ToNativeState();
			RealizeBlock(endCell);
			if (cellWithMinH != null) {
				return cellWithMinH;
			}
			//int mH = int.MaxValue;
			//Cell result = null;
			//if (list != null && list.Count != 0)
			//    for (int i = 0; i < list.Count; i++) {
			//        int H = 10 * (Math.Abs(endCell.Index.X - list[i].Index.X) + Math.Abs(endCell.Index.Y - list[i].Index.Y));
			//        if (H < mH) {
			//            mH = H;
			//            result = list[i];
			//        }
			//    }
			return null;
		}
		///// <summary>
		///// Используется внутренний список клеток границы
		///// </summary>
		//internal static Cell GetCellWithMinH(Cell endCell) {
		//    return GetCellWithMinH(endCell, borderOpen);
		//}

		#region PrivateMethods

		private static void RealizeBlock(Cell startBlockCell, Cell endCell) {
			if (startBlockCell.IsPassable || endCell == null)
				return;
			AddToOList(startBlockCell);
			while (open.Count != 0) {
				Cell x = open[open.Count - 1];
				for (int i = 0; i < x.Neighbors.Count; i++) {
					if (x.Neighbors[i].IsAddedToBlockCList || x.Neighbors[i].IsAddedToBlockOList)
						continue;
					if (!x.Neighbors[i].IsPassable)
						AddToOList(x.Neighbors[i]);
					else {
						AddToBorderOList(x.Neighbors[i]);
						int H = 10 * (Math.Abs(endCell.Index.X - x.Neighbors[i].Index.X) + Math.Abs(endCell.Index.Y - x.Neighbors[i].Index.Y));
						if (H < minH) {
							cellWithMinH = x.Neighbors[i];
							minH = H;
						}
					}
				}
				RemoveFromOList(x);
				AddToCList(x);
			}
		}
		private static void RealizeBlock(Cell startBlockCell) {
			if (startBlockCell == null)
				return;
			AddToOList(startBlockCell);
			while (open.Count != 0) {
				Cell x = open[open.Count - 1];
				for (int i = 0; i < x.Neighbors.Count; i++) {
					if (x.Neighbors[i].IsAddedToBlockCList || x.Neighbors[i].IsAddedToBlockOList)
						continue;
					if (!x.Neighbors[i].IsPassable)
						AddToOList(x.Neighbors[i]);
					else {
						AddToBorderOList(x.Neighbors[i]);
						int H = 10 * (Math.Abs(startBlockCell.Index.X - x.Neighbors[i].Index.X) + Math.Abs(startBlockCell.Index.Y - x.Neighbors[i].Index.Y));
						if (H < minH) {
							cellWithMinH = x.Neighbors[i];
							minH = H;
						}
					}
				}
				RemoveFromOList(x);
				AddToCList(x);
			}
		}

		private static void ToNativeState() {
			minH = int.MaxValue;
			cellWithMinH = null;
			if (open.Count != 0)
				for (int i = open.Count - 1; i >= 0; i--) {
					open[i].G = 0;
					open[i].H = 0;
					open[i].IsAddedToBlockOList = false;
					open[i].Owner = null;
					open.RemoveAt(i);
				}
			if (close.Count != 0)
				for (int i = close.Count - 1; i >= 0; i--) {
					close[i].G = 0;
					close[i].H = 0;
					close[i].IsAddedToBlockCList = false;
					close[i].Owner = null;
					close.RemoveAt(i);
				}
			if (borderOpen.Count != 0)
				for (int i = borderOpen.Count - 1; i >= 0; i--) {
					borderOpen[i].G = 0;
					borderOpen[i].H = 0;
					borderOpen[i].IsAddedToBorderOList = false;
					borderOpen[i].Owner = null;
					borderOpen.RemoveAt(i);
				}
		}

		private static void RemoveFromOList(Cell cell) {
			if (cell.IsAddedToBlockOList) {
				open.Remove(cell);
				cell.IsAddedToBlockOList = false;
			}
		}
		private static void AddToOList(Cell cell) {
			if (!cell.IsAddedToBlockOList) {
				open.Add(cell);
				cell.IsAddedToBlockOList = true;
			}
		}
		private static void RemoveFromCList(Cell cell) {
			if (cell.IsAddedToBlockCList) {
				close.Remove(cell);
				cell.IsAddedToBlockCList = false;
			}
		}
		private static void AddToCList(Cell cell) {
			if (!cell.IsAddedToBlockCList) {
				close.Add(cell);
				cell.IsAddedToBlockCList = true;
			}
		}

		private static void RemoveFromBorderOList(Cell cell) {
			if (cell.IsAddedToBorderOList) {
				borderOpen.Remove(cell);
				cell.IsAddedToBorderOList = false;
			}
		}
		private static void AddToBorderOList(Cell cell) {
			if (!cell.IsAddedToBorderOList) {
				borderOpen.Add(cell);
				cell.IsAddedToBorderOList = true;
			}
		}

		#endregion
	}
}
