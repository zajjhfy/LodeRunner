using UnityEngine;

public class Candy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            SoundManager.PlaySound(SoundType.Collect);
            ScoreField.Instance.UpdateScore();
            Destroy(gameObject);
        }
    }
    
}
