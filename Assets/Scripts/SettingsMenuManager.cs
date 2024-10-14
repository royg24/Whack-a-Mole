using UnityEngine;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class SettingsMenuManager : Singleton<SettingsMenuManager>
{
    public static SettingsMenuManager SettingsMenuManagerInstance { get; private set; }

    [SerializeField] private Image settingsMenu;
    [SerializeField] private Slider[] settingsSliders;
    [SerializeField] private Toggle[] settingsToggles;
    [SerializeField] private Button approveButton;
    public bool SoundState { get; private set; } = true;

    public void Awake()
    {
        if (SettingsMenuManagerInstance == null)
        {
            SettingsMenuManagerInstance = Singleton<SettingsMenuManager>.Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        ChangeMenuVisibility(false);
        approveButton.onClick.AddListener(ApproveSettings);
        InitSliders();
        InitToggles();
    }

    private void ChangeMenuVisibility(bool value)
    {
         settingsMenu.gameObject.SetActive(value);
         approveButton.gameObject.SetActive(value);

         foreach (var slider in settingsSliders)
         {
            slider.gameObject.SetActive(value); 
         }

         foreach (var toggle in settingsToggles)
         {
             toggle.gameObject.SetActive(value); 
         }
    }


    private void InitSliders()
    {
        foreach (var slider in settingsSliders)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener((value =>
                OnSliderValueChanged(slider, value)));
            InitSliderValue(slider);
        }
    }

    private void InitSliderValue(Slider slider)
    {
        switch (slider.tag)
        {
            case "Music":
                slider.value = GameSettings.GameSettingsInstance.MusicAudioSource.volume;
                break;
            case "Sound":
                slider.value = GameSettings.GameSettingsInstance.SoundAudioSource.volume;
                break;
            default:
                return;
        }
    }

    private void InitToggles()
    {
        foreach (var toggle in settingsToggles)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((isOn =>
                OnToggleValueChanged(toggle, isOn)));
        }
    }

    private void OnSliderValueChanged(Slider slider, float volumeValue)
    {
        AudioSource audioSource;

        switch (slider.tag)
        {
            case "Sound":
                audioSource = GameSettings.GameSettingsInstance.SoundAudioSource;
                break;
            case "Music":
                audioSource = GameSettings.GameSettingsInstance.MusicAudioSource;
                break;
            default:
                return;
        }

        audioSource.volume = volumeValue;
        GameSettings.GameSettingsInstance.SaveSlidersValues();
    }  

    private void OnToggleValueChanged(Toggle toggle, bool isOn)
    {
        var gameAudioSource = GameSettings.GameSettingsInstance.MusicAudioSource;

        switch (toggle.tag)
        {
            case "Sound":
                SoundState = isOn;
                break;
            case "Music":
                if (isOn)
                    gameAudioSource.Play();
                else
                    gameAudioSource.Stop();
                break;
            default:
                return;
        }

    }

    public void ApproveSettings()
    {
        SwitchMenus(false);
    }

    public void OpenSettingsMenu()
    {
        SwitchMenus(true);
    }

    private void SwitchMenus(bool value)
    {
        ChangeMenuVisibility(value);
        if(GameManager.GameManagerInstance.Playing)
            UIManager.UIManagerInstance.ChangePauseMenuItemsVisibility(!value);
        else
        {
            UIManager.UIManagerInstance.ChangeStartUIVisibility(!value);
            UIManager.UIManagerInstance.ChangeUpperButtonsActivation(!value);
        }
    }
}
