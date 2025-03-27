using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MasterVolumeToggle : MonoBehaviour
{
	[SerializeField] private AudioVolume _audioVolume;

	private Toggle _toggle;

	private void Awake()
	{
		_toggle = GetComponent<Toggle>();
	}

	private void OnEnable()
	{
		_toggle.isOn = _audioVolume.IsMasterActive;
		_toggle.onValueChanged.AddListener(OnToggleChanged);
	}

	private void OnDisable()
	{
		_toggle.onValueChanged.RemoveListener(OnToggleChanged);
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
}