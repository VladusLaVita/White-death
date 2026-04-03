using UnityEngine;
using UnityEngine.SceneManagement;
public class changlevl : MonoBehaviour
{
    public void LoadScene(string levl)
    {
        SceneManager.LoadScene(levl);  
    }

}
