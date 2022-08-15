using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

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
    private float _disatce;
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
        instance = this;    
    }

    private void Start()
    {
        _indexOfBaskets = 0;
        cameraOffset = (_camPos - ball.GetBallPos()).y;
        _forceOffset = 5f;
        _mainCamera = Camera.main;
        ball.ActivateRb();

        UpdateBasket();
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
        _disatce = Vector2.Distance(_startPos,_endPos);

        //Calculating force applyimg to the ball.
        _ballForce = _disatce * _direction * _forceOffset;

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
        ResetBasket();
        ScoreHandler.instance.OnGameOver();
    }

    public void UpdateBasket()
    {
        Debug.Log("Indexxxx :: "+_indexOfBaskets);
        Vector2 basketPos = new Vector2(basketList[_indexOfBaskets].transform.position.x, (ball.GetBallPos().y + basketOffsetY));
        basketList[_indexOfBaskets].GetComponentInChildren<BasketController>().UpdateBasket(basketPos);

        if (_indexOfBaskets == 0)
        {
            _indexOfBaskets = 1;
        }
        else
        {
            _indexOfBaskets = 0;
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

    public void UpdateGameOverLine()
    {
        _gameOverLinePos = new Vector2(_gameOverController.GetPosition().x, (ball.GetBallPos().y - basketOffsetY*2));
        _gameOverController.SetPosition(_gameOverLinePos);
    }

    
    private void SetDefaultGamePos()
    {
        _ballStartPos = ball.GetBallPos();
        _cameraStartPos = _mainCamera.transform.position;
        _cameraStartPos.z = -10f;
        _gameOverLineStartPos = _gameOverController.GetPosition();

    }
}
