using System.Collections;
using Enums;
using UnityEngine;

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
    private Sprite _mole;
    private Sprite _hurtMole;
    public int ScoreIntervals { get; private set; }
    public Color MoleColor { get; private set; }

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
        ChangeToRegularMole();
    }

    public void ActivateMole()
    {
        SelectRandomMoleType();
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

    public void SelectRandomMoleType()
    {
        var moleTypeIndex = Random.Range(0, 6);
        var difficulty = GameManager.GameManagerInstance.GameDifficulty;

        switch (difficulty)
        {
            case EDifficulty.Easy:
                ChangeMoleInEasy(moleTypeIndex);
                break;
            case EDifficulty.Medium:
                ChangeMoleInMedium(moleTypeIndex);
                break;
            case EDifficulty.Hard:
                ChangeMoleInHard(moleTypeIndex);
                break;
        }
        _spriteRenderer.sprite = _mole;
    }

     /*
        for easy difficulty the odds for each mole are:
        regular: 67% (4/6)
        good: 33% (2/6)
        bad: 0%  (0/6)
     */
    private void ChangeMoleInEasy(int index)
    {
        if (index < 4)
            ChangeToRegularMole();
        else
            ChangeToGoodMole();
    }

     /*
        for medium difficulty the odds for each mole are:
        regular: 50% (3/6)
        good: 33% (2/6)
        bad: 17%  (1/6)
     */
    private void ChangeMoleInMedium(int index)
    {
         if(index < 3)
             ChangeToRegularMole();
         else if (index < 5)
             ChangeToGoodMole();
         else
             ChangeToBadMole();
    }

     /*
        for hard difficulty the odds for each mole are:
        regular: 33% (2/6)
        good: 33% (2/6)
        bad: 33%  (2/6)
     */
    private void ChangeMoleInHard(int index)
    {
        if(index < 2)
            ChangeToRegularMole();
        else if (index < 4)
            ChangeToGoodMole();
        else
            ChangeToBadMole();
    }
    private void ChangeToRegularMole()
    {
        _mole = GameSettings.GameSettingsInstance.RegularMole;
        _hurtMole = GameSettings.GameSettingsInstance.RegularHurtMole;
        ScoreIntervals = GameSettings.RegularScoreIntervals;
        MoleColor = GameSettings.GameSettingsInstance.RegularColor;
    }
    
    private void ChangeToGoodMole()
    {
        _mole = GameSettings.GameSettingsInstance.GoodMole;
        _hurtMole = GameSettings.GameSettingsInstance.GoodHurtMole;
        ScoreIntervals = GameSettings.GoodScoreIntervals;
        MoleColor = GameSettings.GameSettingsInstance.GoodColor;
    }
    
    private void ChangeToBadMole()
    {
        _mole = GameSettings.GameSettingsInstance.BadMole;
        _hurtMole = GameSettings.GameSettingsInstance.BadHurtMole;
        ScoreIntervals = GameSettings.BadScoreIntervals;
        MoleColor = GameSettings.GameSettingsInstance.BadColor;
    }

    private Sprite GetMoleSprite(bool hurt)
    {
        return hurt ? _hurtMole : _mole;
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
            _spriteRenderer.sprite = GetMoleSprite(false);
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
                _spriteRenderer.sprite = GetMoleSprite(true);
                GameManager.GameManagerInstance.AddScore(ScoreIntervals);
                StopAllCoroutines();
                StartCoroutine(_parent.AddingScoreRoutine());
                StartCoroutine(QuickHide());
            }
        }
    }

    public void InitializeMole()
    {
        transform.localPosition = _startPosition;
        _spriteRenderer.sprite = GetMoleSprite(false);
        _boxCollider2D.offset = _boxOffsetHidden;
        _boxCollider2D.size = _boxSizeHidden;
        _hittable = true;
    }

}