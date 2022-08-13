using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasketController : MonoBehaviour
{
     [SerializeField] private float _offset;
     [SerializeField]private Rigidbody2D _ballRigidbody;
     [SerializeField] private float ThroughForce;

     private BallController _ballController;
     private void Start()
     {
          _ballController = GameObject.FindWithTag("Ball").GetComponent<BallController>();     
          _ballRigidbody = GameObject.FindWithTag("Ball").GetComponent<Rigidbody2D>();
     }

     private void Update()
     {
          if (BallStateController.IsPressed)
          {
               Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
               float rotationZ = Mathf.Atan2(worldMousePosition.y, worldMousePosition.x) * Mathf.Rad2Deg;
               transform.rotation = Quaternion.Euler(0f,0f, rotationZ + _offset);
               
               Vector3 localScale = Vector3.one;

               transform.localScale = localScale;
          }
     }

     private void OnMouseDown()
     {
          BallStateController.IsPressed = true;
     }

     private void OnMouseUp()
     {
          BallStateController.IsPressed = false;
          _ballController.ThroughOfBall(_ballRigidbody,BallParametersDatabase.ForceOfThrough);
          BallStateController.IsInBasket = false;
     }

     private void OnCollisionEnter2D(Collision2D col)
     {
          if (col.gameObject == col.gameObject.CompareTag("Ball"))
          {
               BallStateController.IsInBasket = true;
          }
     }
}
