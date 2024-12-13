using UnityEngine;

public class Candy : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayerMask;
    private BoxCollider2D _collider;

    private void Start(){
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update(){
        if(CheckForPlayer()){
            ScoreField.Instance.UpdateScore();
            Destroy(gameObject);
        }
    }

    private bool CheckForPlayer(){
        return Physics2D.BoxCast(_collider.bounds.center, _collider.size, 0f, Vector2.one, 0.1f, _playerLayerMask);
    }
}
