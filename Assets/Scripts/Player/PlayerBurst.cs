using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerBurst : NetworkBehaviour
    {
        [SerializeField] private float _minScale = 0.15f;
        [SerializeField] private float _maxScale = 1.25f;
        
        private Transform _transform;
        private Vector3 _burstPosition;
        private Vector3 _scaleWhenBurst;
        private Coroutine _burstCoroutine;
        
        private readonly UnityEvent<PlayerBurst> _onBursted = new();
        private readonly UnityEvent _onEndBursted = new();

        public override void OnNetworkSpawn()
        {
            _transform = transform;
        }

        public void Burst()
        {
            if (IsServer)
                GetComponentInChildren<Collider2D>().enabled = false;
            else
                GetComponentInChildren<PlayerInput>().enabled = false;

            _burstPosition = _transform.position;
            _onBursted.Invoke(this);
        }

        public void BurstedTo(Vector3 position)
        {
            _burstCoroutine ??= StartCoroutine(BurstedToCoroutine(position));
        }

        private IEnumerator BurstedToCoroutine(Vector3 spawnPosition)
        {
            Vector3 middlePosition = Vector3.Lerp(_burstPosition, spawnPosition, 0.5f);
            float initialDistance = Vector3.Distance(_burstPosition, spawnPosition);
            float middleDistance = Vector3.Distance(middlePosition, _burstPosition);
            float distance = Vector3.Distance(_transform.position, middlePosition);
            
            while (Vector3.Distance(_transform.position, spawnPosition) > 0.001f)
            {
                _transform.position = Vector3.Lerp(_burstPosition, spawnPosition, 1 - distance / initialDistance + Time.deltaTime);
                _transform.localScale = Vector3.one * Mathf.Lerp(_minScale, _maxScale, 1 - distance / middleDistance);
                distance = Vector3.Distance(_transform.position, middlePosition);
                
                yield return null;
            }

            _transform.position = spawnPosition;
            _burstCoroutine = null;
            EndBurst();
        }

        private void EndBurst()
        {
            if (IsServer)
                GetComponentInChildren<Collider2D>().enabled = true;
            else
                GetComponentInChildren<PlayerInput>().enabled = true;
            
            _transform.localScale = Vector3.one * _minScale;
            _onEndBursted.Invoke();
        }
        
        public UnityEvent<PlayerBurst> OnBurstedEvent => _onBursted;
        public UnityEvent OnEndBurstedEvent => _onEndBursted;
    }
}