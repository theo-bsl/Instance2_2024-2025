using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Lobby
{
    public class TimeManager : NetworkBehaviour
    {
        [SerializeField] private float _gameCloseLobbyTime = 600f;
        private readonly UnityEvent _onTimerFinished = new();
        private readonly UnityEvent<int,int> _onUpdateUI = new();
        private int _minutes;
        private int _seconds;
        
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
                if (_gameCloseLobbyTime >= 0)
                    _gameCloseLobbyTime -= Time.deltaTime;
                _minutes = Mathf.FloorToInt(_gameCloseLobbyTime / 60f);
                _seconds = Mathf.FloorToInt(_gameCloseLobbyTime - _minutes * 60f);
                _onUpdateUI.Invoke(_minutes, _seconds);
            }
        }
        
        public UnityEvent OnTimerFinished => _onTimerFinished;
        public UnityEvent<int, int> OnUpdateUI => _onUpdateUI;
    }
}