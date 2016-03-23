using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using UnitsLib.Enums;

namespace UnitsLib
{
	/// <summary>
	/// Интерфейс источника
	/// </summary>
	public interface ISource
	{
		/// <summary>
		/// Запас источника
		/// </summary>
		int Supply { get; set; }
		/// <summary>
		/// Максимальный запас источника (только для чтения)
		/// </summary>
		int MaxSupply { get; }
		/// <summary>
		/// Тип ресурса у этого источника (только для чтения)
		/// </summary>
		SourceTypes SourceType { get; }
		/// <summary>
		/// Центр источника
		/// </summary>
		Vector2 Center { get; set; }
		/// <summary>
		/// Событие смерти
		/// </summary>
		event EventHandler Death;
	}
}
