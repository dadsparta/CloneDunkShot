using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBasketFinder : MonoBehaviour
{
    [SerializeField] private GameObject _Basket;

    private BasketController _basketController;

    private void Start()
    {
        _basketController = _Basket.GetComponent<BasketController>();
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject == col.gameObject.CompareTag("Ball"))
        {
            _basketController.IsReadyToJump = true;
            _basketController._activeBasket = _Basket;
            
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject == col.gameObject.CompareTag("Ball"))
        {
            _basketController.IsReadyToJump = false;
            _basketController._activeBasket = null;
        }
    }
}
