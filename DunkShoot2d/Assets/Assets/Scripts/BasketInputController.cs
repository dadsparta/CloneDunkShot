using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasketInputController : MonoBehaviour
{
    public static BasketInputController instance;
    public UnityEvent signal;
    [SerializeField] private TrajectoryController _basket;
    private BasketController _previousBasket;
    private BasketController _currentBasket;
    private float _minY = 3f;
    private float _maxY = 5f;
    private float _minX = 0.5f;
    private float _maxX = 2f;
    private void Awake()
    {
        if (!instance) instance = this;
    }

    public void SetPreviousBasket(BasketController b)
    {
        _previousBasket = b;
    }

    public void RemoveAndSpawnBasket(BasketController b)
    {
        if (_previousBasket && b != _previousBasket)
        {
            signal?.Invoke();
            var posPrev = _previousBasket.transform.position;
            Destroy(_previousBasket.transform.root.gameObject);
            var rand = Random.Range(_minY, _maxY);
            var randX = Random.Range(_minX, _maxX);
            if (posPrev.x < 0) randX = -randX;
            var pos = new Vector2(randX, posPrev.y + rand);
            Instantiate(_basket, pos, Quaternion.identity);
        }
    }
}
