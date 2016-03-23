using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitsLib.Enums;

namespace UnitsLib.Interfaces
{
	/// <summary>
	/// Интерфейс очереди
	/// </summary>
	public interface IQueue
	{
		/// <summary>
		/// Очередь создания
		/// </summary>
		Queue<UnitNames> Q { get; set; }
		/// <summary>
		/// Отчистить очередь создания
		/// </summary>
		void Clear();
		/// <summary>
		/// Добавить в конец очереди
		/// </summary>
		/// <param name="unitName">Имя юнита для добавления</param>
		void ToQueue(UnitNames unitName);
		/// <summary>
		/// Удаляет и возвращает элемент очереди, находящийся в начале
		/// </summary>
		UnitNames FromQueue();
	}
}
