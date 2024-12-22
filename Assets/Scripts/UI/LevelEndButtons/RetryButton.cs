using System;
using UnityEngine;

public class RetryButton : MonoBehaviour
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
