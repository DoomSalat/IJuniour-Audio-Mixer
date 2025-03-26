using UnityEngine;
using UnityEngine.Audio;

public class AudioVolume : MonoBehaviour
{
	private const float DbConversionFactor = 20f;
	private const float MinVolumeDb = -80f;
	private const string MasterParameterName = "MasterVolume";
	private const float LinearBase = 10f;

	[SerializeField] private AudioMixer _audioMixer;

	private bool _isMasterActive = true;
	private float _savedMasterVolume;

	public void SetLinearVolume(string parameterName, float linearValue)
	{
		if (parameterName == MasterParameterName && _isMasterActive == false)
		{
			_savedMasterVolume = LinearToDb(linearValue);

			return;
		}

		float dbValue = LinearToDb(linearValue);

		if (_audioMixer.SetFloat(parameterName, dbValue) == false)
		{
			Debug.LogError("Parameter " + parameterName + " not found in AudioMixer");
		}
	}

	public void SetDbVolume(string parameterName, float dbValue)
	{
		if (parameterName == MasterParameterName && _isMasterActive == false)
		{
			_savedMasterVolume = dbValue;

			return;
		}

		if (_audioMixer.SetFloat(parameterName, dbValue) == false)
		{
			Debug.LogError("Parameter " + parameterName + " not found in AudioMixer");
		}
	}

	public float GetVolume(string parameterName)
	{
		if (parameterName == MasterParameterName && _isMasterActive == false)
		{
			return DbToLinear(_savedMasterVolume);
		}

		float dbValue;

		if (_audioMixer.GetFloat(parameterName, out dbValue))
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

		SetDbVolume(MasterParameterName, MinVolumeDb);
		_isMasterActive = false;
	}

	public void ActivateMaster()
	{
		if (_isMasterActive)
			return;

		_isMasterActive = true;
		SetDbVolume(MasterParameterName, _savedMasterVolume);
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