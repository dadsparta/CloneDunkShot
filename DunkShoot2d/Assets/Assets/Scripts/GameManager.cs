using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Database;
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

    public float cameraOffset;

    private float _forceOffset;
    private float _distance;
    private float _minSpawnBacketX;
    private float _maxSpawnBacketX;
    private Camera _mainCamera;
    private Vector2 _startPos;
    private Vector2 _endPos;
    private Vector2 _direction;
    private Vector2 _ballForce;
    private Vector2 _camPos;
    private Vector2 _gameOverLinePos;
    
    private int _indexOfBaskets = 0;
    private float _basketOffsetX;
    public float basketOffsetY;

    public List<GameObject> basketList = new List<GameObject>();
    
    private Vector2 _ballStartPos;
    private Vector3 _cameraStartPos;
    private Vector2 _gameOverLineStartPos;

    #endregion

    private void Awake()
    {
        if(instance == null)
        instance = this;    
    }

    private void Start()
    {
        _indexOfBaskets = 0;
        cameraOffset = (_camPos - ball.GetBallPos()).y;
        _forceOffset = 5f;
        _minSpawnBacketX = -1.38f;
        _maxSpawnBacketX = 2.14f;
        _mainCamera = Camera.main;
        ball.ActivateRb();
        
        SetDefaultGamePos();
    }


    private void Update()
    {
        if (GameStateDatabase.IsGameOver)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            BallStatesDatabase.IsInBasket = false;
            ball.DeactivateRb();
            OnDragStart();
        }
        if(Input.GetMouseButtonUp(0))
        {
            BallStatesDatabase.IsBallDragging = false;
            BallStatesDatabase.IsFirstBasket = false;
            OnDragEnd();
        }

        if(BallStatesDatabase.IsBallDragging)
        {
            OnDrag();
        }

        if(BallStatesDatabase.IsInBasket)
        {
            ball.DeactivateRb();
        }
    }

    // Starting the mouse drag.
    private void OnDragStart()
    {
        BallStatesDatabase.IsInDragArea = false;
        ball.DeactivateRb();
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
        _distance = Vector2.Distance(_startPos,_endPos);

        //Calculating force applyimg to the ball.
        _ballForce = _distance * _direction * _forceOffset;

        _trajectory.UpdatePoints(ball.GetBallPos(), _ballForce);
    }


    private void OnDragEnd()
    {
        if (BallStatesDatabase.IsInDragArea)
        {
            ball.ActivateRb();
            ball.AddForce(_ballForce);
            _trajectory.Hide();
        }
    }

    private void LateUpdate()
    {
        if (GameStateDatabase.IsGameOver)
            return;
        _camPos.y = ball.GetBallPos().y + cameraOffset;
        Vector3 smoothPos = Vector3.Lerp(_mainCamera.transform.position, _camPos, 0.05f);
        smoothPos.z = -10f;
        _mainCamera.transform.position = smoothPos;
    }

    public void GameOver()
    {
        GameStateDatabase.IsGameOver = true;
        SceneManager.LoadScene(0);
        GameStateDatabase.IsGameOver = false;
        SetDefaultGamePos();
    }

    public void UpdateBasket()
    {
        Debug.Log(BasketDataBase.RepeatingOfBasket0 + "" + BasketDataBase.RepeatingOfBasket1);
        if (_indexOfBaskets == 0)
        {
            if (BasketDataBase.RepeatingOfBasket0 == 0)
            {
                BasketDataBase.RepeatingOfBasket0 = 1;
                BasketDataBase.RepeatingOfBasket1 = 0;
                
                Debug.Log("Indexxxx :: "+_indexOfBaskets);
                float basketSpawnX = Random.Range(_minSpawnBacketX, _maxSpawnBacketX);
                Vector2 basketPos = new Vector2(basketSpawnX, (ball.GetBallPos().y + basketOffsetY));
                basketList[1].GetComponentInChildren<BasketController>().UpdateBasket(basketPos);
                
                if (_indexOfBaskets == 0)
                {
                    _indexOfBaskets = 1;
                }
                else
                {
                    _indexOfBaskets = 0;
                }
            }
        }
        
        else if (_indexOfBaskets == 1)
        {
            if (BasketDataBase.RepeatingOfBasket1 == 0)
            {
                BasketDataBase.RepeatingOfBasket0 = 0;
                BasketDataBase.RepeatingOfBasket1 = 1;
                
                Debug.Log("Indexxxx :: "+_indexOfBaskets);
                float basketSpawnX = Random.Range(_minSpawnBacketX, _maxSpawnBacketX);
                Vector2 basketPos = new Vector2(basketSpawnX, (ball.GetBallPos().y + basketOffsetY));
                basketList[0].GetComponentInChildren<BasketController>().UpdateBasket(basketPos);
                
                if (_indexOfBaskets == 0)
                {
                    _indexOfBaskets = 1;
                }
                else
                {
                    _indexOfBaskets = 0;
                }
            }
            
        }
        
    }


    private void ResetBasket()
    {
        for (int i = 0; i < basketList.Count; i++)
        {
            basketList[i].GetComponentInChildren<BasketController>().DisableBasket();
        }
    }

    public void OnButtonReplayPressed()
    {
        ResetBasket();
        _indexOfBaskets = 0;
        GameStateDatabase.IsGameOver = false;
        _mainCamera.transform.position = _cameraStartPos;
        ball.SetBallPos(_ballStartPos);
        _gameOverController.SetPosition(_gameOverLineStartPos);
        UpdateBasket();
    }


    private void SetDefaultGamePos()
    {
        _ballStartPos = ball.GetBallPos();
        _cameraStartPos = _mainCamera.transform.position;
        _cameraStartPos.z = -10f;
        _gameOverLineStartPos = _gameOverController.GetPosition();

    }
}
