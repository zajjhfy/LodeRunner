using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance => _instance;
    private static EnemyController _instance;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _ladderMask;
    [SerializeField] private LayerMask _playerMask;

    private void Awake(){
        if(_instance == null) _instance = this;
    }

    public Vector3 GetMoveDirectionY(Transform enemyTransform){
        if(GetPlayerIsDownTest(enemyTransform)){
            return Vector3.down;
        }
        else if(GetPlayerIsUpTest(enemyTransform)){
            return Vector3.up;
        }
        return Vector3.zero;
    }
    // надо от лестницы до игрока рейкаст и где ближе туда и идти
    public Vector3 RaycastLaddersUp(Transform enemyTransform){
        float rightDistanceToPlayer = 0f;
        float leftDistanceToPlayer = 0f;
        float distance = Math.Abs(enemyTransform.position.x) + 10f;
        float distanceToLadder = 0f;

        var ray = Physics2D.Raycast(enemyTransform.position, Vector2.right, distance, _ladderMask);
        Debug.DrawRay(enemyTransform.position, Vector3.right * distance, Color.red);
        if(ray.transform != null){
            distanceToLadder = ray.distance;
            if(ray.transform.gameObject.CompareTag("Ladder")){
                var ladderPos = ray.transform.gameObject.transform.position;
                ray = Physics2D.Raycast(ladderPos, (_playerTransform.position - ladderPos).normalized, 25f, _playerMask);
                Debug.DrawRay(ladderPos, (_playerTransform.position - ladderPos).normalized * 25f, Color.red);
                if(ray.transform != null){
                    rightDistanceToPlayer = ray.distance + distanceToLadder;
                }
            }
        }

        distanceToLadder = 0f;
        ray = Physics2D.Raycast(enemyTransform.position, Vector2.left, distance, _ladderMask);
        Debug.DrawRay(enemyTransform.position, Vector3.left * distance, Color.red);
        if(ray.transform != null){
            distanceToLadder = ray.distance;
            if(ray.transform.gameObject.CompareTag("Ladder")){
                var ladderPos = ray.transform.gameObject.transform.position;
                ray = Physics2D.Raycast(ladderPos, (_playerTransform.position - ladderPos).normalized, 25f, _playerMask);
                Debug.DrawRay(ladderPos, (_playerTransform.position - ladderPos).normalized * 25f, Color.red);
                if(ray.transform != null){
                    leftDistanceToPlayer = ray.distance + distanceToLadder;
                }
            }
        }
        
        if(leftDistanceToPlayer > rightDistanceToPlayer && rightDistanceToPlayer > 0f && leftDistanceToPlayer > 0f){
            return Vector3.right;
        }
        else if(leftDistanceToPlayer < rightDistanceToPlayer && rightDistanceToPlayer > 0f && leftDistanceToPlayer > 0f){
            return Vector3.left;
        }
        else if(leftDistanceToPlayer > 0f && rightDistanceToPlayer == 0f){
            return Vector3.left;
        }
        else if(leftDistanceToPlayer == 0f && rightDistanceToPlayer > 0f){
            return Vector3.right;
        }

        return Vector3.zero;
    }

    // проверка лестниц в ground
    public Vector3 RaycastLaddersDown(Transform enemyTransform){
        float rightDistanceToPlayer = 0f;
        float leftDistanceToPlayer = 0f;
        float distance = Math.Abs(enemyTransform.position.x) + 10f;
        float distanceToLadder = 0f;
        Vector3 origin = new Vector3(enemyTransform.position.x, enemyTransform.position.y-1f);

        var ray = Physics2D.Raycast(origin, Vector2.right, distance, _ladderMask);
        Debug.DrawRay(origin, Vector3.right * distance, Color.red);
        if(ray.transform != null){
            distanceToLadder = ray.distance;
            if(ray.transform.gameObject.CompareTag("Ladder")){
                var ladderPos = ray.transform.gameObject.transform.position;

                ray = Physics2D.Raycast(ladderPos, (_playerTransform.position - ladderPos).normalized, 25f, _playerMask);
                Debug.DrawRay(ladderPos, (_playerTransform.position - ladderPos).normalized * 25f, Color.red);
                if(ray.transform != null){
                    rightDistanceToPlayer = ray.distance + distanceToLadder;
                }
            }
        }

        distanceToLadder = 0f;
        ray = Physics2D.Raycast(origin, Vector2.left, distance, _ladderMask);
        Debug.DrawRay(origin, Vector3.left * distance, Color.red);
        if(ray.transform != null){
            distanceToLadder = ray.distance;
            if(ray.transform.gameObject.CompareTag("Ladder")){
                var ladderPos = ray.transform.gameObject.transform.position;
                
                ray = Physics2D.Raycast(ladderPos, (_playerTransform.position - ladderPos).normalized, 25f, _playerMask);
                Debug.DrawRay(ladderPos, (_playerTransform.position - ladderPos).normalized * 25f, Color.red);
                if(ray.transform != null){
                    leftDistanceToPlayer = ray.distance + distanceToLadder;
                }
            }
        }
        
        if(leftDistanceToPlayer > rightDistanceToPlayer && rightDistanceToPlayer > 0f && leftDistanceToPlayer > 0f){
            return Vector3.right;
        }
        else if(leftDistanceToPlayer < rightDistanceToPlayer && rightDistanceToPlayer > 0f && leftDistanceToPlayer > 0f){
            return Vector3.left;
        }
        else if(leftDistanceToPlayer > 0f && rightDistanceToPlayer == 0f){
            return Vector3.left;
        }
        else if(leftDistanceToPlayer == 0f && rightDistanceToPlayer > 0f){
            return Vector3.right;
        }

        return Vector3.zero;
    }

    public bool RaycastCheckPlayerOnSameHeight(Transform enemyTransform){
        float distance = Math.Abs(enemyTransform.position.x) + 12f;

        var ray = Physics2D.Raycast(enemyTransform.position, Vector3.left, distance, _playerMask);
        Debug.DrawRay(enemyTransform.position, Vector3.left * distance, Color.cyan);
        if(ray) return true;

        ray = Physics2D.Raycast(enemyTransform.position, Vector3.right, distance, _playerMask);
        Debug.DrawRay(enemyTransform.position, Vector3.right * distance, Color.cyan);
        if(ray) return true;

        return false;
    }

    public bool GetPlayerIsDown(Transform enemyTransform){
        return _playerTransform.position.y < enemyTransform.position.y-1f;
    }

    public bool GetPlayerIsUp(Transform enemyTransform){
        return _playerTransform.position.y > enemyTransform.position.y+1f;
    }

    public bool GetPlayerIsUpTest(Transform enemyTransform){
        return _playerTransform.position.y > enemyTransform.position.y;
    }

    public bool GetPlayerIsDownTest(Transform enemyTransform){
        return _playerTransform.position.y < enemyTransform.position.y;
    }

    public float GetPlayerXPosition() => _playerTransform.position.x;

    public float GetPlayerYPosition() => _playerTransform.position.y;
    
}
