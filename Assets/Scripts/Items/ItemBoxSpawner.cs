using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class ItemBoxSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject itemBoxPrefab;
        [SerializeField] private List<Transform> spawnPoints;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                foreach (var spawnPoint in spawnPoints)
                {
                    CreateItemBox(spawnPoint.position);
                }
            }
        }

        private void CreateItemBox(Vector3 position)
        {
            GameObject itemBox = Instantiate(itemBoxPrefab, position, quaternion.identity);
            itemBox.GetComponent<NetworkObject>().Spawn(true);
            itemBox.GetComponent<ItemDispenser>().OnDestroy.AddListener(RespawnItemBox);
        }

        private void RespawnItemBox(float timeBeforeRespawn, Vector3 position)
        {
            StartCoroutine(RespawnItemBoxCoroutine(timeBeforeRespawn, position));
        }

        private IEnumerator RespawnItemBoxCoroutine(float timeBeforeRespawn, Vector3 position)
        {
            yield return new WaitForSeconds(timeBeforeRespawn);
            CreateItemBox(position);
        }
    }
}