using UnityEngine;

public static class VolumeSaveSystem
{
	private const int TrueValue = 1;
	private const int FalseValue = 0;

	public static bool HasSave(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void SaveFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	public static float LoadFloat(string key)
	{
		return PlayerPrefs.GetFloat(key);
	}

	public static void SaveInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	public static int LoadInt(string key)
	{
		return PlayerPrefs.GetInt(key);
	}

	public static void SaveBool(string key, bool value)
	{
		PlayerPrefs.SetInt(key, value ? TrueValue : FalseValue);
	}

	public static bool LoadBool(string key)
	{
		return PlayerPrefs.GetInt(key) == TrueValue;
	}
}
