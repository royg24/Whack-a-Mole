using UnityEngine;

public class MoleHole : MonoBehaviour
{
    private Mole _mole;
    [SerializeField] private Mole molePrefab;


    private void Awake()
    {
        _mole = GetComponentInChildren<Mole>();

        if (_mole == null)
        {
            _mole = Instantiate(molePrefab, transform.localPosition, Quaternion.identity);
            _mole.transform.SetParent(transform);
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

    public void HideMole()
    {
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
}
