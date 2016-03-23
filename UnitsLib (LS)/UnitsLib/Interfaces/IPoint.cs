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

namespace UnitsLib.Interfaces
{
	/// <summary>
	/// Интерфейс точки
	/// </summary>
	public interface IPoint
	{
		Vector2 P { get; set; }
		void CheckFlag(Scene scene);
	}

	/// <summary>
	/// Интерфейс очереди точек
	/// </summary>
	public interface IQueuePoint : IPoint
	{
		Queue<Vector2> QP { get; set; }
	}
}
