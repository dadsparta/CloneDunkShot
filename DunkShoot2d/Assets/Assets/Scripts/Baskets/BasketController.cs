using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    private BallController _ball;

    private void Start()
    {
        _ball = GameObject.FindWithTag("Player").GetComponent<BallController>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == col.gameObject.CompareTag("Player"))
        {
            _ball.SetBasket(gameObject);
        }
    }
}
