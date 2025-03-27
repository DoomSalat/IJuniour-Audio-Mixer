using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSliderUI : MonoBehaviour
{
	private const string Suffix = "Volume";
	private const string MasterParameterName = "MasterVolume";
	private const float SliderMaxValue = 1;

	[SerializeField] private AudioVolume _audioVolume;
	[SerializeField] private AudioMixerGroup _mixerGroup;

	private Slider _slider;

	private string ParameterName
	{
		get
		{
			if (_mixerGroup != null)
			{
				return _mixerGroup.name + Suffix;
			}

			Debug.LogError($"{nameof(_mixerGroup)} must be set.");

			return MasterParameterName;
		}
	}

	private void Awake()
	{
		_slider = GetComponent<Slider>();
		_slider.minValue = 0f;
		_slider.maxValue = SliderMaxValue;

		_audioVolume.LoadLocalVolumeSettings(ParameterName);
	}

	private void Start()
	{
		OnSliderChanged(_slider.value);
	}

	private void OnEnable()
	{
		_slider.value = _audioVolume.GetLinearVolume(ParameterName);
		_slider.onValueChanged.AddListener(OnSliderChanged);
	}

	private void OnDisable()
	{
		_slider.onValueChanged.RemoveListener(OnSliderChanged);
	}

	private void OnSliderChanged(float linearValue)
	{
		_audioVolume.SetLinearVolume(ParameterName, linearValue);
	}
}