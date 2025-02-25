using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

    [SerializeField] GameObject _pauseMenu;

    private GameInput _gameInput;

    public void Initialize(GameInput gameInput)
    {
        _gameInput = gameInput;
        _gameInput.OnPausePressed += HandlePause;
    }

    private void HandlePause(object sender, EventArgs e)
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
    }

    public void Play()
    {
        _gameInput.Unpause();
    }

    public void QuitScene() 
    {
        Debug.Log("quit scene");
        SceneManager.LoadScene("Menu");
    }
}
