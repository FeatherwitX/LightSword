using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SettingsLib
{
	[Serializable]
	public class Settings : ICloneable
	{
		public string Ground { get; set; }
		public int ScrollSensitive { get; set; }
		public int ResolutionWidth { get; set; }
		public int ResolutionHeight { get; set; }
		public decimal SoundVolume { get; set; }
		public decimal MusicVolume { get; set; }
		public bool SoundIsMuted { get; set; }
		public bool MusicIsMuted { get; set; }

		static BinaryFormatter bf = new BinaryFormatter();

		static string fileName = Global.SettingsFile;

		public static Settings GetSettings() {
			if (File.Exists(fileName)) {
				using (Stream input = File.OpenRead(fileName)) {
					return (Settings)bf.Deserialize(input);
				}
			}
			return new Settings();
		}

		public void Save() {
			if (File.Exists(fileName))
				File.Delete(fileName);
			using (Stream output = File.Create(fileName)) {
				bf.Serialize(output, this);
			}
		}

		public object Clone() {
			Settings result = new Settings();
			result.Ground = Ground;
			result.ScrollSensitive = ScrollSensitive;
			result.ResolutionWidth = ResolutionWidth;
			result.ResolutionHeight = ResolutionHeight;
			result.SoundVolume = SoundVolume;
			result.MusicVolume = MusicVolume;
			result.SoundIsMuted = SoundIsMuted;
			result.MusicIsMuted = MusicIsMuted;
			return result;
		}
	}
}
