using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private int _score;
        public int _dmgTaken;
        
        public void IncreaseScoreRPC(int amount)
        {
            _score += amount;
        }

        public void TakeDamageRPC(int amount)
        {
            _dmgTaken += amount;
        }
    }
}