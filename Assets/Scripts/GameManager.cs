using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private List<Mole> moles;
    [SerializeField] private Mole molePrefab;
    private const float StartingTime = 60f;
    private float _timeRemaining;
    private int _score;
    private bool _playing;
    [Header("UI Objects")] 
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private TextMeshProUGUI scoreHeader;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeHeader;
    [SerializeField] private TextMeshProUGUI timeText;

    // Start is called before the first frame update
    void Start()
    {
        scoreHeader.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timeHeader.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = Singleton<GameManager>.Instance;
            SetMolesPositions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {      
          startButton.SetActive(false);
          _timeRemaining = StartingTime;
          _score = 0;
          _playing = true;
          scoreHeader.gameObject.SetActive(true);
          scoreText.gameObject.SetActive(true);
          timeHeader.gameObject.SetActive(true);
          timeText.gameObject.SetActive(true);
          timeText.text  = Mathf.RoundToInt(StartingTime).ToString();
          scoreText.text = "000";
          var moleIndex = Random.Range(0, moles.Count);
          Debug.Log(moleIndex);
          moles[moleIndex].ActivateMole();
    }

    private void SetMolesPositions()
    {
        moles = new List<Mole>();
        var horizontalIntervale = 4;
        var verticalInterval = 3;
    
        for (var i = 1; i > -2; i--)
        {
            for (var j = -1; j < 2; j++)
            {
                var mole = Instantiate(molePrefab, new Vector3(j * horizontalIntervale, i * verticalInterval, 0), quaternion.identity);
                moles.Add(mole);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (_playing)
        {
            _timeRemaining -= Time.deltaTime;
            timeText.text = Mathf.RoundToInt(_timeRemaining).ToString();
            if (_timeRemaining <= 0)
            {
                _timeRemaining = 0;
                GameOver();
            }
            else
            {
                //var moleIndex = Random.Range(0, moles.Count);
                //moles[moleIndex].ActivateMole();
                scoreText.text = _score.ToString("D3");
            }
        }
    }

    public void AddScore()
    {
        _score += 10;
    }

    void GameOver()
    {
        _playing = false;
        gameOverText.gameObject.SetActive(true);
    }
}
