using System;
using System.Collections;
using Items;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private NetworkVariable<int> _score = new(0);
        public NetworkVariable<int> _dmgTaken = new(0);

        private bool _freezed;
        private Item _item;
        private PlayerMovement _playerMovement;

        public override void OnNetworkSpawn()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        public void IncreaseScore(int amount)
        {
            _score.Value += amount;
        }

        [Rpc(SendTo.Server)]
        public void TakeDamageRPC(int amount)
        {
            Debug.Log(amount);
            _dmgTaken.Value += amount;
            //_dmgTaken += amount;

            // if (_dmgTaken >= 100)
            // {
            //     transform.position = Vector3.zero;
            // }
        }

        public void UseItem()
        {
            _item?.Do();
        }

        private void ModifySpeed(float speedModifier, float duration)
        {
            StartCoroutine(ModifySpeedCoroutine(speedModifier, duration));
        }
        private IEnumerator ModifySpeedCoroutine(float speedModifier, float duration)
        {
            _playerMovement.ModifySpeed(speedModifier);
            yield return new WaitForSeconds(duration);
            _playerMovement.ResetSpeed();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ItemDispenser itemDispenser))
            {
                _item = itemDispenser.GetItem();
                itemDispenser.GetComponent<NetworkObject>().Despawn();

                if (_item is SpeedModifier)
                {
                    _item.OnDo.AddListener(obj =>
                    {
                        (float speedModifier, float duration) = ((float, float))obj;
                        ModifySpeed(speedModifier, duration);
                    });
                }
            }
        }

        public void Freeze()
        {
            _freezed = true;
        }
    }
}