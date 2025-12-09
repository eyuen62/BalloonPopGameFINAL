using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;  // NEW: Reference to mute toggle
    public TextMeshProUGUI volumeValueText; // Optional

    void Start()
    {
        // Load saved volume or use default 0.5
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        // Load mute state (0 = false/not muted, 1 = true/muted)
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        muteToggle.isOn = isMuted;
        ApplyMuteState(isMuted);

        // Update display if using text
        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(savedVolume * 100) + "%";

        // Listen for changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteChanged);  // NEW
    }

    void OnVolumeChanged(float value)
    {
        // If muted, don't change actual volume (but still save the value)
        if (!muteToggle.isOn)
        {
            AudioListener.volume = value;
        }

        // Save volume
        PlayerPrefs.SetFloat("Volume", value);

        // Update display if using text
        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(value * 100) + "%";

        PlayerPrefs.Save();
    }

    // NEW: Handle mute toggle
    void OnMuteChanged(bool isMuted)
    {
        ApplyMuteState(isMuted);
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // NEW: Apply mute state to audio
    void ApplyMuteState(bool isMuted)
    {
        if (isMuted)
        {
            AudioListener.volume = 0f;  // Mute
        }
        else
        {
            // Restore to saved volume
            AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}