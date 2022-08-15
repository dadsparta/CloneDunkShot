using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class BasketMeshController : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ball"))
            {
                BallStatesDatabase.IsInBasket = true;
            }
        }
        
    }
}
