using Player;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class FreezeBullet : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _duration;
        [SerializeField] private float _freezeDuration;

        private Vector3 _direction;
        private Transform _transform;

        public override void OnNetworkSpawn()
        {
            _transform = transform;
            
            if (!IsServer)
            {
                enabled = false;
                GetComponent<Collider2D>().enabled = false;
            }
        }

        private void Update()
        {
            _transform.position += _direction * (_speed * Time.deltaTime);
            _transform.up = _direction;

            if (_duration <= 0)
                GetComponent<NetworkObject>().Despawn();

            _duration -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerManager playerManager))
                playerManager.Freeze(_freezeDuration);

            GetComponent<NetworkObject>().Despawn();
        }

        public Vector3 Direction { get { return _direction; } set { _direction = value; } }
    }
}
