using System.Collections.Generic;
using UnityEngine;

namespace SpawnManager
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] List<Transform> _spawnPoints;
        private List<Transform> _openSpawnPoints;

        void Awake()
        {
            _openSpawnPoints = new List<Transform>(_spawnPoints);
        }
        
        public Transform RandomSpawnPoint()
        {
            int index = Random.Range(0, _openSpawnPoints.Count);
            Transform spawnPoint  = _openSpawnPoints[index];
            _openSpawnPoints.RemoveAt(index);
            return spawnPoint;
        }
    }
}