using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 100f;
    [Range(0.1f, 10f)]
    public float smoothing = 1.5f;

    [Header("References")]
    public Transform playerBody;

    private float _xRotation = 0f;
    private Vector2 _currentMouseDelta;
    private Vector2 _appliedMouseDelta;

    void Start()
    {
        // Скрываем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // 1. Получаем «сырой» ввод (без фильтрации ОС)
        Vector2 targetMouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // 2. Правильное сглаживание, не зависящее от кадров в секунду
        // Используем 10.0f как множитель для удобства настройки smoothing
        _appliedMouseDelta = Vector2.Lerp(_appliedMouseDelta, targetMouseDelta, Time.deltaTime * 10f / smoothing);

        // 3. Расчет вращения (чувствительность теперь стабильна)
        float mouseX = _appliedMouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = _appliedMouseDelta.y * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        // 4. Применяем вращение
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
