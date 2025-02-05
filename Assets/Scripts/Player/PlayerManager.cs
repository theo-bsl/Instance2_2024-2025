using System;
using Leaderboard;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private int _damageLimit = 150;
        [SerializeField] private int _burstedScoreEarn = 150;
        [SerializeField] private NetworkVariable<int> _score = new(0);
        [SerializeField] private NetworkVariable<int> _dmgTaken = new(0);
        public NetworkVariable<int> _playerName = new(0);

        public override void OnNetworkSpawn()
        {
            GetComponent<PlayerAttack>().OnEnemyBursted.AddListener(() => IncreaseScoreRPC(_burstedScoreEarn));
        }
        
        [Rpc(SendTo.Server)]
        public void IncreaseScoreRPC(int amount)
        {
            _score.Value += amount;
        }

        [Rpc(SendTo.Server)]
        private void TakeDamageRPC(int amount)
        {
            _dmgTaken.Value += amount;
        }

        public bool TakeDamage(int amount)
        {
            TakeDamageRPC(amount);
            return _dmgTaken.Value >= _damageLimit;
        }
        
        public NetworkVariable<int> Score => _score;
        public NetworkVariable<int> PlayerName => _playerName;
    }
}