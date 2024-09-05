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
    public Vector3 StartPosition { get; set; }
    public Vector3 EndPosition { get; set; }

    [Header("Sprites")] 
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite hurtMole;

    [Header("UI Objects")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject scoreHeader;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI timeHeader;
    [SerializeField] private TMPro.TextMeshProUGUI TimeText;

    private float startingTime = 30f;
    private float timeRemaining;
    private HashSet<Mole> currentMoles = new HashSet<Mole>();
    private int score;

    private SpriteRenderer _spriteRenderer;
    private bool _hittable = true;
    // Start is called before the first frame update

    void Start()
    {
        //to make the mole show and hide
    }

    // public void StartGame()
    //{
    //  for (int i=0;i<moles.Count;i++)
    // {
    //    moles[i].Hide
    //}
    //}

    //public void GameOver(int type)
    //{
    //show the massege
    //  if (type == 0)
    //    gameOver.SetActive(true);

    //Hide all moles
    //foreach Mole mole in moles
    //  mole.StopGame();
    // }

    //public void AddScore(int moleIndex)
    //{
    //    //Add and update score
    //    score += 1;
    //    scoreText.text = $"{score}";
    //    //Remove from active moles
    //    //currentMoles.Remove(moles[moleIndex]);
    //}

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //start and end position might change for different moles in the game managerS
        StartPosition = new Vector3(0f, -2f, 0f);
        EndPosition = new Vector3(0f, 1f, 0f);
        StartCoroutine(ShowHide(StartPosition, EndPosition));
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
            yield return StartCoroutine(ShowHideLoop(EndPosition, StartPosition, quickHideDuration));
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
