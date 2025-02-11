using UnityEngine;

namespace Player
{
    public class PlayerUIFollow : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        private Vector3 _canvaPosition = Vector3.zero;
        private float _canvasYPosition = 0.16f;
        
        private void Update()
        {
            _canvaPosition.Set(_playerTransform.position.x, _playerTransform.position.y + _canvasYPosition, 0);
            transform.position = _canvaPosition;
        }
    }
}