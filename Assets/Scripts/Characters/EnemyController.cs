using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _ladderMask;
    [SerializeField] private LayerMask _playerMask;

    public Vector3 GetMoveDirectionY(Transform enemyTransform){
        if(GetPlayerIsDown(enemyTransform)){
            return Vector3.down;
        }
        else if(GetPlayerIsUp(enemyTransform)){
            return Vector3.up;
        }
        return Vector3.zero;
    }
    // надо от лестницы до игрока рейкаст и где ближе туда и идти
    public Vector3 RaycastLaddersUp(Transform enemyTransform){
        float rightDistanceToPlayer = 0f;
        float leftDistanceToPlayer = 0f;
        float distance = Math.Abs(enemyTransform.position.x) + 12f;

        var ray = Physics2D.Raycast(enemyTransform.position, Vector2.right, distance, _ladderMask);

        if(ray.transform != null){
            if(ray.transform.gameObject.CompareTag("Ladder")){
                var ladderPos = ray.transform.gameObject.transform.position;
                ray = Physics2D.Raycast(ladderPos, (_playerTransform.position - ladderPos).normalized, 25f, _playerMask);
                if(ray.transform != null){
                    rightDistanceToPlayer = ray.distance;
                }
            }
        }

        ray = Physics2D.Raycast(enemyTransform.position, Vector2.left, distance, _ladderMask);

        if(ray.transform != null){
            if(ray.transform.gameObject.CompareTag("Ladder")){
                var ladderPos = ray.transform.gameObject.transform.position;
                ray = Physics2D.Raycast(ladderPos, (_playerTransform.position - ladderPos).normalized, 25f, _playerMask);
                if(ray.transform != null){
                    leftDistanceToPlayer = ray.distance;
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

    public Vector3 RaycastLaddersDown(Transform enemyTransform){

        var ray = Physics2D.Raycast(enemyTransform.position, Vector2.right, 12f, _ladderMask);
        if(ray.transform.gameObject.CompareTag("Ladder")){
            var ladderPos = ray.transform.gameObject.transform.position;
            ray = Physics2D.Raycast(ladderPos, Vector2.down, 1f, _groundMask);
            if(!ray) return Vector2.right;
        }

        ray = Physics2D.Raycast(enemyTransform.position, Vector2.left, 12f, _ladderMask);
        if(ray.transform.gameObject.CompareTag("Ladder")){
            var ladderPos = ray.transform.gameObject.transform.position;
            ray = Physics2D.Raycast(ladderPos, Vector2.down, 1f, _groundMask);
            if(!ray) return Vector2.left;
        }
        
        return Vector3.zero;
    }

    public bool GetPlayerIsDown(Transform enemyTransform){
        return _playerTransform.position.y < enemyTransform.position.y-1f;
    }

    public bool GetPlayerIsUp(Transform enemyTransform){
        return _playerTransform.position.y > enemyTransform.position.y+1f;
    }

    public float GetPlayerXPosition() => _playerTransform.position.x;

    public float GetPlayerYPosition() => _playerTransform.position.y;
    
}
