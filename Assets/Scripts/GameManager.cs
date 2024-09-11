using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public static GameManager GameManagerInstance { get; private set; }
    private List<MoleHole> _moleHoles;
    [SerializeField] private MoleHole moleHolePrefab;
    private HashSet<MoleHole> _activeMoleHoles;
    private float _timeRemaining;
    private float _timer;
    private int _score;
    private bool _playing;
    private int _highScore;


    private void Awake()
    {
        if (GameManagerInstance == null)
        {
            GameManagerInstance = Singleton<GameManager>.Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        UIManager.UIManagerInstance.StartUI(true);
        CreateMoleHoles();
        RestartGame(); 
    }

    public void RestartGame()
    {
        _highScore = PlayerPrefs.GetInt(GameSettings.HighScoreData, 0);
        UIManager.UIManagerInstance.RestartUI(_highScore, GameSettings.StartingTime);
        ChangeMoleHolesVisibility(true);
        _timeRemaining = GameSettings.StartingTime;
        _timer = 0f;
        _score = 0;
        _playing = true;
        _activeMoleHoles = new HashSet<MoleHole>();
    }

    private void CreateMoleHoles()
    {
        _moleHoles = new List<MoleHole>();
        var horizontalInterval = 4f;
        var verticalInterval = 2.5f;
    
        for (var i = 1; i > -2; i--)
        {
            for (var j = -1; j < 2; j++)
            {
                var moleHole = Instantiate(moleHolePrefab,
                    new Vector3(j * horizontalInterval, i * verticalInterval, 0),
                    quaternion.identity);
                _moleHoles.Add(moleHole);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (_playing)
        {
            _timeRemaining -= Time.deltaTime;
            UIManager.UIManagerInstance.UpdateTime(_timeRemaining);

            if (_timeRemaining <= 0)
            {
                _timeRemaining = 0;
                GameOver();
            }
            else
            {
                _timer += Time.deltaTime;
                if (Input.GetMouseButtonDown(0))
                    GameSettings.GameSettingsInstance.PlayHammerSound();
                if (_timer >= GameSettings.DelayDuration)
                {
                    ActivateRandomMoleHole();
                    _timer = 0f;
                }

                UIManager.UIManagerInstance.UpdateScoreText(_score);
            }
        }
    }

    private void ActivateRandomMoleHole()
    {
        var moleHoleIndex = Random.Range(0, _moleHoles.Count);
        var currentMoleHole = _moleHoles[moleHoleIndex];

        if (!_activeMoleHoles.Contains(currentMoleHole))
        {
            _activeMoleHoles.Add(currentMoleHole);
            currentMoleHole.ActivateMoleHole();
        }
    }

    public void InactivateMoleHole(MoleHole moleHole)
    {
        _activeMoleHoles.Remove(moleHole);
    }

    public void AddScore()
    {
        _score += 10;

        if (_score >= _highScore)
        {
            _highScore = _score;
            UpdateHighScore();
        }
    }


    private void GameOver()
    {
        _playing = false;
        UIManager.UIManagerInstance.SwitchGameModesUI(false);
        ChangeMoleHolesVisibility(false);
    }

    public void WaitDelay(float delay)
    {
        _timer = 0;
        while (_timer <= delay)
            _timer += Time.deltaTime;
    }

    private void ChangeMoleHolesVisibility(bool value)
    {
        foreach (var moleHole in _moleHoles)
        {
            moleHole.gameObject.SetActive(value);
            moleHole.HideMole();
        }
    }

    public void StopDelay()
    {
        _timer = GameSettings.DelayDuration;
    }

    private void UpdateHighScore()
    {
        PlayerPrefs.SetInt(GameSettings.HighScoreData, _highScore);
        PlayerPrefs.Save();
        UIManager.UIManagerInstance.UpdateHighScoreText(_highScore);
    }
}
