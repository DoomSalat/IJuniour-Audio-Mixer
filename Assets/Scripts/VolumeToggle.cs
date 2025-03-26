using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SoundToggleController : MonoBehaviour
{
	private const float OffVolumeDb = -80f;

	[SerializeField] private AudioMixerGroup _mixerGroup;
	[SerializeField] private VolumeMixer _volumeMixer;

	private Toggle _toggle;

	private void Awake()
	{
		_toggle = GetComponent<Toggle>();

		float currentDb = _volumeMixer.GetDbVolume(_mixerGroup);
		_toggle.isOn = currentDb > OffVolumeDb;

		_toggle.onValueChanged.AddListener(OnToggleChanged);
	}

	private void OnToggleChanged(bool isOn)
	{
		if (isOn)
		{
			_volumeMixer.ActiveVolume();
		}
		else
		{
			_volumeMixer.DeactiveVolume();
		}
	}

	private void OnDestroy()
	{
		_toggle.onValueChanged.RemoveListener(OnToggleChanged);
	}
}