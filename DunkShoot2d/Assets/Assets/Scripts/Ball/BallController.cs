using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallController : MonoBehaviour
{
     [SerializeField] private Rigidbody2D _pushRigidbody2D;
     [SerializeField] private float _pushArea = 1f;
     [SerializeField] private float _jumpPower = 4f;
     [SerializeField]private UnityEvent _reached;

     [SerializeField]private GameObject _basket;
     private Rigidbody2D _rigidbody2D;
     private void Start()
     {
          _rigidbody2D = GetComponent<Rigidbody2D>();
     }

     private void Update()
     {
          if (BallStateController.IsPressed)
          {
               Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               Vector3 mouseRot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               if (Vector2.Distance(mousePos, _pushRigidbody2D.position) > _pushArea)
               {
                    _rigidbody2D.position =
                         _pushRigidbody2D.position + (mousePos - _pushRigidbody2D.position).normalized * _pushArea;
               }
               else
               {
                    transform.LookAt(mouseRot);
                    _basket.transform.rotation = gameObject.transform.rotation;
                    _rigidbody2D.position = mousePos;
               }

          }
     }

     public void OnMouseDown()
     {
          _reached?.Invoke();
          BallStateController.IsPressed = true;
          _rigidbody2D.isKinematic = true;
     }

     public void OnMouseUp()
     {
          BallStateController.IsPressed = false;
          _rigidbody2D.isKinematic = false;

          StartCoroutine(FlyOfBall());
     }

     IEnumerator FlyOfBall()
     {
          yield return new WaitForSeconds(0.1f);
          if (BallStateController.IsPressed)
          {
               _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
          }
          gameObject.GetComponent<SpringJoint2D>().enabled = false;
          this.enabled = false;
     }

     public void SetBasket(GameObject basket)
     {
          _basket = basket;
     }
}
