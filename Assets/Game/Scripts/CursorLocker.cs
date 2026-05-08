using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    void Start()
    {
        // Скрывает курсор и фиксирует его в центре окна
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Нажмите ESC, чтобы вернуть курсор во время игры
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
