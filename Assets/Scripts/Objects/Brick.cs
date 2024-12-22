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
            SoundManager.PlaySound(SoundType.Break);
            StartCoroutine(BreakBlockCoroutine());
            toggled = true;
        }
        _coroutineworking = true;
    }

    private void ChangeCollider(){
        _collider.offset = new Vector2(0f, -0.5f);
        _collider.size = new Vector2(1f, 0.2f);
        gameObject.layer = 0;
        _collider.isTrigger = true;
    }

    private void ReturnCollider(){
        _collider.offset = new Vector2(0f, 0f);
        _collider.size = new Vector2(1f, 1f);
        gameObject.layer = 6;
        _collider.isTrigger = false;
    }

    private IEnumerator BreakBlockCoroutine(){
        int i = 0;
        while(i < 7){
            yield return new WaitForSeconds(.05f);
            _spriteRenderer.sprite = _breakingSprites[i];
            if(i < 4){
                _particlesSpriteRenderer.sprite = _particleSprites[i];
            }
            if(i > 4){
                _particlesSpriteRenderer.sprite = null;
            }
            i++;
        }
        _spriteRenderer.sprite = null;
        ChangeCollider();
        StartCoroutine(RecoverBlockCoroutine());
    }

    private IEnumerator RecoverBlockCoroutine(){
        yield return new WaitForSeconds(6f);
        int i = 0;
        while(i < 6){
            yield return new WaitForSeconds(.1f);
            _spriteRenderer.sprite = _recoverySprites[i];
            i++;
        }
        ReturnCollider();
        _coroutineworking = false;
    }

    private void OnTriggerStay2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Enemy")){
            SoundManager.PlaySound(SoundType.Trapped);
            Sprite sprite = collider.gameObject.GetComponent<SpriteRenderer>().sprite;
            _spriteRenderer.sprite = sprite;
            ReturnCollider();
        }
    }
}
