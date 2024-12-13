using System.Collections;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private Sprite[] _breakingSprites;
    [SerializeField] private Sprite[] _recoverySprites;
    [SerializeField] private Sprite[] _particleSprites;
    [SerializeField] private Transform _particlesTransform;
    private SpriteRenderer _particlesSpriteRenderer;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;
    private bool _coroutineworking = false;

    private void Start(){
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _particlesSpriteRenderer = _particlesTransform.GetComponent<SpriteRenderer>();
    }
    
    public void Break(out bool toggled){
        toggled = false;
        if(!_coroutineworking){
            StartCoroutine(BreakBlockCoroutine());
            toggled = true;
        }
        _coroutineworking = true;
    }

    private void ChangeColliderSize(){
        _collider.offset = new Vector2(0f, -0.5f);
        _collider.size = new Vector2(1f, 0.005f);
    }

    private void ReturnColliderSize(){
        _collider.offset = new Vector2(0f, 0f);
        _collider.size = new Vector2(1f, 1f);
    }

    private IEnumerator BreakBlockCoroutine(){
        int i = 0;
        while(i < 7){
            yield return new WaitForSeconds(.1f);
            _spriteRenderer.sprite = _breakingSprites[i];
            if(i < 4){
                _particlesSpriteRenderer.sprite = _particleSprites[i];
            }
            if(i > 4){
                _particlesSpriteRenderer.sprite = null;
            }
            i++;
        }
        ChangeColliderSize();
        StartCoroutine(RecoverBlockCoroutine());
    }

    private IEnumerator RecoverBlockCoroutine(){
        yield return new WaitForSeconds(10f);
        int i = 0;
        while(i < 6){
            yield return new WaitForSeconds(.1f);
            _spriteRenderer.sprite = _recoverySprites[i];
            i++;
        }
        ReturnColliderSize();
        _coroutineworking = false;
    }
}
