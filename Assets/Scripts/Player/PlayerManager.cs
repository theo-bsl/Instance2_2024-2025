using System.Collections;
using Items;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private PlayerShowInfoUI _playerShowInfoUI;
        
        [SerializeField] private GameObject _item;
        private string _itemName;
        private PlayerMovement _playerMovement;
        private PlayerRotation _playerRotation;
        private PlayerAttack _playerAttack;
        private PlayerBurst _playerBurst;
        private SpriteRenderer _spriteRenderer;
        private PlayerInfos _playerInfos;
        private Animator _animator;
        
        private readonly UnityEvent<string> _onUseItem = new();
        private readonly UnityEvent<string> _onEndItem = new();
        private readonly UnityEvent<string> _onGetItem = new();
        
        
        private GameObject _instantiatedItem;

        //private NetworkVariable<char[]> _playerName = new("bob".ToCharArray());
        private NetworkList<char> _playerName = new();
        
        public override void OnNetworkSpawn()
        {
            _playerInfos = GetComponent<PlayerInfos>();
            
            _playerInfos.Name(_playerShowInfoUI);
            UpdatePlayerName(transform.name);

            _animator = GetComponent<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerRotation = GetComponent<PlayerRotation>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerAttack = GetComponent<PlayerAttack>();
            _playerBurst = GetComponent<PlayerBurst>();

            _playerShowInfoUI.SetDamage(0);
            /*transform.name = GetPseudo._usernameResponse.username;
            _playerShowInfoUI.SetName(GetPseudo._usernameResponse.username);*/

            //_playerName = _playerInfos.username;
            
            if(IsServer)
            {
                _playerAttack.OnEnemyBursted.AddListener(() => IncreaseScoreRPC(_burstedScoreEarn));
                _playerBurst.OnEndBurstedEvent.AddListener(ResetDamage);
            }

            NetworkManager.Singleton.OnClientDisconnectCallback += OnDestroyPlayer;
        }

        private void OnDestroyPlayer(ulong obj)
        {
            OnDestroyed?.Invoke();
        }

        [Rpc(SendTo.Server)]
        public void IncreaseScoreRPC(int amount)
        {
            _score.Value += amount;
        }

        public bool TakeDamage(float amount)
        {
            TakeDamageRPC(amount);
            _animator.SetTrigger("TakeDamage");
            if(_dmgTaken.Value >= _maxDmgTaken)
                _playerBurst.Burst();
            _playerShowInfoUI.SetDamage(_dmgTaken.Value);
            return _dmgTaken.Value >= _damageLimit;
        }
        
        [Rpc(SendTo.Server)]
        private void TakeDamageRPC(float amount)
        {
            _dmgTaken.Value += amount;
            UpdatePlayerDamageUIRpc(_dmgTaken.Value);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void UpdatePlayerDamageUIRpc(float damage)
        {
            _playerShowInfoUI.SetDamage(damage);
        }

        private void UpdatePlayerName(string playerName)
        {
            _playerShowInfoUI.SetName(playerName);
        }

        private void ResetDamage()
        {
            _dmgTaken.Value = 0f;
            UpdatePlayerDamageUIRpc(_dmgTaken.Value);
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
            _onUseItem.Invoke(_itemName);
            
            foreach (Item item in _item.GetComponents<Item>())
            {
                item?.Do();
                if (item is FreezeGun)
                {
                    _animator.SetBool("hasGun", false);
                }
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
            _onEndItem.Invoke(_itemName);
            _playerMovement.ResetSpeed();
        }

        private void ModifyDamage(float damageModifier, float duration)
        {
            StartCoroutine(ModifyDamageCoroutine(damageModifier, duration));
        }

        private IEnumerator ModifyDamageCoroutine(float damageModifier, float duration)
        {
            _playerAttack.ModifyDamageRpc(damageModifier);
            yield return new WaitForSeconds(duration);
            _playerAttack.ResetDamage();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ItemDispenser itemDispenser))
            {
                if (_item)
                {
                    itemDispenser.Despawn();
                    return;
                }
                
                GameObject item = itemDispenser.GetItem();
                _itemName = item.name;

                itemDispenser.Despawn();
                _onGetItem.Invoke(_itemName);

                foreach (Item itemComponent in item.GetComponents<Item>())
                {
                    if (itemComponent is SpeedModifier)
                    {
                        _item = item;
                        itemComponent.OnDo.AddListener(obj =>
                        {
                            (float speedModifier, float duration) = ((float, float))obj;
                            ModifySpeed(speedModifier, duration);
                            itemComponent.OnDo.RemoveAllListeners();
                        });
                    }
                    else if (itemComponent is DamageModifier)
                    {
                        _item = item;
                        itemComponent.OnDo.AddListener(obj =>
                        {
                            (float damageModifier, float duration) = ((float, float))obj;
                            ModifyDamage(damageModifier, duration);
                            itemComponent.OnDo.RemoveAllListeners();
                        });
                    }
                    else if (itemComponent is FreezeGun)
                    {
                        _item = Instantiate(item);
                        _item.GetComponent<GunFollow>().Target = _gunTransform;
                        _item.transform.localPosition = Vector3.zero;
                        _item.transform.up = transform.up;
                        _item.GetComponent<NetworkObject>().Spawn(true);
                        
                        _animator.SetBool("hasGun", true);
                        
                        itemComponent.OnDo.AddListener(_ => _playerAttack.EjectedSelf(this));
                        itemComponent.OnDo.RemoveAllListeners();
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
        public NetworkVariable<float> DmgTaken => _dmgTaken;
        public NetworkList<char> PlayerName => _playerName;
        public UnityEvent<string> OnUseItem => _onUseItem;
        public UnityEvent<string> OnEndItem => _onEndItem;
        public UnityEvent<string> OnGetItem => _onGetItem;
        
        public event System.Action OnDestroyed;

    }
}