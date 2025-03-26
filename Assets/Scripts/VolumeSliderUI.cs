using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSliderUI : MonoBehaviour
{
	private const string MasterParameterName = "MasterVolume";
	private const float SliderMaxValue = 1;

	[SerializeField] private AudioVolume _audioVolume;
	[SerializeField] private AudioMixerGroup _mixerGroup;
	[SerializeField] private string _parameterName;

	private Slider _slider;

	private void Awake()
	{
		_slider = GetComponent<Slider>();
		_slider.minValue = 0f;
		_slider.maxValue = SliderMaxValue;

		string parameter = GetParameterName();
		float currentLinear = _audioVolume.GetVolume(parameter);
		_slider.value = currentLinear;

		_slider.onValueChanged.AddListener(OnSliderChanged);
	}

	private void OnSliderChanged(float linearValue)
	{
		_audioVolume.SetLinearVolume(GetParameterName(), linearValue);
	}

	private string GetParameterName()
	{
		if (string.IsNullOrEmpty(_parameterName) == false)
		{
			return _parameterName;
		}

		if (_mixerGroup != null)
		{
			return _mixerGroup.name + "Volume";
		}

		Debug.LogError($"Either '{nameof(_parameterName)}' or '{nameof(_mixerGroup)}' must be set.");

		return MasterParameterName;
	}

	private void OnDestroy()
	{
		_slider.onValueChanged.RemoveListener(OnSliderChanged);
	}
}