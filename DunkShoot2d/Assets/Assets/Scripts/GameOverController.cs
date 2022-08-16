using UnityEngine;

namespace Assets.Scripts
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _mainUI;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameManager.instance.GameOver(_pauseMenu,_mainUI);
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
