using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown, displayModeDropdown, refreshRateDropdown, fpsDropdown;
    [SerializeField] Toggle fullscreenToggle, vsyncToggle;
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] TMP_Text displayText, feedbackText;
    Resolution[] resolutions;
    Coroutine messageCoroutine;

    void Start()
    {
        SetupResolutions();
        SetupDisplayModes();
        SetupRefreshRates();
        SetupFPS();
    }

    void Update()
    {
        
    }

    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (Resolution res in resolutions)
        {
            string option = res.width + " x " + res.height;

            if (!options.Contains(option))
            {
                options.Add(option);
            }
        }
        resolutionDropdown.AddOptions(options);

        int savedWidth = PlayerPrefs.GetInt("Width", Screen.currentResolution.width);
        int savedHeight = PlayerPrefs.GetInt("Height", Screen.currentResolution.height);

        for (int i = 0; i < options.Count; i++)
        {
            if (options[i] == savedWidth + " x " + savedHeight)
            {
                resolutionDropdown.value = i;
                break;
            }
        }

        resolutionDropdown.RefreshShownValue();
    }

    void SetupDisplayModes()
    {
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(new List<string>
        {
            "Fullscreen",
            "Borderless Fullscreen",
            "Windowed"
        });

        displayModeDropdown.value = PlayerPrefs.GetInt("DisplayMode", 0);
        displayModeDropdown.RefreshShownValue();
        fullscreenToggle.isOn = displayModeDropdown.value != 2;
    }

    public void ApplyDisplaySettings()
    {
        string[] size = resolutionDropdown.options[resolutionDropdown.value].text.Split('x');

        int width = int.Parse(size[0]);
        int height = int.Parse(size[1]);

        FullScreenMode mode = displayModeDropdown.value == 0? FullScreenMode.ExclusiveFullScreen: displayModeDropdown.value == 1? FullScreenMode.FullScreenWindow: FullScreenMode.Windowed;

        int hz = int.Parse(refreshRateDropdown.options[refreshRateDropdown.value].text.Replace(" Hz", ""));

        RefreshRate refreshRate = new RefreshRate
        {
            numerator = (uint)hz,
            denominator = 1
        };

        Screen.SetResolution(width, height, mode, refreshRate);

        PlayerPrefs.SetInt("Width", width);
        PlayerPrefs.SetInt("Height", height);
        PlayerPrefs.SetInt("DisplayMode", displayModeDropdown.value);
        PlayerPrefs.SetInt("RefreshRate", hz);
        PlayerPrefs.Save();
        StartCoroutine(applyFeedback());
    }

    public void ToggleFullscreen()
    {
        displayModeDropdown.value = fullscreenToggle.isOn ? 0 : 2;
        displayModeDropdown.RefreshShownValue();
        ApplyDisplaySettings();
    }

    void SetupRefreshRates()
    {
        refreshRateDropdown.ClearOptions();

        List<string> options = new List<string>();

        foreach (Resolution res in resolutions)
        {
            string rate = res.refreshRateRatio.value.ToString("0") + " Hz";

            if (!options.Contains(rate))
            {
                options.Add(rate);
            }
        }

        refreshRateDropdown.AddOptions(options);

        int savedRate = PlayerPrefs.GetInt("RefreshRate", Screen.currentResolution.refreshRate);

        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].StartsWith(savedRate.ToString()))
            {
                refreshRateDropdown.value = i;
                break;
            }
        }

        refreshRateDropdown.RefreshShownValue();
    }

    void SetupFPS()
    {
        fpsDropdown.ClearOptions();
        fpsDropdown.AddOptions(new List<string>
        {
            "30", "60", "120", "144", "240", "Unlimited"
        });

        int fps = PlayerPrefs.GetInt("FPS", 60);
        fpsDropdown.value = fps == 30 ? 0 :fps == 60 ? 1 :fps == 120 ? 2 :fps == 144 ? 3 :fps == 240 ? 4 : 5;
        fpsDropdown.RefreshShownValue();
        SetFPS();
    }

    public void SetFPS()
    {
        int fps = fpsDropdown.value == 0 ? 30 :fpsDropdown.value == 1 ? 60 :fpsDropdown.value == 2 ? 120 :fpsDropdown.value == 3 ? 144 :fpsDropdown.value == 4 ? 240 : -1;
        Application.targetFrameRate = fps;
        PlayerPrefs.SetInt("FPS", fps);
    }

    void SetupQuality()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        qualityDropdown.value = PlayerPrefs.GetInt("Quality",QualitySettings.GetQualityLevel());
        qualityDropdown.RefreshShownValue();
    }

    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
        PlayerPrefs.SetInt("Quality", qualityDropdown.value);
    }


    public void SetVSync()
    {
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSync", vsyncToggle.isOn ? 1 : 0);
    }

    void ShowMessage(string text)
    {
        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);

        messageCoroutine = StartCoroutine(Message(text));
    }

    IEnumerator Message(string text)
    {
        displayText.text = text;
        yield return new WaitForSecondsRealtime(1);
        displayText.text = "";
    }

    public void ResetSettings()
    {
        StartCoroutine(resetFeedback());
        PlayerPrefs.DeleteKey("Width");
        PlayerPrefs.DeleteKey("Height");
        PlayerPrefs.DeleteKey("DisplayMode");
        PlayerPrefs.DeleteKey("RefreshRate");
        PlayerPrefs.DeleteKey("FPS");
        PlayerPrefs.DeleteKey("Quality");
        PlayerPrefs.DeleteKey("VSync");

        PlayerPrefs.Save();

        SetupResolutions();
        SetupDisplayModes();
        SetupRefreshRates();
        SetupFPS();
        SetupQuality();
        LoadSettings();

    }

    void LoadSettings()
    {
        vsyncToggle.isOn = PlayerPrefs.GetInt("VSync", 1) == 1;
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
    }


    IEnumerator applyFeedback()
    {
        feedbackText.text = "Settings Applied!";
        yield return new WaitForSecondsRealtime(2);
        feedbackText.text = "";
    }
    IEnumerator resetFeedback()
    {
        feedbackText.text = "Settings Reset!";
        yield return new WaitForSecondsRealtime(2);
        feedbackText.text = "";
    }
}
