using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Items
{
    public class ItemDispenser : NetworkBehaviour
    {
        [Header("Item Box")]
        [SerializeField] private float _respawnTime = 5f;
        
        [Header("Item Dispenser")]
        [SerializeField] private List<ItemPercent> items;
        
        private readonly UnityEvent<float, Vector3> _onDestroy = new();

        public GameObject GetItem()
        {
            return items[Random.Range(0, items.Count)].Item;
        }

        [Serializable]
        private struct ItemPercent
        {
            public GameObject Item;
            
            [Range(0, 100)]public float Percent;
        }

        public void Despawn()
        {
            _onDestroy.Invoke(_respawnTime, transform.position);
            GetComponent<NetworkObject>().Despawn();
        }
        
        public UnityEvent<float, Vector3> OnDestroy => _onDestroy;
    }
}