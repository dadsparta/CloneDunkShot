using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsController : MonoBehaviour
{
    private float _speed = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BallController>()) Init();
    }
    private void Init()
    {
        transform.parent = null;
        var point = GameManager.instance.StarPoint.position;
        var target = Camera.main.ScreenToWorldPoint(new Vector3
            (point.x, point.y, Camera.main.transform.position.z * -1));
        transform.DOMove(target, _speed).SetEase(Ease.OutBack, 1f)
            .OnComplete(() => GameManager.instance.AddStar());
        Destroy(this.gameObject, _speed);
    }
}
