using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Mole : MonoBehaviour
{
    [SerializeField] private float showHideDuration = 1.5f;
    [SerializeField] private float outDuration = 1f;
    [SerializeField] private float hurtDuration = 0.75f;
    [SerializeField] private float quickHideDuration = 0.75f;
    Vector3 _startPosition = new Vector3(0f, -2f, 0f);
    Vector3 _endPosition = new Vector3(0f, 1f, 0f);

    [Header("Sprites")] 
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite hurtMole;

    private SpriteRenderer _spriteRenderer;
    private bool _hittable = true;
    // Start is called before the first frame update
    void Start()
    {
        //to make the mole show and hide
        //StartCoroutine(ShowHide(_startPosition, _endPosition));
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator ShowHide(Vector3 startPosition, Vector3 endPosition)
    {
        transform.localPosition = startPosition;

        //show the mole
        yield return StartCoroutine(ShowHideLoop(startPosition, endPosition, showHideDuration));

        //let the mole be out of the hole
        yield return new WaitForSeconds(outDuration);

        //hide the mole
        yield return StartCoroutine(ShowHideLoop(endPosition, startPosition, showHideDuration));
    }

    private IEnumerator ShowHideLoop(Vector3 startPosition, Vector3 endPosition, float showDuration)
    {
        float elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPosition;
    }

    private IEnumerator QuickHide()
    {
        yield return new WaitForSeconds(hurtDuration);

        if (!_hittable)
        {
            yield return StartCoroutine(ShowHideLoop(_endPosition, _startPosition, quickHideDuration));
            _spriteRenderer.sprite = mole;
        }
    }

    private void OnMouseDown()
    {
        if (_hittable)
        {
            _hittable = false;
            _spriteRenderer.sprite = hurtMole;
            StopAllCoroutines();
            StartCoroutine(QuickHide());
            _hittable = false;
        }
    }

}
