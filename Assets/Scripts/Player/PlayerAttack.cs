using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerAttack : NetworkBehaviour
    {
        [SerializeField] private int _defaultDamage = 10;
        [SerializeField] private int _comboDamage = 5;
        [SerializeField] private float _comboDelay = 2;
        [SerializeField] private float _attackDelay = 1;
        
        private int _currentInflictedDamage;
        private float _comboTimer;
        private float _fightTimer;
        private bool _isInCombo = false;
        
        private readonly UnityEvent _onEnemyBursted = new();

        void Awake()
        {
            _fightTimer = _attackDelay;
            _currentInflictedDamage = _defaultDamage;
        }

        private void Update()
        {
            if (_isInCombo)
            {
                _comboTimer += Time.deltaTime;
                if (_comboTimer > _comboDelay)
                {
                    _comboTimer = 0;
                    _isInCombo = false;
                    _currentInflictedDamage = _defaultDamage;
                }
            }
            _fightTimer += Time.deltaTime;
        }

        public void Attack()
        {
            AttackRPC();
        }
        
        [Rpc(SendTo.Server)]
        private void AttackRPC()
        {
            if (_fightTimer >= _attackDelay)
            {
                _fightTimer = 0;
                
                float angle = Vector2.Angle(Vector2.up, transform.up);
                RaycastHit2D[] castAll = Physics2D.BoxCastAll(transform.position, Vector2.one, angle,transform.up);
                
                foreach (var hit2D in castAll)
                {
                    if (hit2D.transform == transform)
                        continue;
                    
                    if (!hit2D.collider.TryGetComponent(out PlayerManager enemy))
                        return;
                

                    ComboSystem();
                    enemy.TakeDamageRPC(_currentInflictedDamage);
                    Debug.Log("dmg" + enemy.name);
                }
            }
        }

        private void ComboSystem()
        {
            _isInCombo = true;
            
            _comboTimer = 0;
            _currentInflictedDamage += _comboDamage;
        }

        public UnityEvent OnEnemyBursted => _onEnemyBursted;
    }
}