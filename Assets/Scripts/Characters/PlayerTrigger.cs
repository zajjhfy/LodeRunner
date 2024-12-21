using System;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public event EventHandler OnGameLose;
    private GameManager _gameManager;

    private void Start(){
        _gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Enemy"){
            OnGameLose?.Invoke(this, EventArgs.Empty);
        }
    }
}
