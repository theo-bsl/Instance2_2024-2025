using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _playerSpeed;
        
        private Rigidbody2D _playerBody;
        private Vector2 _movementDirection;

        void Awake()
        {
            _playerBody = GetComponent<Rigidbody2D>();
        }

        public void SetDirection(Vector2 Direction)
        {
            _movementDirection = Direction.normalized;
        }
        void FixedUpdate()
        {
            _playerBody.MovePosition(_playerBody.position + _movementDirection * (_playerSpeed * Time.deltaTime));
        }
    
    }
}
