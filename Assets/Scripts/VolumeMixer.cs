using NUnit.Framework.Constraints;
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

	private bool _active = true;
	private float _savedVolume;

	public void SetLinearVolume(AudioMixerGroup _mixerGroup, float linearValue)
	{
		if (_active == false)
			return;

		string parameter = GetParameterName(_mixerGroup);
		_mixerGroup.audioMixer.SetFloat(parameter, LinearToDb(linearValue));
	}

	public void SetDbVolume(AudioMixerGroup _mixerGroup, float dbValue)
	{
		if (_active == false)
			return;

		string parameter = GetParameterName(_mixerGroup);
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
		_active = false;

		_audioMixer.GetFloat(ParameterMain, out _savedVolume);
		_audioMixer.SetFloat(ParameterMain, MinVolumeDb);
	}

	public void ActiveVolume()
	{
		_active = true;

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
