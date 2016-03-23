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
	public static class Saver
	{
		static BinaryFormatter bf = new BinaryFormatter();

		public static void Save() {
			string filePath = Global.SaveFile;
			if (File.Exists(filePath))
				File.Delete(filePath);
			using (Stream output = File.OpenWrite(filePath)) {
				SaveListOfTStone(World.TStones, output);
			}
		}

		private static void SaveListOfTStone(List<TStone> list, Stream output) {
			List<TStoneSaver> saveList = new List<TStoneSaver>();
			bf.Serialize(output, saveList);
		}
	}
}
