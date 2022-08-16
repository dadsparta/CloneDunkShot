using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void ActivateRb()
    {
        rb.isKinematic = false;
            
    }

    public void DeactivateRb()
    {
        if (BallStatesDatabase.IsInBasket)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
        
    }


    public Vector2 GetBallPos()
    {
        return transform.position;
    }

    public void SetBallPos(Vector2 position)
    {
        transform.position = position;
    }
}
