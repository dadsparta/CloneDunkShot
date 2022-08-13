using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketCollisionController : MonoBehaviour
{
    private GameObject _bell;

    private void Start()
    {
        _bell = GameObject.FindWithTag("Ball");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == col.gameObject.CompareTag("Ball"))
        {
            _bell.transform.rotation = gameObject.transform.rotation;
            BallStateController.IsInBasket = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject == col.gameObject.CompareTag("Ball"))
        {
            BallStateController.IsInBasket = false;
        }
    }
}
