using System;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    public event Action OnClick;

    private void OnEnable(){
        Debug.Log(this + " enabled");
    }

    private void OnMouseDown(){
        Debug.Log("press");
        OnClick?.Invoke();
    }
}
