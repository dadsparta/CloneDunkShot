using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts
{
    public class TouchController : MonoBehaviour
    { 
        [SerializeField] private GameObject counter;
        private CounterController _counterController;

        private void Start()
        {
            _counterController = counter.GetComponent<CounterController>();
        }

        private void OnMouseDown()
        {
            ScoreDataBase.Score++;
            _counterController.ChangeCounter();
            
        }
    }
}
