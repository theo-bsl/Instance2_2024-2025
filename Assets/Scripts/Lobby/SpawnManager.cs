using System.Collections.Generic;
using Player;
using Unity.Netcode;
using UnityEngine;

namespace Lobby
{
    public class SpawnManager : NetworkBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoints;
        private List<Transform> _openSpawnPoints;

        private void Awake()
        {
            _openSpawnPoints = new List<Transform>(_spawnPoints);
        }

        public void ManageNewPlayer(ulong id)
        {
            var client = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
            SetPlayerPosition(client);
            client.GetComponentInChildren<PlayerBurst>().OnBurstedEvent.AddListener(RespawnPlayer);
        }

        private void SetPlayerPosition(NetworkObject client)
        {
            client.GetComponentInChildren<PlayerManager>().transform.position = RandomSpawnPoint();
        }


        private Vector3 RandomSpawnPoint()
        {
            int index = Random.Range(0, _openSpawnPoints.Count);
            Transform spawnPoint  = _openSpawnPoints[index];
            _openSpawnPoints.RemoveAt(index);
            
            if (_openSpawnPoints.Count == 0)
                ResetSpawnPoints();
            
            return spawnPoint.position;
        }

        private void ResetSpawnPoints()
        {
            _openSpawnPoints = new List<Transform>(_spawnPoints);
        }

        private void RespawnPlayer(PlayerBurst playerBurst)
        {
            playerBurst.BurstedTo(RandomSpawnPoint());
        }
    }
}