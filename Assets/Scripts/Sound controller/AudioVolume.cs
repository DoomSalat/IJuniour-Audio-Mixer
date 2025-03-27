using UnityEngine;
using UnityEngine.Audio;

public class AudioVolume : MonoBehaviour
{
	private const string SaveSuffix = "Pref";
	private const string MasterVolumeSaveKey = "MasterVolumePref";
	private const string MasterActiveSaveKey = "MasterActivePref";
	private const int TrueValue = 1;
	private const int FalseValue = 0;

	private const string MasterParameterName = "MasterVolume";
	private const float DbConversionFactor = 20f;
	private const float MinVolumeDb = -80f;
	private const float LinearBase = 10f;

	[SerializeField] private AudioMixer _audioMixer;

	private bool _isMasterActive = true;
	private float _savedMasterVolume;

	public bool IsMasterActive => _isMasterActive;

	private void Awake()
	{
		LoadMasterVolumeSettings();
	}

	private void Start()
	{
		if (_isMasterActive == false)
		{
			_audioMixer.SetFloat(MasterParameterName, MinVolumeDb);
		}
	}

	public void SetLinearVolume(string parameterName, float linearValue)
	{
		SetDbVolume(parameterName, LinearToDb(linearValue));
	}

	public void SetDbVolume(string parameterName, float dbValue)
	{
		if (parameterName == MasterParameterName && _isMasterActive == false)
		{
			_savedMasterVolume = dbValue;
			PlayerPrefs.SetFloat(MasterVolumeSaveKey, DbToLinear(_savedMasterVolume));

			return;
		}

		if (_audioMixer.SetFloat(parameterName, dbValue) == false)
		{
			Debug.LogError("Parameter " + parameterName + " not found in AudioMixer");
		}
		else
		{
			PlayerPrefs.SetFloat(parameterName + SaveSuffix, DbToLinear(dbValue));
		}
	}

	public float GetLinearVolume(string parameterName)
	{
		if (parameterName == MasterParameterName && _isMasterActive == false)
		{
			return DbToLinear(_savedMasterVolume);
		}

		if (_audioMixer.GetFloat(parameterName, out float dbValue))
		{
			return DbToLinear(dbValue);
		}
		else
		{
			Debug.LogError("Parameter " + parameterName + " not found in AudioMixer");

			return 0f;
		}
	}

	public void DeactivateMaster()
	{
		if (_isMasterActive == false)
			return;

		_audioMixer.GetFloat(MasterParameterName, out float currentDbValue);
		_savedMasterVolume = currentDbValue;

		_audioMixer.SetFloat(MasterParameterName, MinVolumeDb);
		_isMasterActive = false;

		PlayerPrefs.SetInt(MasterActiveSaveKey, FalseValue);
		PlayerPrefs.SetFloat(MasterActiveSaveKey, DbToLinear(_savedMasterVolume));
	}

	public void ActivateMaster()
	{
		if (_isMasterActive)
			return;

		_isMasterActive = true;
		SetDbVolume(MasterParameterName, _savedMasterVolume);

		PlayerPrefs.SetInt(MasterActiveSaveKey, TrueValue);
	}

	public void LoadLocalVolumeSettings(string parameterName)
	{
		string savedParameter = parameterName + SaveSuffix;

		if (PlayerPrefs.HasKey(savedParameter))
		{
			float savedVolume = PlayerPrefs.GetFloat(savedParameter);
			SetLinearVolume(parameterName, savedVolume);
		}
	}

	private void LoadMasterVolumeSettings()
	{
		int isActive = PlayerPrefs.GetInt(MasterActiveSaveKey);
		_isMasterActive = isActive == TrueValue;

		_savedMasterVolume = LinearToDb(PlayerPrefs.GetFloat(MasterVolumeSaveKey));
	}

	private float LinearToDb(float linear)
	{
		if (linear <= 0)
		{
			return MinVolumeDb;
		}
		else
		{
			return DbConversionFactor * Mathf.Log10(linear);
		}
	}

	private float DbToLinear(float db)
	{
		if (db <= MinVolumeDb)
		{
			return 0f;
		}
		else
		{
			return Mathf.Pow(LinearBase, db / DbConversionFactor);
		}
	}
}