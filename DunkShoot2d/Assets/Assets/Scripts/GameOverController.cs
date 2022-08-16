using UnityEngine;

namespace Assets.Scripts
{
    public class GameOverController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameManager.instance.GameOver();
        }


        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }
    }
}
