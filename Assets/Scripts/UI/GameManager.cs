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
    [SerializeField] private GameObject _loseGameObj;
    [SerializeField] private GameObject _winGameObj;
    [SerializeField] private ScoreField _scoreField;

    [Header("SpawnSystem")]
    [SerializeField] private GameObject[] _enemySpawnIds;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Sprite _spawnSprite;

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

    public void NextLevelGame()
    {
        _winGameObj.SetActive(false);
        _loseGameObj.SetActive(false);
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if(index > 3){
            ExitToMainMenu();
        }
        else{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void RetryGame()
    {
        _winGameObj.SetActive(false);
        _loseGameObj.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenuGame()
    {
        _winGameObj.SetActive(false);
        _loseGameObj.SetActive(false);
        ExitToMainMenu();
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
        _controller.DisableInputMap();
        SoundManager.PlaySound(SoundType.Win, 0.5f);
        _winGameObj.SetActive(true);
        Time.timeScale = 0;
    }

    private void LoseGame(object sender, EventArgs e){
        _controller.DisableInputMap();
        SoundManager.PlaySound(SoundType.Lose);
        _loseGameObj.SetActive(true);
        Time.timeScale = 0;
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
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ProcessEnemyDeath(int id){
        _scoreField.UpdateScore();
        StartCoroutine((WaitForEnemySpawn(id)));
    }

    private IEnumerator WaitForEnemySpawn(int id){
        var spawnSprite = _enemySpawnIds[id].GetComponent<SpriteRenderer>();

        yield return new WaitForSeconds(6f);
        spawnSprite.sprite = _spawnSprite;

        yield return new WaitForSeconds(2f);
        spawnSprite.sprite = null;
        var enemy = Instantiate(_enemyPrefab, _enemySpawnIds[id].transform.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().Id = id;
    }

}
