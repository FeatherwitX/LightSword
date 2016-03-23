using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitsLib.Enums;
using UnitsLib.Surround;

namespace SettingsLib
{
	public class AnalysedSettings
	{
		public string Ground { get; set; }
		public int SceneVelocity { get; set; }
		public int ResolutionWidth { get; set; }
		public int ResolutionHeight { get; set; }
		public float SoundVolume { get; set; }
		public float MusicVolume { get; set; }
		public bool SoundIsMuted { get; set; }
		public bool MusicIsMuted { get; set; }

		public AnalysedSettings(Settings settings) {
			SoundVolume = (float)settings.SoundVolume / 100f;
			MusicVolume = (float)settings.MusicVolume / 100f;
			SoundIsMuted = settings.SoundIsMuted;
			MusicIsMuted = settings.MusicIsMuted;
			switch (settings.Ground) {
				case "Dust":
					Ground = Fnames.Dust;
					break;
				case "Grass":
					Ground = Fnames.Grass;
					break;
				case "Snow":
					Ground = Fnames.Snow;
					break;
				default:
					Ground = Fnames.Snow;
					break;
			}

			if (settings.ScrollSensitive <= 0)
				SceneVelocity = 15;
			else if (settings.ScrollSensitive > 100)
				SceneVelocity = 100;
			else SceneVelocity = settings.ScrollSensitive;

			if (settings.ResolutionWidth <= 100)
				ResolutionWidth = 100;
			else if (settings.ResolutionWidth >= 2000)
				ResolutionWidth = 2000;
			else
				ResolutionWidth = settings.ResolutionWidth;

			if (settings.ResolutionHeight <= 100)
				ResolutionHeight = 100;
			else if (settings.ResolutionHeight >= 2000)
				ResolutionHeight = 2000;
			else
				ResolutionHeight = settings.ResolutionHeight;
		}
	}
}
