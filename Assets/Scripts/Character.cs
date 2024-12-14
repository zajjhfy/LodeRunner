using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    private enum Direction {Left, Right}
    private enum States {Run, Climb, Break, Fall, Swing}

    [Header("Sprites")]
    [SerializeField] private Sprite _brickBreakingSprite;
    [SerializeField] private Sprite _climbingSprite;

    [Header("Layers")]
    [SerializeField] private LayerMask[] _layerMasks;

    [Header("References")]
    [SerializeField] private PlayerController _playerController;

    private States _currentState = States.Run;
    private Animator _animator;
    private CapsuleCollider2D _collider;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private int _moveSpeed = 5;
    private bool _onRope;

    private const string IS_RUNNING = "isRunning";
    private const string IS_FALLING = "isFalling";
    
    private void Awake(){
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
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
        switch(_currentState){
            case States.Run:
                if(!CheckForGround()) {
                     SwitchRunAnimation(false);
                     SwitchFallAnimation(true);
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
                    SwitchFallAnimation(false);
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
        _rb.MovePosition(transform.position + Vector3.down * Time.fixedDeltaTime * _moveSpeed);
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
        var moveDirection = _playerController.GetMovementVector();
        FlipSprite(moveDirection);
        if(moveDirection == Vector3.right || moveDirection == Vector3.left){
            SwitchRunAnimation(true);
            Move(moveDirection);
        }
        else{
            SwitchRunAnimation(false);
        }
    }

    private void Move(Vector3 moveDirection){
        bool isBlocked = moveDirection == Vector3.right ? CheckForRightObstacle() : CheckForLeftObstacle();
        if(!isBlocked){
            _rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime * _moveSpeed);
        }
    }

    private void SwitchRunAnimation(bool toggle) => _animator.SetBool(IS_RUNNING, toggle);
    private void SwitchFallAnimation(bool toggle) => _animator.SetBool(IS_FALLING, toggle);

    private void ChangeState(States state) => _currentState = state;

    private void FlipSprite(Vector3 direction){
        if(direction == Vector3.right) _sr.flipX = false;
        else if(direction == Vector3.left) _sr.flipX = true;
    }


    private bool CheckForGround(){
        var ray = Physics2D.BoxCast(_collider.bounds.center, _collider.size, 0f, Vector2.down, 0.05f, _layerMasks[1]);
        if(ray.transform != null && ray.distance == 0){
            _rb.MovePosition(transform.position + new Vector3(0f, 0.05f, 0f));
        }
        return ray;
    }

    private bool CheckForRightObstacle(){
        var ray = Physics2D.BoxCast(_collider.bounds.center, _collider.size, 0f, Vector2.right, 0.05f, _layerMasks[1]);
        if(ray.transform != null && ray.distance == 0){
            _rb.MovePosition(transform.position + new Vector3(-.05f, 0f, 0f));
        }
        return ray;
    }

    private bool CheckForLeftObstacle(){
        var ray = Physics2D.BoxCast(_collider.bounds.center, _collider.size, 0f, Vector2.left, 0.05f, _layerMasks[1]);
        if(ray.transform != null && ray.distance == 0){
            _rb.MovePosition(transform.position + new Vector3(.05f, 0f, 0f));
        }
        return ray;
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Rope"){
            _onRope = true;
            ChangeState(States.Swing);
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Rope"){
            _onRope = false;
            ChangeState(States.Run);
        }
    }

}
