using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using UnitsLib.Surround;

namespace UnitsLib
{
	/// <summary>
	/// Интерфейс движения
	/// </summary>
	public interface IMove
	{
		/// <summary>
		/// Вектор скорости
		/// </summary>
		Vector2 Velocity { get; set; }
		/// <summary>
		/// Коэффициент скорости
		/// </summary>
		float Speed { get; set; }
		/// <summary>
		/// Движение на вектор скорости
		/// </summary>
		void Move(Scene scene);
		/// <summary>
		/// Остановка объекта
		/// </summary>
		void Stop();
	}
}
