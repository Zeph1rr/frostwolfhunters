using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public void QuitScene() {
        Debug.Log("quit scene");
        SceneManager.LoadScene("Menu");
    }
}
