using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private PlayerController _controller;

    private void Awake(){
        _pauseMenu.SetActive(false);
    }

    private void Start(){
        _controller.OnPauseButtonPressed += _controller_OnPauseButtonPressed;
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
}
