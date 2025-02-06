using Leaderboard;
using System.Collections;
using Items;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private int _damageLimit = 150;
        [SerializeField] private int _burstedScoreEarn = 150;
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

        private NetworkVariable<int> _playerName = new(0);

        public override void OnNetworkSpawn()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerRotation = GetComponent<PlayerRotation>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerAttack = GetComponent<PlayerAttack>();

            _playerAttack.OnEnemyBursted.AddListener(() => IncreaseScoreRPC(_burstedScoreEarn));

            if (IsOwner)
                _playerName = new((int)OwnerClientId);
        }        
        
        [Rpc(SendTo.Server)]
        public void IncreaseScoreRPC(int amount)
        {
            _score.Value += amount;
        }

        [Rpc(SendTo.Server)]
        private void TakeDamageRPC(float amount)
        {
            _dmgTaken.Value += amount;
        }

        public bool TakeDamage(float amount)
        {
            TakeDamageRPC(amount);
            return _dmgTaken.Value >= _damageLimit;
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
                GameObject item = itemDispenser.GetItem();
                itemDispenser.Despawn();

                foreach (Item itemComponent in item.GetComponents<Item>())
                {
                    if (itemComponent is SpeedModifier)
                    {
                        _item = item;
                        itemComponent.OnDo.AddListener(obj =>
                        {
                            (float speedModifier, float duration) = ((float, float))obj;
                            ModifySpeed(speedModifier, duration);
                        });
                    }
                    else if (itemComponent is DamageModifier)
                    {
                        _item = item;
                        itemComponent.OnDo.AddListener(obj =>
                        {
                            (float damageModifier, float duration) = ((float, float))obj;
                            ModifyDamage(damageModifier, duration);
                        });
                    }
                    else if (itemComponent is FreezeGun)
                    {
                        _item = Instantiate(item,transform.parent);
                        _item.transform.localPosition = Vector3.zero;
                        _item.transform.up = transform.up;
                        _item.GetComponent<NetworkObject>().Spawn(true);
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
        
        public NetworkVariable<int> Score => _score;
        public NetworkVariable<int> PlayerName => _playerName;

    }
}