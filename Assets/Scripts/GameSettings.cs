using Enums;
using UnityEngine;
using UnityEngine.Serialization;

public class GameSettings : Singleton<GameSettings>
{
    public static GameSettings GameSettingsInstance { get; private set; }
    public static Camera MainCamera { get; private set; }
    public const string HighScoreData = " High Score";
    public const string Plus = "+";
    public const float StartingTime = 12f;
    public const int GoodScoreIntervals = 20;
    public const int RegularScoreIntervals = 10;
    public const int BadScoreIntervals = -10;
    public const float HorizontalIntervals = 4f;
    public const float VerticalIntervals = 2.5f;
    public const float EndDelayDuration = 4f;
    public const float HoleSize = 0.401789f;
    public const float SideMoveDuration = 1f;
    public const float ScoreAddingDuration = 0.6f;
    public const int InitDifficulty = 1;
    public const float MinOpacity = 0f;
    public const float MaxOpacity = 1f;

    // Values that change in each difficulty
    public static float DelayDuration { get; private set; }
    public float ShowHideDuration { get; private set; }
    public float OutDuration { get; private set; }
    public float HurtDuration { get; private set; }
    public float QuickHideDuration { get; private set; }

    public readonly Vector3 StartPosition = new Vector3(0f, -2.56f, 0f);
    public readonly Vector3 EndPosition = new Vector3(0f, -0.5f, 0f);
    public readonly Vector3 SideMove = new Vector3(1f, 0f, 0f);
    public readonly Vector3 ScoreAddingAdditionToPosition = new Vector3(1.3f, 0.6f, 0f);
    public readonly Vector2 CursorSize = new Vector2(Screen.width * 0.1f, Screen.height * 0.18f);
    public Vector3 BoxOffsetHidden { get; private set; }
    public Vector3 BoxSizeHidden { get; private set; }

    [FormerlySerializedAs("mole")]
    [Header("Sprites")]
    [SerializeField] private Sprite regularMole;
    [FormerlySerializedAs("hurtMole")] [SerializeField] private Sprite regularHurtMole;
    [SerializeField] private Sprite goodMole;
    [SerializeField] private Sprite goodHurtMole;
    [SerializeField] private Sprite badMole;
    [SerializeField] private Sprite badHurtMole;
    [SerializeField] private Sprite hammer;
    public Sprite RegularMole => regularMole;
    public Sprite RegularHurtMole => regularHurtMole;
    public Sprite GoodMole => goodMole;
    public Sprite GoodHurtMole => goodHurtMole;
    public Sprite BadMole => badMole;
    public Sprite BadHurtMole => badHurtMole;


    [Header("Audio")]
    [SerializeField] private AudioSource gameAudioSource;
    [SerializeField] private AudioSource shortAudioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip hammerSound;
    [SerializeField] private AudioClip newHighScoreSound;

    public void Awake()
    {
        if (GameSettingsInstance == null)
        {
            GameSettingsInstance = Singleton<GameSettings>.Instance;
            MainCamera = Camera.main;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameAudioSource.Play();
    }

    public void SetDifficultySettings(EDifficulty difficulty)
    {
        UIManager.UIManagerInstance.SetDifficultyHeader(difficulty);
        switch (difficulty)
        {
             case EDifficulty.Easy:
                 SetEasySetting();
                 break;
             case EDifficulty.Medium:
                 SetMediumSettings();
                 break;
             case EDifficulty.Hard:
                 SetHardSettings();
                 break;
        }
    }

    private void SetEasySetting()
    {
        ShowHideDuration = 2f;
        OutDuration = 1f;
        HurtDuration = 0.75f;
        QuickHideDuration = 0.6f;
        DelayDuration = 0.2f;
    }

    private void SetMediumSettings()
    {
        ShowHideDuration = 1f;
        OutDuration = 0.6f;
        HurtDuration = 0.75f;
        QuickHideDuration = 0.3f;
        DelayDuration = 0.4f;
    }

    private void SetHardSettings()
    {
        ShowHideDuration = 0.5f;
        OutDuration = 0.3f;
        HurtDuration = 0.75f;
        QuickHideDuration = 0.2f;
        DelayDuration = 0.6f;
    }

    public void SetBoxCollider2DSettings(float x)
    {
        BoxOffsetHidden = new Vector3(x, -StartPosition.y / 2f, 0f);
        BoxSizeHidden = new Vector3(x, 0f, 0f);
    }

    private void PlayShortSound(AudioClip sound)
    {
        shortAudioSource.PlayOneShot(sound);
    }

    public void PlayHammerSound()
    {
       PlayShortSound(hammerSound);
    }

    public void PlayHitSound()
    {
        PlayShortSound(hitSound);
    }

    public void PlayHighScoreSound()
    {
        PlayShortSound(newHighScoreSound);
    }

    public Sprite GetHammer()
    {
        return hammer;
    }
}
