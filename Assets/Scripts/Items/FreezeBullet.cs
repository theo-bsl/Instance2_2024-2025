using Player;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class FreezeBullet : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _duration;

        private Vector3 _direction;
        private Transform _transform;

        public override void OnNetworkSpawn()
        {
            _transform = transform;
        }

        private void Update()
        {
            _transform.position += _direction * (_speed * Time.deltaTime);

            if (_duration <= 0)
            {
                GetComponent<NetworkObject>().Despawn();
            }

            _duration -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerManager playerManager))
            {
                playerManager.Freeze();
            }

            GetComponent<NetworkObject>().Despawn();
        }

        public Vector3 Direction { get { return _direction; } set { _direction = value; } }
    }
}
