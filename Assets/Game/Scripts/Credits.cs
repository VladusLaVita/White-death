using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}