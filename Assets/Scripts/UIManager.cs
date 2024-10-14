using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Enums;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class UIManager : Singleton<UIManager>
{
    public static UIManager UIManagerInstance { get; private set; }
    private readonly Vector3 _resumeButtonPausePosition = new Vector3(27f, -74.57f, 0f);
    private readonly Vector3 _resumeButtonInfoPosition = new Vector3(349f, -90f, 0f);

    [Header("Canvas")]
    [SerializeField] private Canvas mainCanvas;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button smallMenuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button smallSettingsButton;
    [SerializeField] private Button informationButton;
    [SerializeField] private Button pauseButton;

    [Header("Backgrounds")] 
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Texture2D startBackgroundImage;
    [SerializeField] private Texture2D gameBackgroundImage;
    [SerializeField] private Image gameInformationImage;

    [Header("Toggles")]
    [SerializeField] private Toggle[] difficultyToggles;
    [SerializeField] private TextMeshProUGUI difficultyHeader;

    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI scoreHeader;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] public  TextMeshProUGUI scoreAddingTextPrefab;

    [Header("Time Texts")]
    [SerializeField] private TextMeshProUGUI timeHeader;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("High Score Texts")]
    [SerializeField] private TextMeshProUGUI highScoreHeader;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Pause Texts")] 
    [SerializeField] private TextMeshProUGUI pauseText;

    [Header("End Game Texts")]
    [SerializeField] private TextMeshProUGUI endScoreHeader;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private TextMeshProUGUI newHighScoreText;
    [SerializeField] private GameObject gameOverText;

    private void Awake()
    {
        if (UIManagerInstance == null)
        {
            UIManagerInstance = Singleton<UIManager>.Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        smallSettingsButton.onClick.AddListener(SettingsMenuManager.SettingsMenuManagerInstance.OpenSettingsMenu);
        settingsButton.onClick.AddListener(SettingsMenuManager.SettingsMenuManagerInstance.OpenSettingsMenu);
        StartUI();
        InitToggles();
    }

    private void InitToggles()
    {
        for(var i = 0; i < difficultyToggles.Length; i++)
        {
            var currentToggle = difficultyToggles[i];

            currentToggle.onValueChanged.RemoveAllListeners();
            currentToggle.onValueChanged.AddListener((isOn) => 
                OnToggleValueChanged(currentToggle, isOn));

            difficultyToggles[i].isOn = i == GameSettings.InitDifficulty;
        }
    }


    public void StartUI()
    {
        ChangeStartUIVisibility(true);
        ChangeHighScoreVisibility(true);
        restartButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        ChangeGameUIVisibility(false);
        ChangeEndUIVisibility(false);
        ChangePauseUIVisibility(false);
        ShowGameInformation(false);
    }

    public void ShowGameInformation(bool value)
    {
        gameInformationImage.gameObject.SetActive(value);
        resumeButton.gameObject.SetActive(value);
        resumeButton.transform.localPosition = _resumeButtonInfoPosition;
        ChangeUpperButtonsActivation(!value);
    }

    // Appears only in start menu
    public void ChangeStartUIVisibility(bool value)
    { 
        startButton.gameObject.SetActive(value);
        settingsButton.gameObject.SetActive(value);
        foreach (var toggle in difficultyToggles)
        {
             toggle.gameObject.SetActive(value);
        }
    }

    // Appears only during the game
    private void ChangeGameUIVisibility(bool value)
    {
        scoreHeader.gameObject.SetActive(value);
        scoreText.gameObject.SetActive(value);
        timeHeader.gameObject.SetActive(value);
        timeText.gameObject.SetActive(value);
        difficultyHeader.gameObject.SetActive(value);
        pauseButton.gameObject.SetActive(value);
    }

    // Appears in both start and end
    private void ChangeHighScoreVisibility(bool value)
    {
        highScoreText.gameObject.SetActive(value);
        highScoreHeader.gameObject.SetActive(value);
    }

    // UI changed when game is playing (or not)
    public void SwitchGameModesUI(bool playing)
    {
        ChangeGameUIVisibility(playing);
        ChangeHighScoreUI(playing);
        GameManager.GameManagerInstance.WaitDelay(GameSettings.EndDelayDuration);
        ChangeEndUIVisibility(!playing);
        GameManager.GameManagerInstance.WaitDelay(GameSettings.EndDelayDuration);
    }

    // Appears only in end menu
    private void ChangeEndUIVisibility(bool value)
    {
        restartButton.gameObject.SetActive(value);
        menuButton.gameObject.SetActive(value);
        gameOverText.SetActive(value);
        endScoreHeader.gameObject.SetActive(value);
        endScoreText.gameObject.SetActive(value);
        newHighScoreText.gameObject.SetActive(value);
        informationButton.gameObject.SetActive(!value);
    }

    public void ChangeStartMenuVisibility(bool value)
    {
        ChangeStartUIVisibility(value);
        ChangeHighScoreVisibility(value);
    }

    public void RestartUI(int highScore, float startingTime)
    {
        SwitchGameModesUI(true);
        highScoreText.text = highScore.ToString();
        UpdateTime(startingTime);
    }

    public void UpdateTime(float timeToUpdate)
    {
        timeText.text = Mathf.RoundToInt(timeToUpdate).ToString();
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString(); 
    }

    public void UpdateHighScoreText(int highScore)
    {
        highScoreText.text = highScore.ToString();
    }

    public void UpdateEndScoreText(int score, bool newHighScore)
    {
        endScoreText.text = score.ToString();
        if (newHighScore) 
            GameSettings.GameSettingsInstance.PlayHighScoreSound();
        newHighScoreText.gameObject.SetActive(newHighScore);
    }

    private void ChangeHighScoreUI(bool appear)
    {
        highScoreHeader.gameObject.SetActive(appear);
         highScoreText.gameObject.SetActive(appear);
    }

    private void ChangeBackground(Texture2D background)
    {
        backgroundImage.sprite = Sprite.Create(background,
            new Rect(0, 0, background.width, background.height), 
            new Vector2(0.5f, 0.5f));
    }

    public void ChangeBackgroundToGame()
    {
           ChangeBackground(gameBackgroundImage);
    }

    public void ChangeBackgroundToStart()
    {
        ChangeBackground(startBackgroundImage);
    }

    private void OnToggleValueChanged(Toggle clickedToggle, bool isOn)
    {
        if (isOn)
        {
            foreach (var toggle in difficultyToggles)
            {
                toggle.isOn = toggle == clickedToggle;
                toggle.interactable = !toggle.isOn;

                if (toggle.isOn)
                {
                    // Change high score according to the difficulty
                    var difficultyText = toggle.GetComponentInChildren<Text>().text;
                    UpdateHighScoreText(PlayerPrefs.GetInt(difficultyText + GameSettings.HighScoreData, 0));
                }
            }
        }
    }

    public Toggle[] GetToggles()
    {
        return difficultyToggles;
    }

    public Canvas GetCanvas()
    {
        return mainCanvas;
    }

    public void SetDifficultyHeader(EDifficulty difficulty)
    {
        difficultyHeader.text = difficulty.ToString();
    }

    public void ChangePauseUIVisibility(bool value)
    {
        ChangePauseMenuItemsVisibility(value);
        ChangeUpperButtonsActivation(!value);
    }

    public void ChangePauseMenuItemsVisibility(bool value)
    {
        pauseText.gameObject.SetActive(value);
        resumeButton.gameObject.SetActive(value);
        smallMenuButton.gameObject.SetActive(value);
        smallSettingsButton.gameObject.SetActive(value);
        resumeButton.transform.localPosition = _resumeButtonPausePosition;
    }

    public void ChangeUpperButtonsActivation(bool value)
    {
        pauseButton.interactable = value;
        informationButton.interactable = value;
    }
}
