using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnitsLib;
using UnitsLib.Surround;

namespace SettingsLib
{
	public static class Loader
	{
		static BinaryFormatter bf = new BinaryFormatter();

		public static void Load() {
			string filePath = Global.SaveFile;
			if (!File.Exists(filePath))
				throw new IOException("Файл " + filePath + " не найден!");
			using (Stream output = File.OpenRead(filePath)) {
				World.TObjects.Clear();
				World.TObjects = LoadListOfTObject(output);
			}
		}

		private static List<TObject> LoadListOfTObject(Stream output) {
			List<TObject> result = new List<TObject>();
			//List<TObjectSaver> loadList = (List<TObjectSaver>)bf.Deserialize(output);
			return result;
		}
	}
}
