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
using UnitsLib.Interfaces;
using UnitsLib.Surround;
using UnitsLib.Events;

namespace UnitsLib.Surround
{
	public static class MyExtensions
	{
        /// <summary>
        /// Возвращает новый вектор с округленными компонентами
        /// </summary>
        public static Vector2 Round(this Vector2 v) {
            return new Vector2((float)Math.Round(v.X), (float)Math.Round(v.Y));
        }
        /// <summary>
        /// Форматирует вектор в соответствии с окном текущей игры
        /// </summary>
        public static Vector2 Format(this Vector2 v, Game game) {
            return new Vector2(v.X * (float)game.Window.ClientBounds.Width / 1280f, v.Y * (float)game.Window.ClientBounds.Height / 1024f);
        }

		public static float FormatW(this float width, Game game) {
			return width * (float)game.Window.ClientBounds.Width / 1280f;
		}

		public static float FormatH(this float height, Game game) {
			return height * (float)game.Window.ClientBounds.Height / 1024f;
		}

		/// <summary>
		/// Копирует элементы объекта List T в новый объект Queue T в соответствии с порядком элементов
		/// </summary>
		public static Queue<T> ToQueue<T>(this List<T> list) {
			Queue<T> result = new Queue<T>();
			if (list.Count > 0)
				for (int i = list.Count - 1; i >= 0; i--) {
					result.Enqueue(list[i]);
				}
			return result;
		}

		/// <summary>
		/// Возвращает средний размер для списка спрайтов
		/// </summary>
		public static Vector2 AverrageSize(this List<Sprite> list) {
			float avX = 0, avY = 0;
			int count = 0;
			if (list.Count == 0)
				return Vector2.One;
			for (int i = 0; i < list.Count; i++) {
				count++;
				avX += list[i].Size.X;
				avY += list[i].Size.Y;
			}
			return new Vector2(avX / count, avY / count);
		}
		public static Vector2 AverrageSize(this List<IcoWithHP> list) {
			float avX = 0, avY = 0;
			int count = 0;
			if (list.Count == 0)
				return Vector2.One;
			for (int i = 0; i < list.Count; i++) {
				count++;
				avX += list[i].Size.X;
				avY += list[i].Size.Y;
			}
			return new Vector2(avX / count, avY / count);
		}

		public static TStoneSaver ToSaver(this TObject obj) {
			TStoneSaver saver;
			saver.Position = obj.Position;
			saver.Size = new Vector2(obj.Width, obj.Height);
			saver.TextureFileName = obj.Image.FileName;
			saver.Visible = obj.Visible;
			saver.IsFocused = obj.IsFocused;
			return saver;
		}
		//public static TObject FromSaver(this TObjectSaver saver) {

		//}
	}

	[Serializable]
	public struct TStoneSaver
	{
		internal string TextureFileName;
		internal Vector2 Position;
		internal Vector2 Size;
		internal bool Visible;
		internal bool IsFocused;
	}
}
