using UnityEngine;
using UnityEngine.SceneManagement;

public class AirplaneSceneLoader : MonoBehaviour
{
    [Header("Настройки сцены")]
    [Tooltip("Точное имя сцены (как в Build Settings), например: FlightScene")]
    [SerializeField] private string sceneName;

    [Header("Отладка")]
    public bool showDebugPrompt = false;

    /// <summary>
    /// Вызывается при нажатии E, когда игрок смотрит на самолёт
    /// </summary>
    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"[AirplaneSceneLoader] Имя сцены не задано! Укажите его в инспекторе объекта {gameObject.name}.");
            return;
        }

        if (showDebugPrompt)
            Debug.Log($"Загрузка сцены: {sceneName}");

        SceneManager.LoadScene(sceneName);
    }
}