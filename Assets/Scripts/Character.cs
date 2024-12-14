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
    private string[] _animations;

    private const string IS_RUNNING = "isRunning";
    private const string IS_FALLING = "isFalling";
    private const string IS_SWINGING = "isSwinging";
    private const string IS_IDLE_SWINGING = "isIdleSwinging";
    private const string IS_CLIMBING = "isClimbing";
    private const string IS_IDLE_CLIMBING = "isIdleClimbing";
    
    private void Awake(){
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GetAnimations();
        _playerController.OnBreakButtonPressed += _playerOnBreakButtonPressed;
    }

    private void GetAnimations(){
        _animations = new [] {IS_RUNNING, IS_FALLING, IS_SWINGING, IS_IDLE_SWINGING, IS_IDLE_CLIMBING, IS_CLIMBING};
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
                     StrictAnimationSwitch(IS_FALLING);
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
                    StrictAnimationSwitch();
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
        var moveDirection = _playerController.GetMovementVector();
        FlipSprite(moveDirection);
        if(moveDirection == Vector3.right || moveDirection == Vector3.left){
            StrictAnimationSwitch(IS_IDLE_SWINGING);
            SwingAnimationSwitch(true);
            RopeMove(moveDirection);
        }
        else if(moveDirection == Vector3.down){
            StrictAnimationSwitch(IS_FALLING);
            _rb.MovePosition(transform.position + Vector3.down * Time.fixedDeltaTime * _moveSpeed);
            ChangeState(States.Fall);
        }
        else{
            StrictAnimationSwitch(IS_IDLE_SWINGING);
        }
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
        var moveDirection = _playerController.GetMovementVector();
        FlipSprite(moveDirection);
        if(moveDirection == Vector3.down || moveDirection == Vector3.up){
            StrictAnimationSwitch(IS_IDLE_CLIMBING);
            ClimbAnimationSwitch(true);
            LadderMove(moveDirection);
        }
        else if(moveDirection == Vector3.right || moveDirection == Vector3.left){
            StrictAnimationSwitch(IS_RUNNING);
            Move(moveDirection);
        }
        else{
            StrictAnimationSwitch(IS_IDLE_CLIMBING);
        }
    }

    private void Run()
    {
        var moveDirection = _playerController.GetMovementVector();
        FlipSprite(moveDirection);
        if(moveDirection == Vector3.right || moveDirection == Vector3.left){
            StrictAnimationSwitch(IS_RUNNING);
            Move(moveDirection);
        }
        else{
            StrictAnimationSwitch();
        }
    }

    private void Move(Vector3 moveDirection){
        bool isBlocked = moveDirection == Vector3.right ? CheckForRightObstacle() : CheckForLeftObstacle();
        if(!isBlocked){
            _rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime * _moveSpeed);
        }
    }

    private void RopeMove(Vector3 moveDirection){
        _rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime * _moveSpeed);
    }

    private void LadderMove(Vector3 moveDirection){
        Debug.Log(CheckForGround());
        bool isBlocked = moveDirection == Vector3.down ? CheckForGround() : false;
        if(!isBlocked){
            _rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime * _moveSpeed);
        }
    }

    private void SwingAnimationSwitch(bool toggle) => _animator.SetBool(IS_SWINGING, toggle);
    private void ClimbAnimationSwitch(bool toggle) => _animator.SetBool(IS_CLIMBING, toggle);

    private void StrictAnimationSwitch(string animationToTurn){
        foreach (var item in _animations){
            if(item == animationToTurn) _animator.SetBool(item, true);
            else _animator.SetBool(item, false);
        }
    }

    private void StrictAnimationSwitch(){
        foreach (var item in _animations){
            _animator.SetBool(item, false);
        }
    }

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

    private void OnTriggerStay2D(Collider2D collider){
        if(_currentState != States.Swing && _currentState != States.Climb){
            switch(collider.gameObject.tag){
                case "Rope":
                    float colliderYpos = collider.gameObject.transform.position.y;
                    if(transform.position.y *-1 > colliderYpos * -1){
                        Debug.Log($"{transform.position.y} : player; {colliderYpos} : rope");
                        ChangeState(States.Fall);
                        break;
                    } 
                    ChangeState(States.Swing);
                    break;
                case "Ladder":
                    float colliderXpos = collider.gameObject.transform.position.x;
                    Vector3 moveDirection = _playerController.GetMovementVector();
                    if(moveDirection == Vector3.down || moveDirection == Vector3.up){
                        _rb.MovePosition(new Vector3(colliderXpos, transform.position.y, transform.position.z));
                        ChangeState(States.Climb);
                        break;
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Rope" && _currentState == States.Swing){
            ChangeState(States.Run);
        }
        else if(collider.gameObject.tag == "Ladder" && _currentState == States.Climb){
            ChangeState(States.Run);
        }
    }

}
