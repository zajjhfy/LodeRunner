using UnityEngine;
using UnityEngine.UI;

public class ScoreField : MonoBehaviour
{
    public static ScoreField Instance {get;private set;}
    private Text _scoreText;
    private int _scorePoints = 0;

    private void Start(){
        if(Instance == null){
            Instance = this;
        }
        _scoreText = GetComponent<Text>();
        _scoreText.text = $"SCORE {_scorePoints:d4}";
    }

    public void UpdateScore(){
        _scorePoints += 100;
        _scoreText.text = $"SCORE {_scorePoints:d4}";
    }
}
