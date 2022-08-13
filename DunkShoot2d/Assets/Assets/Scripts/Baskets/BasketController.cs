using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasketController : MonoBehaviour
{
     [SerializeField] private float _offset;
     [SerializeField] private Rigidbody2D _ballRigidbody;
     [SerializeField] public GameObject _activeBasket;
     

     private BallController _ballController;
     public bool IsReadyToJump;
     private Transform _throughTransform;
     private void Start()
     {
          _ballController = GameObject.FindWithTag("Ball").GetComponent<BallController>();     
          _ballRigidbody = GameObject.FindWithTag("Ball").GetComponent<Rigidbody2D>();
     }

     private void Update()
     {
          if (BallStateController.IsPressed)
          {
               if (BallStateController.IsInBasket)
               {
                    if (IsReadyToJump)
                    {
                         Vector3 worldMousePosition = Camera.main.
                              ScreenToWorldPoint(Input.mousePosition) - transform.position;
                         float rotationZ = Mathf.Atan2(worldMousePosition.y, worldMousePosition.x) * Mathf.Rad2Deg;
                         _activeBasket.transform.rotation = Quaternion.Euler(0f,0f, rotationZ + _offset);
                         Vector3 localScale = Vector3.one;
                         _activeBasket.transform.localScale = localScale;
                         _throughTransform = _activeBasket.transform;

                    }
               }
          }
     }

     private void OnMouseDown()
     {
          BallStateController.IsPressed = true;
     }

     private void OnMouseUp()
     {
          BallStateController.IsPressed = false;
          if (BallStateController.IsInBasket)
          {
               _ballController.ThroughOfBall(_ballRigidbody,
                    BallParametersDatabase.ForceOfThrough,_throughTransform.rotation);
          }
          BallStateController.IsInBasket = false;
     }
}
