using Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Lobby
{
    public class ScoreManager : NetworkBehaviour
    {
        [SerializeField] private int _maxScore = 10000;
        private readonly UnityEvent _onScoreMax = new();
        
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
            }
        }

        public void ManageNewPlayer(ulong id)
        {
            var r = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
            PlayerManager player = r.GetComponentInChildren<PlayerManager>();

            player.Score.OnValueChanged += (_, currentScore) => ManageScore(currentScore);
        }

        private void ManageScore(int currentScore)
        {
            if (currentScore >= _maxScore)
            {
                _onScoreMax.Invoke();
            }
        }


        public UnityEvent OnScoreMax => _onScoreMax;
    }
}