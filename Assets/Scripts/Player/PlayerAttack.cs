using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private int _defaultDamage = 10;
        [SerializeField] private int _comboDamage = 5;
        [SerializeField] private float _comboDelay = 2;
        [SerializeField] private float _attackDelay = 1;
        
        private int _currentInflictedDamage;
        private float _comboTimer;
        private float _fightTimer;
        private bool _isInCombo = false;

        void Awake()
        {
            _fightTimer = _attackDelay;
        }

        private void Update()
        {
            if (_isInCombo)
            {
                _comboTimer += Time.deltaTime;
            }
            _fightTimer += Time.deltaTime;
        }

        public void Attack()
        {
            if (_fightTimer >= _attackDelay)
            {
                _fightTimer = 0;
                
                float angle = Vector2.Angle(Vector2.up, transform.up);
                RaycastHit2D[] castAll = Physics2D.BoxCastAll(transform.position, Vector2.one, angle,transform.up);
                //Debug.DrawRay(transform.position, transform.up * angle * 10, Color.red, 2);
                
                foreach (var hit2D in castAll)
                {
                    if (hit2D.transform == transform)
                        continue;
                    
                    if (!hit2D.collider.TryGetComponent(out PlayerManager enemy))
                        return;
                
                    enemy.TakeDamageRPC(_currentInflictedDamage);
                    ComboSystem();
                    Debug.Log("dmg");
                }
            }
        }
        
        /*
        public void OnTriggerStay2D(Collider2D collision)
        {
            if (_fightTimer >= _attackDelay && Input.GetMouseButtonDown(0))
            {
                PlayerManager _enemy = collision.GetComponent<PlayerManager>();
                if (_enemy == null)
                {
                    _fightTimer = 0;
                    return;
                }
                _enemy._dmgTaken += _defaultDamage;
                _enemy.GetComponentInParent<Rigidbody2D>().linearVelocity = _enemy.GetComponentInChildren<Transform>().up * _enemy._dmgTaken;
                Debug.Log(_defaultDamage);
                ComboSystem();
                _fightTimer = 0;
                
                if (!collision.TryGetComponent(out PlayerManager enemy))
                    return;
                
                
            }
        } */

        private void ComboSystem()
        {
            _isInCombo = true;
            if (_comboTimer > _comboDelay)
            {
                _comboTimer = 0;
                _isInCombo = false;
                _currentInflictedDamage = _defaultDamage;
            }
            else
            {
                _comboTimer = 0;
                _currentInflictedDamage += _comboDamage;
            }
        }
    }
}