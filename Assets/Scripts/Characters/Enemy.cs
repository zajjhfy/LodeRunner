using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum Direction {Left, Right}
    private enum States {Run, Climb, Break, Fall, Swing}

    [Header("Layers")]
    [SerializeField] private LayerMask[] _layerMasks;

    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private EnemyController _enemyController;

    private States _currentState = States.Run;
    private Animator _animator;
    private CapsuleCollider2D _collider;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Vector2 _ladderVector;
    private Vector2 moveDirectionToPlayer;
    private float _moveSpeed = 2.5f;
    private string[] _animations;
    private bool inLadderCollider = false;

    private const string IS_RUNNING = "eisRunning";
    private const string IS_FALLING = "eisFalling";
    private const string IS_SWINGING = "eisSwinging";
    private const string IS_IDLE_SWINGING = "eisIdleSwinging";
    private const string IS_CLIMBING = "eisClimbing";
    private const string IS_IDLE_CLIMBING = "eisIdleClimbing";
    
    private void Awake(){
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GetAnimations();
    }

    private void GetAnimations(){
        _animations = new [] {IS_RUNNING, IS_FALLING, IS_SWINGING, IS_IDLE_SWINGING, IS_IDLE_CLIMBING, IS_CLIMBING};
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

    private void Climb()
    {
        var moveDirection = _enemyController.GetMoveDirectionY(transform);
        FlipSprite(moveDirection);
        if(moveDirection == Vector3.down || moveDirection == Vector3.up){
            StrictAnimationSwitch(IS_IDLE_CLIMBING);
            ClimbAnimationSwitch(true);
            LadderMove(moveDirection);
            transform.position = new Vector2(_ladderVector.x, transform.position.y);
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
        float playerXPos = _enemyController.GetPlayerXPosition();
        float playerYPos = _enemyController.GetPlayerYPosition();
        Vector3 moveDirection;
        // проверка на -y
        if(_enemyController.GetPlayerIsUp(transform)){
            moveDirection = _enemyController.RaycastLaddersUp(transform);
            MoveToLadder(moveDirection);
        }
        else{
            Move(playerXPos);
        }
    }

    private void Move(float xPos){
        Vector3 moveDirection;
        if(xPos < transform.position.x){
            moveDirection = Vector3.left;
            _rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime * _moveSpeed);
        }
        else if(xPos > transform.position.x){
            moveDirection = Vector3.right;
            _rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime * _moveSpeed);
        }
    }

    private void MoveToLadder(Vector3 moveDirection){
        _rb.MovePosition(transform.position + moveDirection * Time.fixedDeltaTime * _moveSpeed);
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
        bool isBlocked = moveDirection == Vector3.down ? CheckForGroundOnly() : false;
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
        else if(!ray){
            ray = CheckForLadder();
            if(ray.transform != null && ray.distance == 0 && !inLadderCollider){
                _rb.MovePosition(transform.position + new Vector3(0f, 0.05f, 0f));
            }
        }
        return ray;
    }

    private RaycastHit2D CheckForLadder(){
        return Physics2D.BoxCast(_collider.bounds.center, _collider.size, 0f, Vector2.down, 0.05f, _layerMasks[0]);
    }

    private bool CheckForGroundOnly(){
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
                    if((transform.position.y-0.05f) *-1 < colliderYpos * -1){
                        ChangeState(States.Fall);
                        break;
                    } 
                    ChangeState(States.Swing);
                    break;
                case "Ladder":
                    inLadderCollider = true;
                    _ladderVector = new Vector2(collider.gameObject.transform.position.x, transform.position.y);
                    bool playerIsUp = _enemyController.GetPlayerIsUp(transform);
                    bool playerIsDown = _enemyController.GetPlayerIsDown(transform);
                    if(playerIsUp || playerIsDown){
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
        inLadderCollider = false;
    }
}
