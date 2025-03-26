using UnityEngine;
using UnityEngine.Audio;

public class VolumeMixer : MonoBehaviour
{
	private const string Prefix = "Volume";
	private const string ParameterMain = "MasterVolume";
	private const float DbConversionFactor = 20f;

	private const float MinLinearVolume = 0.0001f;
	private const float MinVolumeDb = -80f;

	[SerializeField] private AudioMixer _audioMixer;

	private bool _isActive = true;
	private float _savedVolume;

	public void SetLinearVolume(AudioMixerGroup _mixerGroup, float linearValue)
	{
		string parameter = GetParameterName(_mixerGroup);

		if (_isActive == false && parameter == ParameterMain)
			return;

		_mixerGroup.audioMixer.SetFloat(parameter, LinearToDb(linearValue));
	}

	public void SetDbVolume(AudioMixerGroup _mixerGroup, float dbValue)
	{
		string parameter = GetParameterName(_mixerGroup);

		if (_isActive == false && parameter == ParameterMain)
			return;

		_mixerGroup.audioMixer.SetFloat(parameter, LinearToDb(dbValue));
	}

	public float GetDbVolume(AudioMixerGroup _mixerGroup)
	{
		string parameter = GetParameterName(_mixerGroup);
		_mixerGroup.audioMixer.GetFloat(parameter, out float volumeParameter);

		return volumeParameter;
	}

	public void DeactiveVolume()
	{
		_isActive = false;

		_audioMixer.GetFloat(ParameterMain, out _savedVolume);
		_audioMixer.SetFloat(ParameterMain, MinVolumeDb);
	}

	public void ActiveVolume()
	{
		_isActive = true;

		_audioMixer.SetFloat(ParameterMain, _savedVolume);
	}

	private float LinearToDb(float linearValue)
	{
		return Mathf.Log10(Mathf.Max(linearValue, MinLinearVolume)) * DbConversionFactor;
	}

	private string GetParameterName(AudioMixerGroup _mixerGroup)
	{
		return _mixerGroup.name + Prefix;
	}
}
