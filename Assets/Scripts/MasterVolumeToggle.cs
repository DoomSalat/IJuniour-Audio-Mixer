using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MasterVolumeToggle : MonoBehaviour
{
	private const string MasterParameterName = "MasterVolume";

	[SerializeField] private AudioVolume _audioVolume;

	private Toggle _toggle;

	private void Awake()
	{
		_toggle = GetComponent<Toggle>();
		float currentLinear = _audioVolume.GetVolume(MasterParameterName);
		_toggle.isOn = currentLinear > 0f;

		_toggle.onValueChanged.AddListener(OnToggleChanged);
	}

	private void OnToggleChanged(bool isOn)
	{
		if (isOn)
		{
			_audioVolume.ActivateMaster();
		}
		else
		{
			_audioVolume.DeactivateMaster();
		}
	}

	private void OnDestroy()
	{
		_toggle.onValueChanged.RemoveListener(OnToggleChanged);
	}
}