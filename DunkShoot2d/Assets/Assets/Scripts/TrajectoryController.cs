using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class TrajectoryController : MonoBehaviour
    {
    
        [SerializeField] private GameObject pointPrefab;
        [SerializeField] private Transform pointParent;

        List<Transform> pathPoints;

        public int maxPoints = 20;
        public float offset = 0.5f;
        private float _deltaTime;
        private Vector2 _pointPos;


        private void Start()
        {
            pathPoints = new List<Transform>();
            Hide();
            for (int i = 0; i < maxPoints; i++)
            {
                Transform tr = Instantiate(pointPrefab).transform;
                tr.SetParent(pointParent);
                pathPoints.Add(tr);
            }
        }

        public void UpdatePoints(Vector2 startPos , Vector2 force)
        {
            _deltaTime = offset;

            for (int i = 0; i < maxPoints; i++)
            {
                _pointPos.x = startPos.x + force.x * _deltaTime;
                _pointPos.y = startPos.y + force.y * _deltaTime - Physics2D.gravity.magnitude * _deltaTime * _deltaTime * 0.5f;

                pathPoints[i].position = _pointPos;

                _deltaTime += offset;
            }
        }


        public void Hide()
        {
            pointParent.gameObject.SetActive(false);
        }

        public void Show()
        {
            if (!BallStatesDatabase.IsInBasket)
            {
                pointParent.gameObject.SetActive(true);
            }
        }
    }
}
