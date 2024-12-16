using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private GameObject _playText;
    private Shadow _playShadow;

    private void Start(){
        _playShadow = _playText.GetComponent<Shadow>();
        _playShadow.enabled = false;
    }

    private void OnMouseEnter(){
        _playShadow.enabled = true;
    }

    private void OnMouseDown(){
        SceneManager.LoadScene(0);
    }

    private void OnMouseExit(){
        _playShadow.enabled = false;
    }
}
