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
using UnitsLib.Surround;
using UnitsLib.Interfaces;

namespace Light_Sword
{
	/// <summary>
	/// Класс, инкапсулирующий прямоугольник, обозначающий место для строительства. 
	/// Является надсройкой над Sprite
	/// </summary>
	public sealed class BuildingRectangle
	{
		Sprite sprite;
		Vector2 drawPosition;
		Vector2 position;
		Vector2 size;
		bool vis;
		Color color;
		internal BoundingBox box;

		/// <summary>
		/// Название здания
		/// </summary>
		public BuildingNames Name { get; set; }
		/// <summary>
		/// Виден ли прямоугольник
		/// </summary>
		public bool Visible {
			get { return vis; }
			set {
				vis = value;
				if (!vis)
					DrawPosition = new Vector2(-500, -500);
				sprite.Visible = vis;
				
			}
		}
		/// <summary>
		/// Позиция, в которой рисуется прямоугольник
		/// </summary>
		public Vector2 DrawPosition {
			get { return drawPosition; }
			set {
				drawPosition = value;
				sprite.Position = value;
			}
		}
		/// <summary>
		/// Позиция прямоугольника в мировых координатах
		/// </summary>
		public Vector2 Position {
			get { return position; }
			set {
				position = value;
				box = new BoundingBox(new Vector3(position, 0), new Vector3(position + size, 0));
			}
		}
		/// <summary>
		/// Размер прямоугольника
		/// </summary>
		public Vector2 Size {
			get { return size; }
			set {
				size = value;
				sprite.Size = value;
				box = new BoundingBox(new Vector3(position, 0), new Vector3(position + size, 0));
			}
		}
		/// <summary>
		/// Цвет прямоугольника
		/// </summary>
		public Color Color {
			get { return color; }
			set {
				color = value;
				sprite.Color = value;
			}
		}

		/// <summary>
		/// Создает стоительный прямоугольник с указанным спрайтом
		/// </summary>
		public BuildingRectangle(Sprite sprite) {
			this.sprite = sprite;
			DrawPosition = new Vector2(-500, -500);
		}

		/// <summary>
		/// Устанавливает параметры строительного прямоугольника относительно имени здания
		/// </summary>
		public void SetRectangle(BuildingNames bname) {
			switch (bname) {
				case BuildingNames.CityCenter:
					break;
				case BuildingNames.Farm:
					Size = new Vector2(120, 67);
					break;
				case BuildingNames.Baracks:
					Size = new Vector2(97, 92);
					break;
				default:
					return;
			}
			Name = bname;
			Visible = true;
			sprite.Color = World.GreenBuildingRectangle;
		}

		public void Update(MouseState ms, MouseState oms, Scene scene) {
			if (Visible) {
				Position = new Vector2(ms.X + scene.Rect.X, ms.Y + scene.Rect.Y) - new Vector2((float)Math.Round(Size.X / 2), (float)Math.Round(Size.Y / 2)); ;
				DrawPosition = new Vector2(ms.X, ms.Y) - new Vector2((float)Math.Round(Size.X / 2), (float)Math.Round(Size.Y / 2));
				for (int i = 0; i <= World.TObjects.Count - 1; i++) {
					Color = World.GreenBuildingRectangle;
					if (Intersect(World.TObjects[i])) {
						Color = World.RedBuildingRectangle;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Рисует строительный прямоугольник
		/// </summary>
		public void Draw(SpriteBatch spriteBatch) {
			sprite.Draw(spriteBatch);
		}

		/// <summary>
		/// Пересекается ли строительный прямоугольник с игровым объектом obj
		/// </summary>
		public bool Intersect(TObject obj) {
			return box.Contains(obj.Box) != ContainmentType.Disjoint;
		}
	}
}
