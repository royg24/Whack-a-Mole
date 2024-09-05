using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

public class CustomCursorController : MonoBehaviour
{
    public Texture2D cursorTexture; // התמונה של הסמן
    private float rotationAngle = 0f; // זווית הסיבוב הנוכחית
    public float rotationDuration = 0.1f; // הזמן שבו הסמן יישאר מסובב ב-90 מעלות

    void Start()
    {
        Cursor.visible = false; // מסתיר את הסמן המובנה של מערכת ההפעלה
    }

    void OnGUI()
    {
        // בודק אם cursorTexture לא מוגדר
        if (cursorTexture == null)
        {
            Debug.LogError("Cursor texture is not assigned. Please assign a texture in the Inspector.");
            return; // יוצא מהפונקציה אם אין תמונה מוגדרת
        }

        // שומר את המטריצה הנוכחית של GUI
        Matrix4x4 matrixBackup = GUI.matrix;

        // מסובב את הסמן מסביב לנקודת העכבר לפי הזווית הנוכחית
        GUIUtility.RotateAroundPivot(rotationAngle, Event.current.mousePosition);

        // מצייר את התמונה של הסמן
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x - cursorTexture.width / 2,
                                 Event.current.mousePosition.y - cursorTexture.height / 2,
                                 cursorTexture.width, cursorTexture.height), cursorTexture);

        // מחזיר את המטריצה למצב הקודם
        GUI.matrix = matrixBackup;
    }

    void Update()
    {
        // בודק אם יש לחיצה על כפתור העכבר השמאלי
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(RotateCursor());
        }
    }

    IEnumerator RotateCursor()
    {
        // מסובב את הסמן ב-90 מעלות שמאלה
        rotationAngle = -90f;

        // ממתין לזמן שהוגדר (rotationDuration)
        yield return new WaitForSeconds(rotationDuration);

        // מחזיר את הסמן למצב המקורי (0 מעלות)
        rotationAngle = 0f;
    }
}
