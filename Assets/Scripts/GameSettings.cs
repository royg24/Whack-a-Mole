using Enums;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    public static GameSettings GameSettingsInstance { get; private set; }
    public const string HighScoreData = " High Score";
    public const float StartingTime = 60f;
    public const int ScoreIntervals = 10;
    public const float HorizontalIntervals = 4f;
    public const float VerticalIntervals = 2.5f;
    public const float EndDelayDuration = 4f;
    public const float HoleSize = 0.401789f;
    public const float SideMoveDuration = 1f;
    public const int InitDifficulty = 1;

    // Values that change in each difficulty
    public static float DelayDuration { get; private set; }
    public float ShowHideDuration { get; private set; }
    public float OutDuration { get; private set; }
    public float HurtDuration { get; private set; }
    public float QuickHideDuration { get; private set; }

    public readonly Vector3 StartPosition = new Vector3(0f, -2.56f, 0f);
    public readonly Vector3 EndPosition = new Vector3(0f, -0.5f, 0f);
    public readonly Vector3 SideMove = new Vector3(1f, 0f, 0f);
    public readonly Vector2 CursorSize = new Vector2(Screen.width * 0.1f, Screen.height * 0.18f);
    public Vector3 BoxOffsetHidden { get; private set; }
    public Vector3 BoxSizeHidden { get; private set; }

    [Header("Sprites")]
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite hurtMole;
    [SerializeField] private Sprite hammer;


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

    public Sprite GetMoleSprite(bool hurt)
    {
        return hurt ? hurtMole : mole;
    }

    public Sprite GetHammer()
    {
        return hammer;
    }
}
