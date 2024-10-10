using System.Collections;
using UnityEngine;

public class PlayerCursorController : Singleton<PlayerCursorController>
{
    private Texture2D _cursorTexture;
    private float _rotationAngle = 0f;
    public float rotationDuration = 0.1f;
    private Vector2 _cursorSize;

    public void Start()
    {
        _cursorTexture = GameSettings.GameSettingsInstance.GetHammer().texture;
        Cursor.visible = false;
        _cursorSize = GameSettings.Instance.CursorSize;
    }

    void OnGUI()
    {
        if (!_cursorTexture)
        {
            return;
        }

        // Update cursor size based on screen dimensions
        //_cursorSize = new Vector2(Screen.width * 0.10f, Screen.height * 0.18f);

        Matrix4x4 matrixBackup = GUI.matrix;
        GUIUtility.RotateAroundPivot(_rotationAngle, Event.current.mousePosition);

        GUI.DrawTexture(new Rect(Event.current.mousePosition.x - _cursorSize.x / 2f,
            Event.current.mousePosition.y - _cursorSize.y / 2f,
            _cursorSize.x, _cursorSize.y), _cursorTexture);

        GUI.matrix = matrixBackup;
    }

    void Update()
    {
        if (GameManager.IsPause)
            _rotationAngle = 0f;
        else if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(RotateCursor());
        }
    }

    IEnumerator RotateCursor()
    {
        _rotationAngle = -90f;
        yield return new WaitForSeconds(rotationDuration);
        _rotationAngle = 0f;
    }
}