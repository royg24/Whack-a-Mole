using System.Collections;
using UnityEngine;

public class Mole : MonoBehaviour
{
    private Camera _camera;

    [Header("Show-Hide")]
    [SerializeField] private float showHideDuration = 0.6f;
    [SerializeField] private float outDuration = 1f;
    [SerializeField] private float hurtDuration = 0.75f;
    [SerializeField] private float quickHideDuration = 0.3f;
    public static float Delay { get; private set; }

    [Header("Positions")]
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _boxOffset;
    private Vector3 _boxSize;
    private Vector3 _boxOffsetHidden;
    private Vector3 _boxSizeHidden;

    [Header("Sprites")] 
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite hurtMole;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;

    private MoleHole _parent;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private bool _hittable = true;

    // Start is called before the first frame update

    void Start()
    {
        Delay = showHideDuration / 3f;
    }


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();  
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _parent = GetComponentInParent<MoleHole>();
        _camera = Camera.main;
        _startPosition = new Vector3(0, -2.56f, 0);
        _endPosition = Vector3.zero;
        _boxOffset = _boxCollider2D.offset;
        _boxSize = _boxCollider2D.size;
        _boxOffsetHidden = new Vector3(_boxOffset.x, -_startPosition.y / 2f, 0f);
        _boxSizeHidden = new Vector3(_boxOffset.x, 0f, 0f);
        InitializeMole();
    }



    public void ActivateMole()
    {
        StartCoroutine(ShowHide());
    }

    private IEnumerator ShowHide()
    {

        transform.localPosition = _startPosition;

        //show the mole
        yield return StartCoroutine(ShowHideLoop(_startPosition, _endPosition, showHideDuration, 
            _boxOffsetHidden, _boxOffset, _boxSizeHidden, _boxSize));

        //let the mole be out of the hole
        yield return new WaitForSeconds(outDuration);

        //hide the mole

        yield return StartCoroutine(ShowHideLoop(_endPosition, _startPosition, showHideDuration, 
            _boxOffset, _boxOffsetHidden, _boxSize, _boxSizeHidden));
    }

    private IEnumerator ShowHideLoop(Vector3 startPosition, Vector3 endPosition, float showDuration,
        Vector3 offsetHidden, Vector3 offset, Vector3 sizeHidden, Vector3 size)
    {
        float elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsed / showDuration);
            _boxCollider2D.offset = Vector3.Lerp(offsetHidden, offset, elapsed / showDuration);
            _boxCollider2D.size = Vector3.Lerp(sizeHidden, size, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPosition;
        _boxCollider2D.offset = offset;
        _boxCollider2D.size = size;
    }

    private IEnumerator QuickHide()
    {
        GameManager.PlaySoundEffect(audioSource, hitSound);
        yield return new WaitForSeconds(hurtDuration);

        if (!_hittable)
        {
            yield return StartCoroutine(ShowHideLoop(transform.localPosition, _startPosition, quickHideDuration, 
                _boxOffset, _boxOffsetHidden, _boxSize, _boxSizeHidden));
            _spriteRenderer.sprite = mole;
            _hittable = true;
            _parent.InactivateMole();
            GameManager.GameManagerInstance.StopDelay();
        }
    }
    private void OnMouseDown()
    {
        // Only proceed if the mole is hittable
        if (_hittable)
        {
            var clickPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (clickPosition.y > transform.position.y)
            {
                _hittable = false;
                _spriteRenderer.sprite = hurtMole;
                GameManager.GameManagerInstance.AddScore();
                StopAllCoroutines();
                StartCoroutine(QuickHide());
            }
        }
    }

    public void InitializeMole()
    {
        transform.localPosition = _startPosition;
        _spriteRenderer.sprite = mole;
        _boxCollider2D.offset = _boxOffsetHidden;
        _boxCollider2D.size = _boxSizeHidden;
    }

}
