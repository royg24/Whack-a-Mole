using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    public static GameSettings GameSettingsInstance { get; private set; }
    public static float DelayDuration { get; private set; }
    public const string HighScoreData = "High Score";
    public const float StartingTime = 10f;

    public float ShowHideDuration { get; private set; }
    public float OutDuration { get; private set; }
    public float HurtDuration { get; private set; }
    public float QuickHideDuration { get; private set; }

    public readonly Vector3 StartPosition = new Vector3(0f, -2.56f, 0f);
    public readonly Vector3 EndPosition = Vector3.zero;
    public Vector3 BoxOffsetHidden { get; private set; }
    public Vector3 BoxSizeHidden { get; private set; }

    [Header("Sprites")]
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite hurtMole;

    [Header("Audio")]
    [SerializeField] private AudioSource gameAudioSource;
    [SerializeField] private AudioSource instantAudioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip hammerSound;

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
        SetTimeSettings();
        DelayDuration = ShowHideDuration / 3f;
    }

    private void SetTimeSettings()
    {
        //TODO Add difficulties
        ShowHideDuration = 1f;
        OutDuration = 1f;
        HurtDuration = 0.75f;
        QuickHideDuration = 0.3f;
    }

    public void SetBoxCollider2DSettings(float x)
    {
        BoxOffsetHidden = new Vector3(x, -StartPosition.y / 2f, 0f);
        BoxSizeHidden = new Vector3(x, 0f, 0f);
    }

    private void PlayShortSound(AudioClip sound)
    {
        instantAudioSource.PlayOneShot(sound);
    }

    public void PlayHammerSound()
    {
       PlayShortSound(hammerSound);
    }

    public void PlayHitSound()
    {
        PlayShortSound(hitSound);
    }

    public Sprite GetMoleSprite(bool hurt)
    {
        return hurt ? hurtMole : mole;
    }
}
