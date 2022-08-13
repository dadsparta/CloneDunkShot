using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketCollisionController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == col.gameObject.CompareTag("Ball"))
        {
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
