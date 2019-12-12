using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettingsMenu : MonoBehaviour {

    public AudioMixer masterMixer;

    public Slider mainVolumeSlider, fxVolumeSlider, musicVolumeSLider;
    public Toggle muteToggle;

    private bool isMuted;

    // Use this for initialization
    void Start () {
        LoadMenuVariables();
	}

    public void ToggleMute(bool toggleValue)
    {
        isMuted = toggleValue;
        if (isMuted)
            masterMixer.SetFloat("mainVolume", -80);
        else
            masterMixer.SetFloat("mainVolume", Mathf.Log(mainVolumeSlider.value) * 20);
    }

    public void SetMainVolume(float sliderValue)
    {
        if (!isMuted)
            masterMixer.SetFloat("mainVolume", Mathf.Log(sliderValue) * 20);
    }

    public void SetFxVolume(float sliderValue)
    {
        masterMixer.SetFloat("fxVolume", Mathf.Log(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        masterMixer.SetFloat("musicVolume", Mathf.Log(sliderValue) * 20);
    }

    public void SaveMenuVariables()
    {
        PlayerPrefs.SetInt("audioPrefsSaved", 0);

        PlayerPrefs.SetInt("mutedI", muteToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("mainVolumeF", mainVolumeSlider.value);
        PlayerPrefs.SetFloat("fxVolumeF", fxVolumeSlider.value);
        PlayerPrefs.SetFloat("musicVolumeF", musicVolumeSLider.value);
    }

    public void LoadMenuVariables()
    {
        if (PlayerPrefs.HasKey("audioPrefsSaved"))
        {
            mainVolumeSlider.value  = PlayerPrefs.GetFloat("mainVolumeF");
            fxVolumeSlider.value    = PlayerPrefs.GetFloat("fxVolumeF");
            musicVolumeSLider.value = PlayerPrefs.GetFloat("musicVolumeF");

            if (PlayerPrefs.GetInt("mutedI") == 1)
                muteToggle.isOn = true;
            else muteToggle.isOn = false;
        }
    }
}
