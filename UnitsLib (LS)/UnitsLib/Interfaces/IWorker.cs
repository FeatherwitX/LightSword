using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitsLib.Enums;
using UnitsLib;

namespace UnitsLib.Interfaces
{
	/// <summary>
	/// Интерфейс рабочего
	/// </summary>
	public interface IWorker : IAttack
	{
		/// <summary>
		/// Городской центр, к которому привязан рабочий
		/// </summary>
		TCityCenter CityCenter { get; set; }
		/// <summary>
		/// Источник, из которого рабочий добывает ресурс
		/// </summary>
		ISource Source { get; set; }
		/// <summary>
		/// Ищет ресурсы вокруг себя
		/// </summary>
		void FindResource();
	}
}
