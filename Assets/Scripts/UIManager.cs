using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public static UIManager UIManagerInstance { get; private set; }

    [Header("Buttons")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject menuButton;

    [Header("Backgrounds")] 
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Texture2D startBackgroundImage;
    [SerializeField] private Texture2D gameBackgroundImage;

    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI scoreHeader;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Time Texts")]
    [SerializeField] private TextMeshProUGUI timeHeader;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("High Score Texts")]
    [SerializeField] private TextMeshProUGUI highScoreHeader;
    [SerializeField] private TextMeshProUGUI highScoreText;

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
        StartUI();
    }

    public void StartUI()
    {
        ChangeStartButtonVisibility(true);
        ChangeHighScoreVisibility(true);
        restartButton.SetActive(false);
        menuButton.SetActive(false);
        ChangeGameUIVisibility(false);
        ChangeEndUIVisibility(false);
    }

    public void ChangeStartButtonVisibility(bool value)
    { 
        startButton.SetActive(value);
    }

    private void ChangeGameUIVisibility(bool value)
    {
        scoreHeader.gameObject.SetActive(value);
        scoreText.gameObject.SetActive(value);
        timeHeader.gameObject.SetActive(value);
        timeText.gameObject.SetActive(value);
    }

    private void ChangeHighScoreVisibility(bool value)
    {
        highScoreText.gameObject.SetActive(value);
        highScoreHeader.gameObject.SetActive(value);
    }

    public void SwitchGameModesUI(bool playing)
    {
        ChangeGameUIVisibility(playing);
        ChangeHighScoreUI(playing);
        GameManager.GameManagerInstance.WaitDelay(GameSettings.EndDelayDuration);
        ChangeEndUIVisibility(!playing);
        GameManager.GameManagerInstance.WaitDelay(GameSettings.EndDelayDuration);
    }


    private void ChangeEndUIVisibility(bool value)
    {
        restartButton.SetActive(value);
        menuButton.SetActive(value);
        gameOverText.SetActive(value);
        endScoreHeader.gameObject.SetActive(value);
        endScoreText.gameObject.SetActive(value);
        newHighScoreText.gameObject.SetActive(value);
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
}
