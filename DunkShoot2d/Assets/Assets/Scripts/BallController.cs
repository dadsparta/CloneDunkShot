using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private CameraTarget _target;
    [SerializeField] private ScoreLabel _scoreLabel;
    [SerializeField] private AudioClip _clip1;
    [SerializeField] private AudioClip _clip2;
    [SerializeField] private AudioClip _clip3;
    [SerializeField] private AudioSource _audio;
    public DistanceJoint2D DistanceJoint2D { get; private set; }
    private Rigidbody2D _rb; 
    private float _posY;
    private string _bonus = "";
    private int _score = 2;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        DistanceJoint2D = GetComponent<DistanceJoint2D>();
        BasketInputController.instance.signal.AddListener(SpawnInit);
    }

    public void Fire(Vector2 speed)
    {
        DistanceJoint2D.enabled = false;
        DistanceJoint2D.connectedBody = null;
        _posY = transform.position.y;
        _rb.AddForce(speed, ForceMode2D.Impulse);
        _posY = transform.position.y;
        _audio.PlayOneShot(_clip1);
    }



    public void Losed()
    {
        _target.transform.parent = null;
        GameManager.instance.LosedGame();
    }

    private void SpawnScoreLabel(string s)
    {
        var label = Instantiate(_scoreLabel, transform.position, Quaternion.identity);
        label.Init(s);
    }
    private async void SpawnInit()
    {
        if (_bonus != "")
        {
            SpawnScoreLabel(_bonus);
            await Task.Delay(1000);
        }
        SpawnScoreLabel(_score.ToString());
        GameManager.instance.AddScore(_score);
        _bonus = "";
        _score = 2;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<CameraBorder>())
        {
            _score += 2;
            _bonus = "Bonus";
            _audio.PlayOneShot(_clip2);
        }
        else if (other.gameObject.CompareTag("basket"))
        {
            if (_score == 2)
                _score -= 1;
            _audio.PlayOneShot(_clip2);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Basket>()) 
            _audio.PlayOneShot(_clip3);
    }
}
