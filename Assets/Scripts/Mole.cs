using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Mole : MonoBehaviour
{
    public AudioSource audioSource; 
    
    [SerializeField] private float showHideDuration = 1.5f;
    [SerializeField] private float outDuration = 1f;
    [SerializeField] private float hurtDuration = 0.75f;
    [SerializeField] private float quickHideDuration = 0.75f;
    [SerializeField] private Camera camera;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _boxOffset;
    private Vector3 _boxSize;
    private Vector3 _boxOffsetHidden;
    private Vector3 _boxSizeHidden;


    [Header("Sprites")] 
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite hurtMole;


    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;
    private bool _hittable = true;

    // Start is called before the first frame update

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        camera = Camera.main;
        _animator = GetComponent<Animator>();
        _startPosition = transform.localPosition;
        _startPosition.y -= 2f;
        _endPosition = transform.localPosition;
        _endPosition.y++;
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxOffset = _boxCollider2D.offset;
        _boxSize = _boxCollider2D.size;
        _boxOffsetHidden = new Vector3(_boxOffset.x, -_startPosition.y / 2f, 0f);
        _boxSizeHidden = new Vector3(_boxOffset.x, 0f, 0f);
    }


    private void Awake()
    {
       
    }

    public void ActivateMole()
    {
        _boxCollider2D.enabled = true;
        //Debug.Log(String.Format("Position = {0}\nStart position = {1}\n End position = {2}",transform.localPosition, _startPosition, _endPosition));
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
        playSoundEffect();
        yield return new WaitForSeconds(hurtDuration);

        if (!_hittable)
        {
            yield return StartCoroutine(ShowHideLoop(_endPosition, _startPosition, quickHideDuration, 
                _boxOffset, _boxOffsetHidden, _boxSize, _boxSizeHidden));
            _spriteRenderer.sprite = mole;
        }
    }
    private void OnMouseDown()
    {
        // Only proceed if the mole is hittable
        if (_hittable)
        {
            Vector3 clickPosition = camera.ScreenToWorldPoint(Input.mousePosition);

            if (clickPosition.y > transform.position.y)
            {
                _hittable = false;
                _spriteRenderer.sprite = hurtMole;
                _boxCollider2D.enabled = false;
                GameManager.Instance.AddScore();
                StopAllCoroutines();
                StartCoroutine(QuickHide());
            }
        }
    }

    public void playSoundEffect()
    {
        audioSource.Play();
    }
}
