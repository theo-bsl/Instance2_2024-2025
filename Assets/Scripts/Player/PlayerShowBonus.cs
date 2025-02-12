using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerShowBonus : NetworkBehaviour
    {
        [SerializeField] PlayerManager _playerManager;
        [SerializeField] private GameObject _showBonus;
        [SerializeField] private GameObject _currentItem;
        [SerializeField] private Image _bonusImage;
        [SerializeField] private Image _malusImage;
        [SerializeField] private Image _currentItemImage;
        [SerializeField] private List<Sprite> _bonusSprites = new List<Sprite>();
        [SerializeField] private List<Sprite> _malusSprites = new List<Sprite>();
        
        public override void OnNetworkSpawn()
        {
            _playerManager.OnUseItem.AddListener((itemName => ShowItemRpc(itemName)));
            _playerManager.OnEndItem.AddListener((itemName => HideItemRpc(itemName)));
            _playerManager.OnGetItem.AddListener((itemName => ShowCurrentItemRpc(itemName)));
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void ShowCurrentItemRpc(string itemName)
        {
            if (itemName == "FreezeGun")
            {
                _currentItem.SetActive(false);
            }
            if (itemName == "SpeedUp")
            {
                _currentItem.SetActive(true);
                _currentItemImage.sprite = _bonusSprites[0];
            }

            if (itemName == "DamageUp")
            {
                _currentItem.SetActive(true);
                _currentItemImage.sprite = _bonusSprites[1];
            }
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void ShowItemRpc(string itemName)
        {
            _currentItem.SetActive(false);
            if (itemName == "SpeedUp")
            {
                _showBonus.SetActive(true);
                _bonusImage.sprite = _bonusSprites[0];
                _malusImage.sprite = _malusSprites[0];
            }

            if (itemName == "DamageUp")
            {
                _showBonus.SetActive(true);
                _bonusImage.sprite = _bonusSprites[1];
                _malusImage.sprite = _malusSprites[1];
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void HideItemRpc(string itemName)
        {
            if (itemName == "SpeedUp")
                _showBonus.SetActive(false);

            if (itemName == "DamageUp")
                _showBonus.SetActive(false);
        }
    }
}