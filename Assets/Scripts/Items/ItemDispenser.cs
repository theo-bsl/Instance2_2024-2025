using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    public class ItemDispenser : NetworkBehaviour
    {
        [SerializeField] private List<ItemPercent> items;

        public GameObject GetItem()
        {
            foreach (var item in items)
            {
                if (item.Percent >= Random.value * 100f)
                    return item.Item;
            }
            
            GetComponent<NetworkObject>().Despawn();
            
            return items[^1].Item;
        }

        [Serializable]
        private struct ItemPercent
        {
            public GameObject Item;
            
            [Range(0, 100)]public float Percent;
        }
    }
}