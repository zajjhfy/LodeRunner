using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameSceneEnter;
    [SerializeField] private GameObject _loseText;
    [SerializeField] private GameObject _winText;
    [SerializeField] private ScoreField _scoreField;

    [Header("SpawnSystem")]
    [SerializeField] private GameObject[] _enemySpawnIds;
    [SerializeField] private GameObject _enemyPrefab;

    [Header("References")]
    [SerializeField] private PlayerController _controller;
    [SerializeField] private PlayerTrigger _playerTrigger;

    public static GameManager Instance { get; private set; }

    private void Awake(){
        if(Instance == null) Instance = this;
        Time.timeScale = 0;
        _pauseMenu.SetActive(false);
        _gameSceneEnter.SetActive(true);
    }

    private void OnEnable(){
        _playerTrigger.OnGameLose += LoseGame;
        _controller.OnGameStartPressed += _controller_OnGameStartPressed;
        _controller.OnPauseButtonPressed += _controller_OnPauseButtonPressed;
        _controller.OnExitButtonPressed += _controller_OnExitButtonPressed;
        _scoreField.OnGameFinish += FinishGame;
    }

    private void FinishGame(object sender, EventArgs e)
    {
        _winText.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(ExitToMainMenuWait());
    }

    private void LoseGame(object sender, EventArgs e){
        _loseText.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(ExitToMainMenuWait());
    }

    private void _controller_OnGameStartPressed(object sender, EventArgs e)
    {
        Destroy(_gameSceneEnter);
        Time.timeScale = 1;
        _controller.EnableInputMap();
        _controller.DisableGameStartMap();
        _controller.OnGameStartPressed -= _controller_OnGameStartPressed;
    }

    private void _controller_OnExitButtonPressed(object sender, EventArgs e)
    {
        ExitToMainMenu();
    }

    private void _controller_OnPauseButtonPressed(object sender, EventArgs e)
    {
        if(_pauseMenu.activeSelf){
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        else{
            _pauseMenu.SetActive(true); 
            Time.timeScale = 0;
        }
    }

    private void DisablePauseMenu(){
        if(_pauseMenu.activeSelf){
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void OnDisable(){
        _scoreField.OnGameFinish -= FinishGame;
        _controller.OnPauseButtonPressed -= _controller_OnPauseButtonPressed;
        _controller.OnExitButtonPressed -= _controller_OnExitButtonPressed;
    }

    private void ExitToMainMenu(){
        _controller.DisableInputMap();
        DisablePauseMenu();
        SceneManager.LoadScene(0);
    }

    public void ProcessEnemyDeath(int id){
        _scoreField.UpdateScore();
        StartCoroutine((WaitForEnemySpawn(id)));
    }

    private IEnumerator WaitForEnemySpawn(int id){
        yield return new WaitForSeconds(10f);
        var enemy = Instantiate(_enemyPrefab, _enemySpawnIds[id].transform.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().Id = id;
    }

    private IEnumerator ExitToMainMenuWait(){
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        ExitToMainMenu();
    }
}
