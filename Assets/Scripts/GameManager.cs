using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public static GameManager GameManagerInstance { get; private set; }
    [SerializeField] private List<MoleHole> moleHoles;
    [SerializeField] private MoleHole moleHolePrefab;
    private HashSet<MoleHole> _activeMoleHoles;
    private const float StartingTime = 10f;
    private float _timeRemaining;
    private float _timer;
    private int _score;
    private bool _playing;

    [Header("UI Objects")] 
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private TextMeshProUGUI scoreHeader;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeHeader;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Audio")] 
    [SerializeField] private AudioSource gameAudioSource;
    [SerializeField] private AudioSource instantAudioSource;
    [SerializeField] private AudioClip hammerSound;

    // Start is called before the first frame update
    void Start()
    {
        restartButton.SetActive(false);
        scoreHeader.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timeHeader.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        gameAudioSource.Play();
    }

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
          startButton.SetActive(false);
          RestartGame();
          CreateMoleHoles();
    }

    public void RestartGame()
    {
        ChangeMoleHolesAppearance(true);
        restartButton.SetActive(false);
        gameOverText.SetActive(false);
        _timeRemaining = StartingTime;
        _timer = 0f;
        _score = 0;
        _playing = true;
        scoreHeader.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        timeHeader.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        timeText.text  = Mathf.RoundToInt(StartingTime).ToString();
        scoreText.text = "000";
        _activeMoleHoles = new HashSet<MoleHole>();
    }

    private void CreateMoleHoles()
    {
        moleHoles = new List<MoleHole>();
        var horizontalInterval = 4;
        var verticalInterval = 3;
    
        for (var i = 1; i > -2; i--)
        {
            for (var j = -1; j < 2; j++)
            {
                var moleHole = Instantiate(moleHolePrefab,
                    new Vector3(j * horizontalInterval, i * verticalInterval, 0),
                    quaternion.identity);
                moleHoles.Add(moleHole);
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
                _timer += Time.deltaTime;
                if(Input.GetMouseButtonDown(0))
                    PlaySoundEffect(instantAudioSource, hammerSound);
                if (_timer >= Mole.Delay)
                {
                    ActivateRandomMoleHole();
                    _timer = 0f;
                }

                scoreText.text = _score.ToString("D3");
            }
        }
    }

    private void ActivateRandomMoleHole()
    {
        var moleHoleIndex = Random.Range(0, moleHoles.Count);
        var currentMoleHole = moleHoles[moleHoleIndex];

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
    }

    public static void PlaySoundEffect(AudioSource audioSource, AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    private void GameOver()
    {
        _playing = false;
        ChangeMoleHolesAppearance(false);
        restartButton.SetActive(true);
        gameOverText.SetActive(true);
    }

    private void ChangeMoleHolesAppearance(bool appearance)
    {
        foreach (var moleHole in moleHoles)
        {
            moleHole.gameObject.SetActive(appearance); 
            //moleHoles._
        }
    }

    public void StopDelay()
    {
        _timer = Mole.Delay;
    }
}
