using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private NetworkVariable<int> _score = new(0);
        public NetworkVariable<int> _dmgTaken = new(0);
        public void IncreaseScoreRPC(int amount)
        {
            _score.Value += amount;
        }

        [Rpc(SendTo.Server)]
        public void TakeDamageRPC(int amount)
        {
            Debug.Log(amount);
            _dmgTaken.Value += amount;
            //_dmgTaken += amount;

            // if (_dmgTaken >= 100)
            // {
            //     transform.position = Vector3.zero;
            // }
        }
        
        public NetworkVariable<int> Score => _score;
    }
}