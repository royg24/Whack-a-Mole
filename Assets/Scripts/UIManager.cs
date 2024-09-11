using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UIManager : Singleton<UIManager>
{
    public static UIManager UIManagerInstance { get; private set; }
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private TextMeshProUGUI scoreHeader;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeHeader;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI highScoreHeader;
    [SerializeField] private TextMeshProUGUI highScoreText;

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
        startButton.SetActive(true);
        restartButton.SetActive(false);
        ChangeGameUIVisibility(false);
    }

    public void StartUI(bool value)
    {
          ChangeGameUIVisibility(value);
          startButton.SetActive(false);
    }

    private void ChangeGameUIVisibility(bool value)
    {
        scoreHeader.gameObject.SetActive(value);
        scoreText.gameObject.SetActive(value);
        timeHeader.gameObject.SetActive(value);
        timeText.gameObject.SetActive(value);
        gameOverText.gameObject.SetActive(value);
        highScoreText.gameObject.SetActive(value);
        highScoreHeader.gameObject.SetActive(value);
    }

    public void SwitchGameModesUI(bool playing)
    {
        ChangeGameUIVisibility(playing);
        GameManager.GameManagerInstance.WaitDelay(4f);
        restartButton.SetActive(!playing);
        gameOverText.SetActive(!playing);
        GameManager.GameManagerInstance.WaitDelay(4f);
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

}
