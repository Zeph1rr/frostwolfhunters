using UnityEngine;

public class UI : MonoBehaviour
{
    public void CloseGame() {
        Debug.Log("quit game");
        Application.Quit();
    }
}
