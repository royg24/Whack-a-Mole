using System;
using UnityEngine;
using System.Collections;

public class MoleHole : MonoBehaviour
{
    private Mole _mole;
    private Vector3 _startPosition;
    [SerializeField] private Mole molePrefab;


    private void Awake()
    {
        _mole = GetComponentInChildren<Mole>();
        _startPosition = transform.position;

        if (_mole == null)
        {
            _mole = Instantiate(molePrefab, transform.localPosition, Quaternion.identity);
            _mole.transform.SetParent(transform);
            _mole.transform.localPosition = Vector3.zero;
        }
    }

    public void ActivateMoleHole()
    {
        _mole.ActivateMole();
    }

    public void InactivateMole()
    {
        GameManager.GameManagerInstance.InactivateMoleHole(this);
    }

    public void InitializeMoleHole()
    {
        transform.position = _startPosition;
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
