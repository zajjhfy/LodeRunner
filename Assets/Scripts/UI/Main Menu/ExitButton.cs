using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private GameObject _exitText;
    private Shadow _exitShadow;

    private void Start(){
        _exitShadow = _exitText.GetComponent<Shadow>();
        _exitShadow.enabled = false;
    }

    private void OnMouseEnter(){
        _exitShadow.enabled = true;
    }

    private void OnMouseDown(){
        Application.Quit();
    }

    private void OnMouseExit(){
        _exitShadow.enabled = false;
    }
}
