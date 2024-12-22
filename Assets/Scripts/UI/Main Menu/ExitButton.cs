using System;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public event Action OnLevelSelectionExit;
    [SerializeField] private GameObject _exitText;
    [SerializeField] private PlayButton _playButton;
    private Shadow _exitShadow;
    private bool _onLevelSelection = false;

    private void Start(){
        _exitShadow = _exitText.GetComponent<Shadow>();
        _exitShadow.enabled = false;
    }

    private void OnEnable(){
        _playButton.OnLevelSelection += TurnLevelSelection;
    }

    private void TurnLevelSelection()
    {
        _onLevelSelection = true;
        _exitText.transform.GetComponent<Text>().text = "BACK";
    }

    private void OnMouseEnter(){
        _exitShadow.enabled = true;
        SoundManager.PlaySound(SoundType.ButtonSelect, 0.1f);
    }

    private void OnMouseDown(){
        if(!_onLevelSelection){
            Application.Quit();
        }
        else{
            _exitText.transform.GetComponent<Text>().text = "EXIT";
            _onLevelSelection = false;
            OnLevelSelectionExit?.Invoke();
        }
    }

    private void OnMouseExit(){
        _exitShadow.enabled = false;
    }
}
