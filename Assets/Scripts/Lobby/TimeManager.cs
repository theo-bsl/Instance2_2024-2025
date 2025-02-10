using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Lobby
{
    public class TimeManager : NetworkBehaviour
    {
        [SerializeField] private float _gameCloseLobbyTime = 600;
        [SerializeField] private float _gameTimer;
        private readonly UnityEvent _onTimerFinished = new();
        
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if(IsServer)
            {
                _gameTimer += Time.deltaTime;
                if (_gameTimer >= _gameCloseLobbyTime)
                {
                    _onTimerFinished.Invoke();
                }
            }
        }
        
        public UnityEvent OnTimerFinished => _onTimerFinished;
    }
}