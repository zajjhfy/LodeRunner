using System;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    private enum Direction {Left, Right}
    private enum States {Run, Climb, Break, Fall, Swing}

    [Header("Sprites")]
    [SerializeField] private Sprite _brickBreakingSprite;
    [SerializeField] private Sprite _climbingSprite;
    [SerializeField] private Sprite _fallingSprite;
    [SerializeField] private Sprite[] _runSprites;

    [Header("Layers")]
    [SerializeField] private LayerMask[] _layerMasks;

    private States _currentState = States.Run;
    private SpriteRenderer _spriteRenderer;
    private Direction _currentDirection;
    private PlayerController _playerController;
    private BoxCollider2D _collider;
    private bool isGrounded;
    private int _currentRunSprite = 0;
    private int _moveSpeed = 5;
    

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _playerController = new PlayerController();
        _playerController.OnBreakButtonPressed += _playerOnBreakButtonPressed;
    }

    private void _playerOnBreakButtonPressed(object sender, ButtonPressedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {   
        CheckState();
    }

    private void CheckState(){
        switch(_currentState){
            case States.Run:
                Run();
                break;
            case States.Climb:
                Climb();
                break;
            case States.Break:
                Break();
                break;
            case States.Fall:
                Fall();
                break;
            case States.Swing:
                Swing();
                break;
        }
    }

    private void Swing()
    {
        throw new NotImplementedException();
    }

    private void Fall()
    {
        throw new NotImplementedException();
    }

    private void Break()
    {
        throw new NotImplementedException();
    }

    private void Climb()
    {
        throw new NotImplementedException();
    }

    private void Run()
    {
        Debug.Log(isGrounded);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.gameObject.layer == _layerMasks[1].value){
            isGrounded = true;
        }
        else{
            isGrounded = false;
        }
    }
}
