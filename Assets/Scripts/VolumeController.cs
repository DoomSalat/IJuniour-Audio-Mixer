using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
	private const float MinVolumeDb = -80f;
	private const float MaxVolumeDb = 0f;
	private const float DbConversionFactor = 20f;

	private const float DefaultVolumeLinear = 0.75f;
	private const float MinLinearThreshold = 0.0001f;

	[SerializeField] private AudioMixer _audioMixer;
	[SerializeField] private string _volumeParameter = "MasterVolume";
	[SerializeField] private Slider _volumeSlider;

	private void Awake()
	{
		float savedVolume = PlayerPrefs.GetFloat(_volumeParameter, DefaultVolumeLinear);

		_volumeSlider.value = savedVolume;
		SetVolume(savedVolume);

		_volumeSlider.onValueChanged.AddListener(SetVolume);
	}

	public void SetVolume(float linearValue)
	{
		if (linearValue <= MinLinearThreshold)
		{
			_audioMixer.SetFloat(_volumeParameter, MinVolumeDb);
			PlayerPrefs.SetFloat(_volumeParameter, MaxVolumeDb);

			return;
		}

		float volumeDb = Mathf.Log10(linearValue) * DbConversionFactor;
		_audioMixer.SetFloat(_volumeParameter, volumeDb);

		PlayerPrefs.SetFloat(_volumeParameter, linearValue);
	}

	private void OnDestroy()
	{
		if (_volumeSlider != null)
		{
			_volumeSlider.onValueChanged.RemoveListener(SetVolume);
		}
	}
}