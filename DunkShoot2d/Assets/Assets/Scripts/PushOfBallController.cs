using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushOfBallController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider2D;
    public float Force { get; private set; }
    private Vector2 _touchStarted;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
        
        InputManager.instance.OnMouseDown.AddListener(StartToch);
        InputManager.instance.OnMouseDrag.AddListener(CalculateForce);
        InputManager.instance.OnMouseUp.AddListener(EnabledCollider);
    }
    private void StartToch()
    {
        _touchStarted = Input.mousePosition;
    }

    private void CalculateForce()
    {
        Vector2 touchEnded = Input.mousePosition;
        var swipe = touchEnded - _touchStarted;
        Force = swipe.magnitude / 100;
    }

    private void EnabledCollider()
    {
        _collider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out BallController ball))
            ball.Losed();
    }
}
