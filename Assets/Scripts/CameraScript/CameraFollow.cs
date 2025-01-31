using UnityEngine;

namespace CameraScript
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        private Vector3 _cameraPosition = Vector3.zero;
        private float _cameraDistance = -10f;
        
        private void Update()
        {
            _cameraPosition.Set(_playerTransform.position.x, _playerTransform.position.y, _cameraDistance);
            transform.position = _cameraPosition;
        }
    }
}