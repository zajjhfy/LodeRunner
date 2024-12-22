using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public event Action OnLevelSelection;
    [SerializeField] private GameObject _playText;
    [SerializeField] private GameObject _buttonsTransform;
    [SerializeField] private GameObject _levelsCanvas;
    [SerializeField] private ExitButton _exitButton;
    private int _levelIndex = -1;
    private bool _onLevelSelection = false;
    private Shadow _playShadow;

    private void Start(){
        _playShadow = _playText.GetComponent<Shadow>();
        _playShadow.enabled = false;
    }

    private void OnEnable(){
        _exitButton.OnLevelSelectionExit += ExitLevelSelection;
    }

    private void ExitLevelSelection()
    {
        _buttonsTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        _levelsCanvas.SetActive(false);
        _onLevelSelection = false;
    }

    private void OnMouseEnter(){
        _playShadow.enabled = true;
        SoundManager.PlaySound(SoundType.ButtonSelect, 0.1f);
    }

    private void OnMouseDown(){
        GoToLevelSelection();
    }

    private void OnMouseExit(){
        _playShadow.enabled = false;
    }

    private void GoToLevelSelection(){
        if(!_onLevelSelection){
            _buttonsTransform.transform.localPosition = new Vector3(0f, -20f, 0f);
            _levelsCanvas.SetActive(true);
            _onLevelSelection = true;
            OnLevelSelection?.Invoke();
        }
        else if(_levelIndex != -1){
            SceneManager.LoadScene(_levelIndex);
        }
    }

    public void SetLevel(int levelIndex) => _levelIndex = levelIndex;
}
