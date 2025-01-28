using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        private int _score;

        public void IncreaseScore(int amount)
        {
            _score += amount;
        }

    }
}