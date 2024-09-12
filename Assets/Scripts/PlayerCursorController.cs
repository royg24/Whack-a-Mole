using System.Collections;
using UnityEngine;

public class PlayerCursorController : Singleton<PlayerCursorController>
{
    private Texture2D _cursorTexture;
    private float _rotationAngle = 0f;
    public float rotationDuration = 0.1f;

    void Start()
    {
        _cursorTexture = GameSettings.GameSettingsInstance.GetHammer().texture;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        if (!_cursorTexture)
        {
            return;
        }

        Matrix4x4 matrixBackup = GUI.matrix;
        GUIUtility.RotateAroundPivot(_rotationAngle, Event.current.mousePosition);

        GUI.DrawTexture(new Rect(Event.current.mousePosition.x - _cursorTexture.width / 2f,
            Event.current.mousePosition.y - _cursorTexture.height / 2f,
            _cursorTexture.width, _cursorTexture.height), _cursorTexture);

        GUI.matrix = matrixBackup;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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