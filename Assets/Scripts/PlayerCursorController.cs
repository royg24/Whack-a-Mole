using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

public class PlayerCursorController : Singleton<PlayerCursorController>
{
    public Texture2D cursorTexture; // ������ �� ����
    private float rotationAngle = 0f; // ����� ������ �������
    public float rotationDuration = 0.1f; // ���� ��� ���� ����� ����� �-90 �����

    void Start()
    {
        Cursor.visible = false; // ����� �� ���� ������ �� ����� ������
    }

    void OnGUI()
    {
        // ���� �� cursorTexture �� �����
        if (cursorTexture == null)
        {
            Debug.LogError("Cursor texture is not assigned. Please assign a texture in the Inspector.");
            return; // ���� ��������� �� ��� ����� ������
        }

        // ���� �� ������� ������� �� GUI
        Matrix4x4 matrixBackup = GUI.matrix;

        // ����� �� ���� ����� ������ ����� ��� ������ �������
        GUIUtility.RotateAroundPivot(rotationAngle, Event.current.mousePosition);

        // ����� �� ������ �� ����
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x - cursorTexture.width / 2f,
                                 Event.current.mousePosition.y - cursorTexture.height / 2f,
                                 cursorTexture.width, cursorTexture.height), cursorTexture);

        // ����� �� ������� ���� �����
        GUI.matrix = matrixBackup;
    }

    void Update()
    {
        // ���� �� �� ����� �� ����� ����� ������
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(RotateCursor());
        }
    }

    IEnumerator RotateCursor()
    {
        // ����� �� ���� �-90 ����� �����
        rotationAngle = -90f;

        // ����� ���� ������ (rotationDuration)
        yield return new WaitForSeconds(rotationDuration);

        // ����� �� ���� ���� ������ (0 �����)
        rotationAngle = 0f;
    }
}
