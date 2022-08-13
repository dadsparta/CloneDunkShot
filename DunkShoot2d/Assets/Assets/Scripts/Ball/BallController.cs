using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float _forceOfThrough;
    private Rigidbody2D _ballRigidbody2D;
    private void Start()
    {
        _ballRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ThroughOfBall(Rigidbody2D rigidbody2D, float forceOfThrough)
    {
        rigidbody2D.AddForce(new Vector2(0,forceOfThrough), ForceMode2D.Impulse);
    }
    
}
