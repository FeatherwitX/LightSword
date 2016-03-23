using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitsLib.Enums;

namespace UnitsLib
{
	/// <summary>
	/// Интерфейс атаки
	/// </summary>
	public interface IAttack
	{
		/// <summary>
		/// Мод агрессии
		/// </summary>
		AgrModes AgrMode { get; set; }
		/// <summary>
		/// Радиус агрессии
		/// </summary>
		int AgrR { get; }
		/// <summary>
		/// Размер атаки
		/// </summary>
		int Atack { get; set; }
		/// <summary>
		/// Атака (непосредственное нанесение урона)
		/// </summary>
		void Attack();
		/// <summary>
		/// Проверка территории в радиусе агрессии
		/// </summary>
		void CheckArea();
	}
}
