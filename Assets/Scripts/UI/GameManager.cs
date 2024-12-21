using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private PlayerController _controller;
    [SerializeField] private GameObject _gameSceneEnter;

    private void Awake(){
        Time.timeScale = 0;
        _pauseMenu.SetActive(false);
        _gameSceneEnter.SetActive(true);
    }

    private void OnEnable(){
        _controller.OnGameStartPressed += _controller_OnGameStartPressed;
        _controller.OnPauseButtonPressed += _controller_OnPauseButtonPressed;
        _controller.OnExitButtonPressed += _controller_OnExitButtonPressed;
    }

    private void _controller_OnGameStartPressed(object sender, EventArgs e)
    {
        Destroy(_gameSceneEnter);
        Time.timeScale = 1;
        _controller.EnableInputMap();
        _controller.DisableGameStartMap();
        _controller.OnGameStartPressed -= _controller_OnGameStartPressed;
    }

    private void _controller_OnExitButtonPressed(object sender, EventArgs e)
    {
        _controller.DisableInputMap();
        DisablePauseMenu();
        SceneManager.LoadScene(0);
    }

    private void _controller_OnPauseButtonPressed(object sender, EventArgs e)
    {
        if(_pauseMenu.activeSelf){
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        else{
            _pauseMenu.SetActive(true); 
            Time.timeScale = 0;
        }
    }

    private void DisablePauseMenu(){
        if(_pauseMenu.activeSelf){
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void OnDisable(){
        _controller.OnPauseButtonPressed -= _controller_OnPauseButtonPressed;
        _controller.OnExitButtonPressed -= _controller_OnExitButtonPressed;
    }
}
