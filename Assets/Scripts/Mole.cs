using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Mole : MonoBehaviour
{
    [Header("Positions")]
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _boxOffset;
    private Vector3 _boxSize;
    private Vector3 _boxOffsetHidden;
    private Vector3 _boxSizeHidden;

    private MoleHole _parent;
    private float _validClickPosition;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private bool _hittable = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();  
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _parent = GetComponentInParent<MoleHole>();
        _startPosition = GameSettings.GameSettingsInstance.StartPosition;
        _endPosition = GameSettings.GameSettingsInstance.EndPosition;
        _boxOffset = _boxCollider2D.offset;
        _boxSize = _boxCollider2D.size;
        GameSettings.GameSettingsInstance.SetBoxCollider2DSettings(_boxOffset.x);
        _boxOffsetHidden = GameSettings.GameSettingsInstance.BoxOffsetHidden;
        _boxSizeHidden = GameSettings.GameSettingsInstance.BoxSizeHidden;
        _validClickPosition = _parent.FindBottomY() + GameSettings.HoleSize;
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
        yield return StartCoroutine(ShowHideLoop(_startPosition, _endPosition, 
            GameSettings.GameSettingsInstance.ShowHideDuration, 
            _boxOffsetHidden, _boxOffset, _boxSizeHidden, _boxSize));

        //let the mole be out of the hole
        yield return new WaitForSeconds(GameSettings.GameSettingsInstance.OutDuration);

        //hide the mole

        yield return StartCoroutine(ShowHideLoop(_endPosition, _startPosition, 
            GameSettings.GameSettingsInstance.ShowHideDuration, 
            _boxOffset, _boxOffsetHidden, _boxSize, _boxSizeHidden));
        _parent.InactivateMole();
    }

    private IEnumerator ShowHideLoop(Vector3 startPosition, Vector3 endPosition, float showDuration,
        Vector3 offsetHidden, Vector3 offset, Vector3 sizeHidden, Vector3 size)
    {
        var elapsed = 0f;
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
        GameSettings.GameSettingsInstance.PlayHitSound();
        yield return new WaitForSeconds(GameSettings.GameSettingsInstance.HurtDuration);

        if (!_hittable)
        {
            yield return StartCoroutine(ShowHideLoop(transform.localPosition, _startPosition, 
                GameSettings.GameSettingsInstance.QuickHideDuration, 
                _boxOffset, _boxOffsetHidden, _boxSize, _boxSizeHidden));
            _spriteRenderer.sprite = GameSettings.GameSettingsInstance.GetMoleSprite(false);
            _hittable = true;
            _parent.InactivateMole();
            GameManager.GameManagerInstance.StopDelay();
        }
    }
    private void OnMouseDown()
    {
        // Only proceed if the mole is hittable
        if (_hittable && !GameManager.IsPause)
        {
            var clickPosition = GameSettings.MainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (clickPosition.y > _validClickPosition)
            {
                _hittable = false;
                _spriteRenderer.sprite = GameSettings.GameSettingsInstance.GetMoleSprite(true);
                GameManager.GameManagerInstance.AddScore();
                StopAllCoroutines();
                StartCoroutine(_parent.AddingScoreRoutine());
                StartCoroutine(QuickHide());
            }
        }
    }

    public void InitializeMole()
    {
        transform.localPosition = _startPosition;
        _spriteRenderer.sprite = GameSettings.GameSettingsInstance.GetMoleSprite(false);
        _boxCollider2D.offset = _boxOffsetHidden;
        _boxCollider2D.size = _boxSizeHidden;
        _hittable = true;
    }

}