using UnityEngine;
using UnityEngine.SceneManagement;

public class retry : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}