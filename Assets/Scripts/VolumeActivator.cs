using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class VolumeActivator : MonoBehaviour
{
	private const string SoundEnabledKey = "SoundEnabled";
	private const string SavedVolumeKey = "SavedVolume";

	private const int SoundEnabledDefaultValue = 1;
	private const int SoundDisabledValue = 0;

	private const float DbConversionBase = 10f;
	private const float DbConversionFactor = 20f;
	private const float MinLinearVolume = 0.0001f;
	private const float MaxLinearVolume = 1f;
	private const float DisableVolumeDb = -80f;
	private const float ZeroVolume = 0f;

	[Header("Sound Settings")]
	[SerializeField] private AudioMixer _audioMixer;
	[SerializeField] private string _volumeParameter = "MasterVolume";

	[Header("Volume Levels")]
	[SerializeField, Range(MinLinearVolume, MaxLinearVolume)] private float _defaultVolume = 0.75f;

	private Toggle _toggle;
	private float _lastEnabledVolume;

	private void Awake()
	{
		_toggle = GetComponent<Toggle>();

		bool isSoundOn = PlayerPrefs.GetInt(SoundEnabledKey, SoundEnabledDefaultValue) == SoundEnabledDefaultValue;
		_lastEnabledVolume = PlayerPrefs.GetFloat(SavedVolumeKey, _defaultVolume);

		_toggle.isOn = isSoundOn;
		SetVolume(isSoundOn, _lastEnabledVolume);
		_toggle.onValueChanged.AddListener(OnToggleChanged);
	}

	private void OnToggleChanged(bool isOn)
	{
		if (isOn)
		{
			SetVolume(true, _lastEnabledVolume);
		}
		else
		{
			_audioMixer.GetFloat(_volumeParameter, out float currentDb);
			_lastEnabledVolume = DbToLinear(currentDb);
			SetVolume(false, ZeroVolume);
		}

		PlayerPrefs.SetInt(SoundEnabledKey, isOn ? SoundEnabledDefaultValue : SoundDisabledValue);
		PlayerPrefs.SetFloat(SavedVolumeKey, _lastEnabledVolume);
	}

	private void SetVolume(bool isEnabled, float volume)
	{
		float targetVolume = isEnabled ? LinearToDb(volume) : DisableVolumeDb;
		_audioMixer.SetFloat(_volumeParameter, targetVolume);
	}

	private float LinearToDb(float linearValue)
	{
		return Mathf.Log10(Mathf.Max(linearValue, MinLinearVolume)) * DbConversionFactor;
	}

	private float DbToLinear(float dbValue)
	{
		return Mathf.Pow(DbConversionBase, dbValue / DbConversionFactor);
	}

	private void OnDestroy()
	{
		if (_toggle != null)
		{
			_toggle.onValueChanged.RemoveListener(OnToggleChanged);
		}
	}
}