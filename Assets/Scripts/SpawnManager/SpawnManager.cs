using System.Collections.Generic;
using Player;
using Unity.Netcode;
using UnityEngine;

namespace SpawnManager
{
    public class SpawnManager : NetworkBehaviour
    {
        [SerializeField] List<Transform> _spawnPoints;
        private List<Transform> _openSpawnPoints;

        private void Awake()
        {
            _openSpawnPoints = new List<Transform>(_spawnPoints);
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += SetPlayerPosition;
            }
        }

        private void SetPlayerPosition(ulong clientID)
        {
            var client = NetworkManager.Singleton.ConnectedClients[clientID].PlayerObject;
            client.transform.position = RandomSpawnPoint();
        }


        private Vector3 RandomSpawnPoint()
        {
            int index = Random.Range(0, _openSpawnPoints.Count);
            Transform spawnPoint  = _openSpawnPoints[index];
            _openSpawnPoints.RemoveAt(index);
            return spawnPoint.position;
        }
    }
}