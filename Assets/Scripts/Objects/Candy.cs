using UnityEngine;

public class Candy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            ScoreField.Instance.UpdateScore();
            Destroy(gameObject);
        }
    }
}
