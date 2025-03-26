using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
	private const float MinLinearVolume = 0.0001f;
	private const float MaxLinearVolume = 1f;

	private const float MinVolumeDb = -80f;
	private const float MaxVolumeDb = 0f;

	[SerializeField] private AudioMixerGroup _mixerGroup;
	[SerializeField] private VolumeMixer _volumeMixer;

	private Slider _slider;

	private void Awake()
	{
		_slider = GetComponent<Slider>();
		_slider.minValue = MinLinearVolume;
		_slider.maxValue = MaxLinearVolume;

		float currentDb = _volumeMixer.GetDbVolume(_mixerGroup);
		_slider.value = DbToLinear(currentDb);

		_slider.onValueChanged.AddListener(OnSliderChanged);
	}

	private void OnSliderChanged(float value)
	{
		_volumeMixer.SetLinearVolume(_mixerGroup, value);
	}

	private float DbToLinear(float db)
	{
		return Mathf.InverseLerp(MinVolumeDb, MaxVolumeDb, db);
	}

	private void OnDestroy()
	{
		_slider.onValueChanged.RemoveListener(OnSliderChanged);
	}
}