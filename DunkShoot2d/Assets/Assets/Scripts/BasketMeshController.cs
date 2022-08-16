using Assets.Scripts.Database;
using UnityEngine;

namespace Assets.Scripts
{
    public class BasketMeshController : MonoBehaviour
    {

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ball"))
            {
                if (!BasketDataBase.IsFirstBasket)
                {
                    GameManager.instance.backgroundAnimator.Play("BackgroundChange");
                }   
                BallStatesDatabase.IsInBasket = true;

            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                BallStatesDatabase.IsInBasket = false;
            }
        }
    }
}
