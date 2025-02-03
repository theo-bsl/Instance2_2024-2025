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

        public Item GetItem()
        {
            foreach (var item in items)
            {
                if (item.Percent >= Random.value * 100f)
                    return item.Item;
            }
            
            return items[^1].Item;
        }

        [Serializable]
        struct ItemPercent
        {
            public Item Item;
            
            [Range(0, 100)]public float Percent;
        }
    }
}