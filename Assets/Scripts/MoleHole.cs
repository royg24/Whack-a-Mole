using System;
using UnityEngine;
using System.Collections;
using TMPro;
using Unity.Mathematics;

public class MoleHole : MonoBehaviour
{
    private Mole _mole;
    private Vector3 _startPosition;
    private TextMeshProUGUI _scoreAddingText;
    private CanvasGroup _canvasGroup;
    [SerializeField] private Mole molePrefab;


    private void Awake()
    {
        _mole = GetComponentInChildren<Mole>();
        _startPosition = transform.position;
        InitScoreAddingTexts();
        InitCanvasGroup();

        if (_mole == null)
        {
            _mole = Instantiate(molePrefab, transform.localPosition, Quaternion.identity);
            _mole.transform.SetParent(transform);
            _mole.transform.localPosition = Vector3.zero;
        }
    }

    private void InitScoreAddingTexts()
    {
        _scoreAddingText = Instantiate(UIManager.UIManagerInstance.scoreAddingTextPrefab, 
            UIManager.UIManagerInstance.GetCanvas().transform, false);

        ChangeScoreAddingTextPosition();
        _scoreAddingText.gameObject.SetActive(true);
        _scoreAddingText.text = GameSettings.Plus + _mole.ScoreIntervals;
    }

    private void InitCanvasGroup()
    {
        _canvasGroup = _scoreAddingText.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    private void ChangeScoreAddingTextPosition()
    {
        Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(GameSettings.MainCamera, 
            transform.position + 
            GameSettings.GameSettingsInstance.ScoreAddingAdditionToPosition);
        _scoreAddingText.transform.position = screenPosition;
    }

    public IEnumerator AddingScoreRoutine()
    {
        if (GameManager.GameManagerInstance.IsDifficultyHard())
            ChangeScoreAddingTextPosition();
        

        var growthDuration = GameSettings.ScoreAddingDuration / 4;

        yield return StartCoroutine(OpacityRoutine(GameSettings.MinOpacity, 
            GameSettings.MaxOpacity, 2 * growthDuration, growthDuration));

        yield return new WaitForSeconds(growthDuration);

        yield return StartCoroutine(OpacityRoutine(GameSettings.MaxOpacity, 
            GameSettings.MinOpacity, growthDuration, growthDuration));

    }

    private IEnumerator OpacityRoutine(float startOpacity, float endOpacity, float duration, float normalize)
    {
        for (var t = 0f; t < duration; t += Time.deltaTime)
        {
            var normalizedTime = t / normalize;
            _canvasGroup.alpha = Mathf.Lerp(startOpacity, endOpacity, normalizedTime);
            yield return null;
        }

        _canvasGroup.alpha = endOpacity;
    }

    public void ActivateMoleHole()
    {
        _mole.ActivateMole();
    }

    public void InactivateMole()
    {
        GameManager.GameManagerInstance.InactivateMoleHole(this);
    }

    public void SelectMoleType()
    {
        _mole.SelectRandomMoleType();
    }

    public void InitializeMoleHole()
    {
        transform.position = _startPosition;
        _canvasGroup.alpha = 0;
        _mole.InitializeMole();
    }

    public float FindBottomY()
    {
        float result = 0;
        var spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

        if (spriteRenderers.Length > 0)
        {
            var combinedBounds = spriteRenderers[0].bounds;

            foreach (var spriteRenderer in spriteRenderers)
            {
                combinedBounds.Encapsulate(spriteRenderer.bounds);  
            }

            result = combinedBounds.min.y;
        }
        return result;
    }

    public void MoveSideAndReturn()
    {
        var endPosition = _startPosition + GameSettings.GameSettingsInstance.SideMove;

        StartCoroutine(MoveSideAndReturnRoutine(_startPosition, endPosition));
    }

    private IEnumerator MoveSideAndReturnRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        // Move to the side
        yield return StartCoroutine(MoveToSide(startPosition, endPosition));

        // After moving to the side, move back to the start
        yield return StartCoroutine(MoveToSide(endPosition, startPosition));

        GameManager.GameManagerInstance.MakeMoleHoleMoveable(this);
    }

    private IEnumerator MoveToSide(Vector3 startPosition, Vector3 endPosition)
    {
        var duration = GameSettings.SideMoveDuration;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }


}
