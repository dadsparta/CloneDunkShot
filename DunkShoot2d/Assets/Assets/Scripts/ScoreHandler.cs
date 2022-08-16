using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class ScoreHandler : MonoBehaviour
    {
        public static ScoreHandler instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

    }
}
