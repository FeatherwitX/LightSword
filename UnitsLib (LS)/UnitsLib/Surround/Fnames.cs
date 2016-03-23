using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitsLib.Surround
{
	/// <summary>
	/// Статичный класс, содержащий пути к ресурсам игры
	/// </summary>
	public static class Fnames
	{
		#region Textures

		public const string Flag = @"Textures\flag";
		public const string Hp = @"Textures\hp";
		public const string HpBorder = @"Textures\hpBorder";
		public const string Cursor = @"Textures\Sword";
		public const string Mesh = @"Textures\mesh";

		public const string Enemy = @"Textures\Man\Enemy";
		public const string Man = @"Textures\Man\Man";
		public const string Worker = @"Textures\Man\Worker";
		public const string Peasant = @"Textures\Man\Peasant";
		public const string Swordman = @"Textures\Man\Swordman";

		public const string BigTree = @"Textures\Sources\BigTree";
		public const string SmallTree = @"Textures\Sources\SmallTree";
		public const string Stone = @"Textures\Sources\Stone";

		public const string CityCenter = @"Textures\Buildings\CityCenter";
		public const string Farm = @"Textures\Buildings\Farm";
		public const string Baracks = @"Textures\Buildings\Baracks";

		public const string Grass = @"Textures\Ground\Grass";
		public const string DarkGrass = @"Textures\Ground\DarkGrass";
		public const string Snow = @"Textures\Ground\Snow";
		public const string Dust = @"Textures\Ground\Dust";

		#endregion

		#region Fonts

		public const string Arial14 = @"Fonts\arial-14";
		public const string BigFont = @"Fonts\BigFont";
		public const string ButtonFont = @"Fonts\ButtonFont";
		public const string MSReferenceSansSerif14B = @"Fonts\MS Reference Sans Serif-14-B";
		public const string obj = @"Fonts\obj";
		public const string OldEnglishTextMT = @"Fonts\Old English Text MT";
		public const string PanelFont = @"Fonts\PanelFont";
		public const string SavePanelFont = @"Fonts\SavePanelFont";

		#endregion

		#region UI

		public const string HUD = @"UI\HUD";
		public const string MinimapSceneRect = @"UI\minimapSceneRect";

		public const string HpPrev = @"UI\hpPrev";
		public const string HpBorderPrev = @"UI\hpBorderPrev";
		public const string ProgressPrev = @"UI\progressPrev";
		public const string ProgressBorderPrev = @"UI\progressBorderPrev";

		public const string ManPrev = @"UI\Prev\ManPrev";
		public const string SwordmanPrev = @"UI\Prev\SwordmanPrev";
		public const string TreePrev = @"UI\Prev\TreePrev";
		public const string StonePrev = @"UI\Prev\StonePrev";
		public const string CityCenterPrev = @"UI\Prev\CityCenterPrev";
		public const string FarmPrev = @"UI\Prev\FarmPrev";
		public const string BaracksPrev = @"UI\Prev\BaracksPrev";

		public const string WorkerIcon = @"UI\UIPanel\UnitIcons\Worker";
		public const string PeasantIcon = @"UI\UIPanel\UnitIcons\Peasant";
		public const string SwordmanIcon = @"UI\UIPanel\UnitIcons\Swordman";

		public const string FarmIcon = @"UI\UIPanel\BuildingIcons\BuildFarm";
		public const string BaracksIcon = @"UI\UIPanel\BuildingIcons\BuildBaracks";

		public const string DeadIcon = @"UI\UIPanel\CommandIcons\Dead";
		public const string StopIcon = @"UI\UIPanel\CommandIcons\Stop";
		public const string ClearIcon = @"UI\UIPanel\CommandIcons\Clear";

		#endregion

		#region MainMenu

		public const string BackgroundSword = @"UI\MainMenu\Background\BackgroundSword";
		public const string BackgroundKnight = @"UI\MainMenu\Background\BackgroundKnight";

		public const string BigButtonBlue = @"UI\MainMenu\Buttons\BigButtonBlue";
		public const string SmallButtonBlue = @"UI\MainMenu\Buttons\SmallButtonBlue";
		public const string ApplyB = @"UI\MainMenu\Buttons\Apply";
		public const string CancelB = @"UI\MainMenu\Buttons\Cancel";
		public const string ContinueB = @"UI\MainMenu\Buttons\Continue";
		public const string CreditsB = @"UI\MainMenu\Buttons\Credits";
		public const string ExitGlobalB = @"UI\MainMenu\Buttons\ExitGlobal";
		public const string ExitLocalB = @"UI\MainMenu\Buttons\ExitLocal";
		public const string LoadB = @"UI\MainMenu\Buttons\Load";
		public const string NewB = @"UI\MainMenu\Buttons\New";
		public const string OptionsB = @"UI\MainMenu\Buttons\Options";
		public const string SaveB = @"UI\MainMenu\Buttons\Save";

		public const string Checkbox1 = @"UI\MainMenu\Checkboxes\Checkbox1";

		public const string Slider1 = @"UI\MainMenu\Sliders\Slider1";
		public const string Slider_ = @"UI\MainMenu\Sliders\Slider_";

		public const string ListBox = @"UI\MainMenu\ListBoxes\ListBox";

		public const string Panel = @"UI\MainMenu\Panel\Panel";

		#endregion

		#region Music

		public const string DeathSong = @"Sound\Music\DeathSong";
		public const string EpicScore = @"Sound\Music\EpicScore";
		public const string FearNotThisNight = @"Sound\Music\FearNotThisNight";
		public const string WindGuideYou = @"Sound\Music\WindGuideYou";
		public const string Stronghold = @"Sound\Music\Stronghold";
		public const string HeritageOfKings = @"Sound\Music\HeritageOfKings";
		public const string Elfish1 = @"Sound\Music\Elfish1";
		public const string Elfish2 = @"Sound\Music\Elfish2";
		public const string Elfish3 = @"Sound\Music\Elfish3";

		#endregion

		#region Sound

		public const string UIButtonClick = @"Sound\Sounds\UI\ButtonClick";
		public const string UIButtonSelect = @"Sound\Sounds\UI\ButtonSelection";

		#endregion
	}
}
