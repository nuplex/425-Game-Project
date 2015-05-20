using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadGame{
	
	public static List<Game> savedGames = new List<Game>();
	public static Game loaded;

	public static bool pressedContinue = false;

	public static void Save(){
		savedGames.Add(Game.current);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath +"/svs.gd");
		bf.Serialize(file, savedGames);
		file.Close();
	}

	public static void Load(){
		if (File.Exists (Application.persistentDataPath + "/svs.gd")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/svs.gd", FileMode.Open);
			savedGames = (List<Game>)bf.Deserialize (file);
			file.Close ();

			loaded = savedGames.ToArray()[0];
		}
	}
	
}
