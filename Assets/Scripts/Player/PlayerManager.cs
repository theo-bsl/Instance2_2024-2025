using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private int _score;
        public NetworkVariable<int> _dmgTaken = new(0);
        
        public void IncreaseScoreRPC(int amount)
        {
            _score += amount;
        }

        [Rpc(SendTo.Server)]
        public void TakeDamageRPC(int amount)
        {
            Debug.Log(amount);
            _dmgTaken.Value += amount;
        }
    }
}