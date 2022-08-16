using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    #region Parameters



    public static GameManager instance;
    
    [SerializeField] private BallController ball;
    [SerializeField] private TrajectoryController _trajectory;
    [SerializeField] private GameObject basketPrefab;
    [SerializeField] private Transform basketParent;
    [SerializeField] private GameOverController _gameOverController;

    [Header("Backgrounds")] 
    [SerializeField] private GameObject _whiteBackground;
    [SerializeField] private GameObject _blackBackground;

    [Header("ChangeBackgroundButtons")] 
    [SerializeField] private GameObject _whiteChangeButton;
    [SerializeField] private GameObject _blackChangeButton;

    [Header("AudioSources")] 
    [SerializeField] private AudioClip _hoopClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _basketSpawnClip;

    [Header("UI")] 
    [SerializeField] private GameObject _gameStart;
    [SerializeField] private GameObject _mainUI;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _deathMenu;
    
    [SerializeField]private TMP_Text _counter;

    public float cameraOffset;

    private float _forceOffset;
    private float _distance;
    private float _minSpawnBasketX;
    private float _maxSpawnBasketX;
    private Camera _mainCamera;
    private Vector2 _startPos;
    private Vector2 _endPos;
    private Vector2 _direction;
    private Vector2 _ballForce;
    private Vector2 _camPos;
    private Vector2 _gameOverLinePos;

    private int _indexOfBaskets;

    private float _basketOffsetX;
    public float basketOffsetY;

    public List<GameObject> basketList = new List<GameObject>();

    private Vector2 _ballStartPos;
    private Vector3 _cameraStartPos;
    private Vector2 _gameOverLineStartPos;

    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        _indexOfBaskets = 0;
        cameraOffset = (_camPos - ball.GetBallPos()).y;
        _forceOffset = 10f;
        _minSpawnBasketX = -1.38f;
        _maxSpawnBasketX = 2.14f;
        _mainCamera = Camera.main;
        ball.ActivateRb();

        SetDefaultGamePos();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BallStatesDatabase.IsInBasket = false;
            OnDragStart();
            if (GameStateDatabase.IsStartGame)
            {
                OffStartScreen();
                GameStateDatabase.IsStartGame = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            BallStatesDatabase.IsBallDragging = false;
            BallStatesDatabase.IsFirstBasket = false;
            BallStatesDatabase.IsInFly = true;
            OnDragEnd();
        }

        if (BallStatesDatabase.IsBallDragging)
        {
            OnDrag();
        }

        if (!BallStatesDatabase.IsInFly)
        {
            if (BallStatesDatabase.IsInBasket)
            {
                ball.DeactivateRb();
            }
        }
    }

    private void OnDragStart()
    {
        BallStatesDatabase.IsInDragArea = false;
        _startPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (_startPos.y < ball.GetBallPos().y)
        {
            BallStatesDatabase.IsInDragArea = true;
            BallStatesDatabase.IsBallDragging = true;
            _trajectory.Show();
        }
    }


    private void OnDrag()
    {
        _endPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _direction = (_startPos - _endPos).normalized;
        _distance = Vector2.Distance(_startPos, _endPos);

        _ballForce = _distance * _direction * _forceOffset;

        _trajectory.UpdatePoints(ball.GetBallPos(), _ballForce);
    }


    private void OnDragEnd()
    {
        if (BallStatesDatabase.IsInDragArea)
        {
            ball.ActivateRb();
            ball.AddForce(_ballForce);
            _audioSource.PlayOneShot(_hoopClip);
            _trajectory.Hide();
        }
    }

    private void LateUpdate()
    {
        _camPos.y = ball.GetBallPos().y + cameraOffset;
        Vector3 smoothPos = Vector3.Lerp(_mainCamera.transform.position, _camPos, 0.05f);
        smoothPos.z = -10f;
        _mainCamera.transform.position = smoothPos;
    }

    public void GameOver(GameObject deathMenu, GameObject pauseMenu)
    {
        DeathMenuShow(pauseMenu,deathMenu);
        Time.timeScale = 0;

    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        BasketDataBase.IsFirstBasket = true;
        ScoreStateDatabase.Score = 0;
        GameStateDatabase.IsStartGame = true;
        SceneManager.LoadScene(0);
        SetDefaultGamePos();
    }

    public void UpdateBasket()
    {
        {
            if (!BasketDataBase.IsFirstBasket)
            {
                float basketSpawnX = Random.Range(_minSpawnBasketX, _maxSpawnBasketX);
                Vector2 basketPos = new Vector2(basketSpawnX, (ball.GetBallPos().y + basketOffsetY));
                basketList[_indexOfBaskets].GetComponentInChildren<BasketController>().UpdateBasket(basketPos);
                _audioSource.PlayOneShot(_basketSpawnClip);
                _counter.text = ScoreStateDatabase.Score.ToString();

                if (_indexOfBaskets == 0)
                {
                    _indexOfBaskets = 1;
                }
                else
                {
                    _indexOfBaskets = 0;
                }
            }

            BasketDataBase.IsFirstBasket = false;
        }

    }

    private void SetDefaultGamePos()
    {
        _ballStartPos = ball.GetBallPos();
        _cameraStartPos = _mainCamera.transform.position;
        _cameraStartPos.z = -10f;
        _gameOverLineStartPos = _gameOverController.GetPosition();

    }

    private void OffStartScreen()
    {
        _gameStart.SetActive(false);
        _mainUI.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _mainUI.SetActive(false);
        _pauseMenu.SetActive(true);

    }

    public void ContineGame()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _mainUI.SetActive(true);
    }

    public void DeathMenuShow(GameObject mainMenu, GameObject deathMenu)
    {
        mainMenu.SetActive(false);
        deathMenu.SetActive(true);   
    }

    public void ChangeColorOfBackgroundOnBlack()
    {
        _blackBackground.SetActive(true);
        _whiteBackground.SetActive(false);
        _whiteChangeButton.SetActive(false);
        _blackChangeButton.SetActive(true);
    }

    public void ChangeColorOfBackgroundOnWhite()
    {
        _blackBackground.SetActive(false);
        _whiteBackground.SetActive(true);
        _blackChangeButton.SetActive(false);
        _whiteChangeButton.SetActive(true);
    }

}
