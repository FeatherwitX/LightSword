using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace UnitsLib.Surround.Algorithms
{
	internal static class AStar
	{
		static List<Cell> open = new List<Cell>();
		static List<Cell> close = new List<Cell>();
		static Cell start;
		static Cell end;
		static Grid grid = World.AGrid;

		#region SearchPath as Queue<Cell>

		internal static Queue<Cell> SearchPath(TUnit unit) {
		    ToNativeState();
			start = grid.GetCell(unit.Center.X, unit.Center.Y);
			end = grid.GetCell(unit.P.X, unit.P.Y);

			AddToOList(start);
			start.G = 0;
			start.H = 10 * (Math.Abs(end.Index.X - start.Index.X) + Math.Abs(end.Index.Y - start.Index.Y));

			while (open.Count != 0) {
				Cell x = CellWithMinF(open);
				if (x == end) {
					return CompleteSolution(x);
				}
				RemoveFromOList(x);
				AddToCList(x);
				for (int i = 0; i < x.Neighbors.Count; i++) {
					Cell neighbor = x.Neighbors[i];
					if (neighbor.IsAddedToCList || !neighbor.IsPassable || isForcedNeighbor(x, neighbor)) {
						continue;
					}
					int g = (neighbor.Index.X != x.Index.X && neighbor.Index.Y != x.Index.Y) ? 14 + x.G : 10 + x.G;
					bool isGBetter = false;
					if (!neighbor.IsAddedToOList) {
						neighbor.H = 10 * (Math.Abs(end.Index.X - neighbor.Index.X) + Math.Abs(end.Index.Y - neighbor.Index.Y));
						AddToOList(neighbor);
						isGBetter = true;
					} else {
						isGBetter = g < neighbor.G;
					}
					if (isGBetter) {
						neighbor.Owner = x;
						neighbor.G = (neighbor.Index.X != neighbor.Owner.Index.X && neighbor.Index.Y != neighbor.Owner.Index.Y) ? 14 + neighbor.Owner.G : 10 + neighbor.Owner.G;
					}
				}
			}
		    return new Queue<Cell>();
		}

		#endregion

		#region SearchPath as List<Vector2>

		internal static List<Vector2> SearchPathAsList(TUnit unit) {
			return SearchPathAsList(unit, grid.GetCell(unit.Center.X, unit.Center.Y), grid.GetCell(unit.P.X, unit.P.Y));
		}
		internal static List<Vector2> SearchPathAsList(TUnit unit, Cell start, Cell end) {
			ToNativeState();

			AddToOList(start);
			start.G = 0;
			start.H = 10 * (Math.Abs(end.Index.X - start.Index.X) + Math.Abs(end.Index.Y - start.Index.Y));

			while (open.Count != 0) {
				Cell x = CellWithMinF(open);
				if (x == end) {
					return CompleteSolutionAsList(x);
				}
				RemoveFromOList(x);
				AddToCList(x);
				for (int i = 0; i < x.Neighbors.Count; i++) {
					Cell neighbor = x.Neighbors[i];
					if (neighbor.IsAddedToCList || !neighbor.IsPassable || isForcedNeighbor(x, neighbor)) {
						continue;
					}
					int g = (neighbor.Index.X != x.Index.X && neighbor.Index.Y != x.Index.Y) ? 14 + x.G : 10 + x.G;
					bool isGBetter = false;
					if (!neighbor.IsAddedToOList) {
						neighbor.H = 10 * (Math.Abs(end.Index.X - neighbor.Index.X) + Math.Abs(end.Index.Y - neighbor.Index.Y));
						AddToOList(neighbor);
						isGBetter = true;
					} else {
						isGBetter = g < neighbor.G;
					}
					if (isGBetter) {
						neighbor.Owner = x;
						neighbor.G = (neighbor.Index.X != neighbor.Owner.Index.X && neighbor.Index.Y != neighbor.Owner.Index.Y) ? 14 + neighbor.Owner.G : 10 + neighbor.Owner.G;
					}
				}
			}
			return new List<Vector2>();
		}

		#endregion

		#region SearchPath as Stack<Vector2>

		internal static Stack<Vector2> SearchPathAsStack(TUnit unit) {
			ToNativeState();
			start = grid.GetCell(unit.Center.X, unit.Center.Y);
			end = grid.GetCell(unit.P.X, unit.P.Y);

			AddToOList(start);
			start.G = 0;
			start.H = 10 * (Math.Abs(end.Index.X - start.Index.X) + Math.Abs(end.Index.Y - start.Index.Y));

			while (open.Count != 0) {
				Cell x = CellWithMinF(open);
				if (x == end) {
					return CompleteSolutionAsStack(x);
				}
				RemoveFromOList(x);
				AddToCList(x);
				for (int i = 0; i < x.Neighbors.Count; i++) {
					Cell neighbor = x.Neighbors[i];
					if (neighbor.IsAddedToCList || !neighbor.IsPassable || isForcedNeighbor(x, neighbor)) {
						continue;
					}
					int g = (neighbor.Index.X != x.Index.X && neighbor.Index.Y != x.Index.Y) ? 14 + x.G : 10 + x.G;
					bool isGBetter = false;
					if (!neighbor.IsAddedToOList) {
						neighbor.H = 10 * (Math.Abs(end.Index.X - neighbor.Index.X) + Math.Abs(end.Index.Y - neighbor.Index.Y));
						AddToOList(neighbor);
						isGBetter = true;
					} else {
						isGBetter = g < neighbor.G;
					}
					if (isGBetter) {
						neighbor.Owner = x;
						neighbor.G = (neighbor.Index.X != neighbor.Owner.Index.X && neighbor.Index.Y != neighbor.Owner.Index.Y) ? 14 + neighbor.Owner.G : 10 + neighbor.Owner.G;
					}
				}
			}
			return new Stack<Vector2>();
		}

		#endregion

		#region Private Methods

		private static Cell CellWithMinF(List<Cell> list) {
			int min = int.MaxValue;
			Cell result = null;
			for (int i = 0; i < list.Count; i++) {
				if (list[i].F < min) {
					result = list[i];
					min = list[i].F;
				}
			}
			return result;
		}

		private static bool isForcedNeighbor(Cell current, Cell neighbor) {
			Point diff = new Point(neighbor.Index.X - current.Index.X, neighbor.Index.Y - current.Index.Y);
			if (grid.Cells[current.Index.X + diff.X, current.Index.Y].IsPassable && grid.Cells[current.Index.X, current.Index.Y + diff.Y].IsPassable)
				return false;
			else
				return true;
		}

		private static Queue<Cell> CompleteSolution(Cell end) {
			Queue<Cell> path = new Queue<Cell>();
			Cell c = end;
			while (c != null) {
				path.Enqueue(c);
				c = c.Owner;
			}
			return path;
		}
		private static List<Vector2> CompleteSolutionAsList(Cell end) {
			List<Vector2> path = new List<Vector2>();
			Cell c = end;
			while (c != null) {
				path.Add(c.Center);
				c = c.Owner;
			}
			return path;
		}
		private static Stack<Vector2> CompleteSolutionAsStack(Cell end) {
			Stack<Vector2> path = new Stack<Vector2>();
			Cell c = end;
			while (c != null) {
				path.Push(c.Center);
				c = c.Owner;
			}
			return path;
		}

		private static void ToNativeState() {
			start = null;
			end = null;
			if (open.Count != 0)
				for (int i = open.Count - 1; i >= 0; i--) {
					open[i].G = 0;
					open[i].H = 0;
					open[i].IsAddedToOList = false;
					open[i].Owner = null;
					open.RemoveAt(i);
				}
			if (close.Count != 0)
				for (int i = close.Count - 1; i >= 0; i--) {
					close[i].G = 0;
					close[i].H = 0;
					close[i].IsAddedToCList = false;
					close[i].Owner = null;
					close.RemoveAt(i);
				}
		}

		private static void RemoveFromOList(Cell cell) {
			if (cell.IsAddedToOList) {
				open.Remove(cell);
				cell.IsAddedToOList = false;
			}
		}
		private static void AddToOList(Cell cell) {
			if (!cell.IsAddedToOList) {
				open.Add(cell);
				cell.IsAddedToOList = true;
			}
		}
		private static void RemoveFromCList(Cell cell) {
			if (cell.IsAddedToCList) {
				close.Remove(cell);
				cell.IsAddedToCList = false;
			}
		}
		private static void AddToCList(Cell cell) {
			if (!cell.IsAddedToCList) {
				close.Add(cell);
				cell.IsAddedToCList = true;
			}
		}

		#endregion
	}
}
