using UnityEngine;
using System.Collections.Generic;

[System.Serializable] public class ThemeShopData
{
	public List<int> purchasedItemIndexes = new List<int>();
}

[System.Serializable] public class PlayerData
{
	public int selectedThemeIndex = 0;
}
public static class GameDataManager
{
	static PlayerData playerData = new PlayerData();
	static ThemeShopData themeShopData = new ThemeShopData();

	static Theme selectedTheme;
	static GameDataManager()
	{
		LoadPlayerData();
		LoadThemeShopData();
	}


	public static Theme GetSelectedTheme()
	{
		return selectedTheme;
	}

	public static void SetSelectedTheme(Theme theme, int index)
	{
		selectedTheme = theme;
		playerData.selectedThemeIndex = index;
		SavePlayerData();
	}

	public static int GetSelectedThemeIndex()
	{
		return playerData.selectedThemeIndex;
	}

	public static void SetSelectedThemeIndex(int newIndex)
	{
		playerData.selectedThemeIndex = newIndex;
		SavePlayerData();
	}

	static void LoadPlayerData()
	{
		playerData = BinarySerializer.Load<PlayerData>("player-data.txt");
		Debug.Log("<color=green>[PlayerData] loaded</color>");
	}

	static void SavePlayerData()
	{
		BinarySerializer.Save<PlayerData>(playerData, "player-data.txt");
		Debug.Log("<color=magenta>[PlayerData] saved</color>");
	}

	public static bool CanPurchaseTheme(int price)
	{
		if (PlayerPrefs.GetInt("highScore") >= price)
			return true;
		return false;
	}
	public static void AddPurchasedTheme(int index)
	{
		themeShopData.purchasedItemIndexes.Add(index);
		SaveThemeShopData();
	}

	public static List<int> GetAllPurchasedThemes()
	{
		return themeShopData.purchasedItemIndexes;
	}

	public static int GetPurchasedTheme(int index)
	{
		return themeShopData.purchasedItemIndexes[index];
	}
	static void LoadThemeShopData()
	{
		themeShopData = BinarySerializer.Load<ThemeShopData>("theme-shop-data.txt");
		Debug.Log("<color=green>[ThemeShopData] loaded</color>");
	}

	static void SaveThemeShopData()
	{
		BinarySerializer.Save<ThemeShopData>(themeShopData, "theme-shop-data.txt");
		Debug.Log("<color=magenta>[ThemeShopData] saved</color>");
	}
}
