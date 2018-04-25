using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine;

public static class SaveManager {
	public static SaveFile activeSave;
	public static List<SaveFile> savedGames = new List<SaveFile>();

	public static void Save() {
		savedGames.Add(SaveGameState());
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, SaveManager.savedGames);
		file.Close();
	}

	public static void Load() {
		if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			SaveManager.savedGames = (List<SaveFile>)bf.Deserialize(file);
			file.Close();
		}
	}

	static void SetGameState(SaveFile save) {
		Inventory.instance.PopulateInventory (save.inventory);
		Player.instance.health = save.currentHealth;
	}

	static SaveFile SaveGameState() {
		SaveFile file = new SaveFile ();
		file.inventory = Inventory.instance.GetInventoryItems ();
		file.currentHealth = Player.instance.health;

		return file;
	}
}

[System.Serializable]
public class SaveFile {
	public Item[] inventory;
	public Item[] hotbar;
	public int currentHealth;
}
