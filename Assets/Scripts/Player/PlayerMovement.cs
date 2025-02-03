using Unity.Netcode;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private float _playerSpeed = 10.0f;
        [SerializeField] private float _defaultPlayerSpeed;
        
        private Rigidbody2D _playerBody;
        private Vector2 _movementDirection;

        public override void OnNetworkSpawn()
        {
            _playerSpeed = _defaultPlayerSpeed;
        }

        void Awake()
        {
            _playerBody = GetComponent<Rigidbody2D>();
        }

        public void SetDirection(Vector2 dir)
        {
            SetDirectionRPC(dir);
        }
        
        [Rpc(SendTo.Server)]
        public void SetDirectionRPC(Vector2 Direction)
        {
            _movementDirection = Direction.normalized;
        }
        void FixedUpdate()
        {
            _playerBody.MovePosition(_playerBody.position + _movementDirection * (_playerSpeed * Time.deltaTime));
        }

        public void ModifySpeed(float speedModifier)
        {
            _playerSpeed += _playerSpeed * speedModifier;
        }

        public void ResetSpeed()
        {
            _playerSpeed = _defaultPlayerSpeed;
        }
    }
}
