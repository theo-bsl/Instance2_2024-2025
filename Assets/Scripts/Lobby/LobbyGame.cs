using System.Linq;
using Leaderboard;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Lobby
{
    public class LobbyGame : NetworkBehaviour
    {
        [SerializeField] private LeaderboardManager _leaderboard;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private SpawnManager _spawnManager;
        
        public override void OnNetworkSpawn()
        {
            if (IsServer)
                NetworkManager.Singleton.OnClientConnectedCallback += ManageNewPlayer;
            else
                enabled = false;
            _scoreManager.OnScoreMax.AddListener(CloseLobby);
            _timeManager.OnTimerFinished.AddListener(CloseLobby);
        }
        
        private void ManageNewPlayer(ulong id)
        {
            _leaderboard.AddNewPlayerID(id);
            _scoreManager.ManageNewPlayer(id);
            _spawnManager.ManageNewPlayer(id);
        }

        private void CloseLobby()
        {
            var spawnedObjects = NetworkManager.Singleton.SpawnManager.SpawnedObjectsList.ToList();
                    
            for (var i = spawnedObjects.Count - 1; i >= 0; i--)
            {
                spawnedObjects[i].Despawn();
            }
                    
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}