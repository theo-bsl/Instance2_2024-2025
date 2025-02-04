using System.Collections;
using Items;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private NetworkVariable<int> _score = new(0);
        [SerializeField] private NetworkVariable<float> _dmgTaken = new(0);
        [SerializeField] private float _maxDmgTaken = 100f;
        [SerializeField] private Transform _gunTransform;

        [SerializeField] private GameObject _item;
        private PlayerMovement _playerMovement;
        private PlayerRotation _playerRotation;
        private PlayerAttack _playerAttack;
        private SpriteRenderer _spriteRenderer;
        
        private GameObject _instanciatedItem;

        public override void OnNetworkSpawn()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerRotation = GetComponent<PlayerRotation>();
            _playerAttack = GetComponent<PlayerAttack>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void IncreaseScore(int amount)
        {
            _score.Value += amount;
        }

        [Rpc(SendTo.Server)]
        public void TakeDamageRPC(float amount)
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
            UseItemRpc();
        }
        
        [Rpc(SendTo.Server)]
        private void UseItemRpc()
        {
            if (!_item)
                return;
            
            foreach (Item item in _item.GetComponents<Item>())
            {
                item?.Do();
            }
            
            _item = null;
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

        private void ModifyDamage(float damageModifier, float duration)
        {
            StartCoroutine(ModifyDamageCoroutine(damageModifier, duration));
        }

        private IEnumerator ModifyDamageCoroutine(float damageModifier, float duration)
        {
            _playerAttack.ModifyDamage(damageModifier);
            yield return new WaitForSeconds(duration);
            _playerAttack.ResetDamage();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ItemDispenser itemDispenser))
            {
                _item = Instantiate(itemDispenser.GetItem(),transform);
                _item.GetComponent<NetworkObject>().Spawn(true);
                _item.SetActive(false);

                foreach (Item item in _item.GetComponents<Item>())
                {
                    if (item is SpeedModifier)
                    {
                        item.OnDo.AddListener(obj =>
                        {
                            (float speedModifier, float duration) = ((float, float))obj;
                            ModifySpeed(speedModifier, duration);
                        });
                    }
                    else if (item is DamageModifier)
                    {
                        item.OnDo.AddListener(obj =>
                        {
                            (float damageModifier, float duration) = ((float, float))obj;
                            ModifyDamage(damageModifier, duration);
                        });
                    }
                    else if (item is FreezeGun)
                    {
                        _item.SetActive(true);
                        _item.transform.SetParent(transform.parent);
                        _item.transform.localPosition = Vector3.zero;
                        _item.transform.up = transform.up;
                        _item.GetComponent<GunFollow>().Target = _gunTransform;
                    }
                }
            }
        }

        public void Freeze(float freezeDuration)
        {
            StartCoroutine(FreezeCoroutine(freezeDuration));
        }

        private IEnumerator FreezeCoroutine(float freezeDuration)
        {
            _playerMovement.Freeze(true);
            _playerRotation.Freeze(true);
            _spriteRenderer.color = Color.cyan;
            
            yield return new WaitForSeconds(freezeDuration);
            
            _playerMovement.Freeze(false);
            _playerRotation.Freeze(false);
            _spriteRenderer.color = Color.white;
        }
    }
}