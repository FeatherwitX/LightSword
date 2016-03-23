using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitsLib.Enums
{
	/// <summary>
	/// Цели
	/// </summary>
	public enum Aims
	{
		/// <summary>
		/// Стоять
		/// </summary>
		Stand,
		/// <summary>
		/// Идти в точку
		/// </summary>
		MoveToPoint,
		/// <summary>
		/// Идти в атаку
		/// </summary>
		Attack,
		/// <summary>
		/// Атаковать (непосредственно наносить урон)
		/// </summary>
		AttackNow,
		/// <summary>
		/// Идти собирать ресурсы
		/// </summary>
		Download,
		/// <summary>
		/// Собирать ресурсы
		/// </summary>
		DownloadNow,
		/// <summary>
		/// Идти загружать ресурсы в пункт сбора
		/// </summary>
		Upload,
		/// <summary>
		/// Загружать ресурсы в пункт сбора
		/// </summary>
		UploadNow,
		/// <summary>
		/// Искать ресурсы
		/// </summary>
		FindResorse,
	}
}
