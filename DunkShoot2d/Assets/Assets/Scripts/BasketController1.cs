using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController1 : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D _point;
    [SerializeField] private Animation _anim;
    [SerializeField] private Star _star;
    [SerializeField] private bool isFirst;
    public Ball Player { get; private set; }

    #region Trajectory

    [SerializeField] private Transform _anchor;
    private TrajectoryController _trajectory;
    private Fire _fire;
    private float _force = 10f;
    private float _speed;
    #endregion

    #region Rotate
    private Camera _cam;
    private Vector3 _screenPos;
    private float _angleOffset;
    #endregion

    private void Start()
    {
        _cam = Camera.main;
        _trajectory = transform.root.GetComponent<Trajectory>();
        _fire = transform.root.GetComponent<Fire>();

        InputManager.instance.OnMouseDown.AddListener(RotateBasketStart);
        InputManager.instance.OnMouseDrag.AddListener(TrajectoryDrag);
        InputManager.instance.OnMouseUp.AddListener(TrajectoryEnd);
        ShowStar();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerBall player))
        {
            Player = player;
            Player.DistanceJoint2D.enabled = true;
            Player.DistanceJoint2D.connectedBody = _point;
            _anim.Play();
            BasketManager.instance.RemoveAndSpawnBasket(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerBall player))
        {
            Player = null;
        }
    }


    private void ShowStar()
    {
        if (isFirst) return;
        if (1 == Random.Range(0, 5))
        {
            _star.gameObject.SetActive(true);
        }
    }

    private void TrajectoryDrag()
    {
        if (!Player) return;
        RotateBasketDrag();
        _speed = Mathf.Clamp(_force * _fire.Force, 10f, 15f);
        var Y = Mathf.Clamp(1f + (0.16f * _fire.Force), 1f, 1.5f);
        _anchor.localScale = new Vector3(1f, Y, 1f);
        _trajectory.ShowTrajectory(transform.up * _speed);
    }

    private void TrajectoryEnd()
    {
        if (!Player) return;
        _anchor.localScale = new Vector3(1f, 1f, 1f);
        Player.Fire(transform.up * _speed);
        BasketManager.instance.SetPreviousBasket(this);
    }




    private void RotateBasketStart()
    {
        if (!Player) return;
        _screenPos = _cam.WorldToScreenPoint(transform.position);
        var vec3 = Input.mousePosition - _screenPos;
        _angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, vec3.x)) * Mathf.Rad2Deg;
    }

    private void RotateBasketDrag()
    {
        var vec3 = Input.mousePosition - _screenPos;
        var angle = Mathf.Atan2(vec3.y, vec3.x) * Mathf.Rad2Deg;
        transform.root.eulerAngles = (new Vector3(0, 0, angle + _angleOffset));
    }
}
