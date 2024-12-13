using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    private enum Direction {Left, Right}
    private enum States {Run, Climb, Break, Fall, Swing}

    [Header("Sprites")]
    [SerializeField] private Sprite _brickBreakingSprite;
    [SerializeField] private Sprite _climbingSprite;
    [SerializeField] private Sprite _fallingSprite;

    [Header("Layers")]
    [SerializeField] private LayerMask[] _layerMasks;

    private States _currentState = States.Run;
    private PlayerController _playerController;
    private Animator _animator;
    private BoxCollider2D _collider;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private int _moveSpeed = 7;

    private const string IS_RUNNING = "isRunning";
    
    private void Awake(){
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _playerController = new PlayerController();
        _playerController.OnBreakButtonPressed += _playerOnBreakButtonPressed;
    }

    private void _playerOnBreakButtonPressed(object sender, ButtonPressedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void FixedUpdate()
    {   
        CheckState();
    }

    private void CheckState(){
        //Debug.Log(_currentState);
        switch(_currentState){
            case States.Run:
                if(!CheckForGround()) {
                    ChangeToFallSprite();
                    ChangeState(States.Fall);
                    break;
                }
                Run();
                break;
            case States.Climb:
                Climb();
                break;
            case States.Break:
                Break();
                break;
            case States.Fall:
                if(CheckForGround()){
                    ChangeState(States.Run);
                    break;
                }
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
        Debug.Log(CheckForGround());
        var moveDirection = _playerController.GetMovementVector();
        FlipSprite(moveDirection);
        if(moveDirection == Vector3.right || moveDirection == Vector3.left){
            StartRunAnimation(true);
            Move(moveDirection);
        }
        else{
            StartRunAnimation(false);
        }
    }

    private void Move(Vector3 moveDirection){
        _rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime * _moveSpeed);
    }

    private void StartRunAnimation(bool toggle) => _animator.SetBool(IS_RUNNING, toggle);

    private void ChangeState(States state) => _currentState = state;

    private void FlipSprite(Vector3 direction){
        if(direction == Vector3.right) _sr.flipX = false;
        else if(direction == Vector3.left) _sr.flipX = true;
    }

    private void ChangeToFallSprite() => _sr.sprite = _fallingSprite;

    private bool CheckForGround(){
        return Physics2D.BoxCast(_collider.bounds.center, _collider.size, 0f, Vector2.down, 0.05f, _layerMasks[1]);
    }

}
