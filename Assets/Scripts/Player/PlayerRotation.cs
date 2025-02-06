using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerRotation : NetworkBehaviour
    {
        [SerializeField] private Camera cam;
        
        private Transform _transform;
        private Vector2 _rotation = Vector2.zero;
        private Vector2 _mousePosition = Vector2.zero;
        private Vector3 _worldMousePosition = Vector3.zero;
        private Vector3 _mouseInput = Vector3.zero;
        
        private bool _isFreezed = false;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            if (_isFreezed)
                return;
            
            _mouseInput.x = Input.mousePosition.x;
            _mouseInput.y = Input.mousePosition.y;
            _mouseInput.z = cam.nearClipPlane;
            Vector3 _worldMousePosition = cam.ScreenToWorldPoint(_mouseInput);
            _worldMousePosition.z = 0f;
            RotatePlayerServerRpc(_worldMousePosition);
            
        }

        private void RotatePlayer()
        {
            _mouseInput.x = Input.mousePosition.x;
            _mouseInput.y = Input.mousePosition.y;
            _mouseInput.z = cam.nearClipPlane;
            Vector3 _worldMousePosition = cam.ScreenToWorldPoint(_mouseInput);
            _worldMousePosition.z = 0f;
            RotatePlayerServerRpc(_worldMousePosition);
        }

        [ServerRpc]
        private void RotatePlayerServerRpc(Vector3 _worldMousePosition)
        {
            if (_worldMousePosition != _transform.position)
            {
                 Vector3 targetDirection = _worldMousePosition - _transform.position;
                targetDirection.z = 0f;
                _transform.up = targetDirection;
            }
        }
        
        [ServerRpc]
        private void SetMousePositionServerRpc(Vector2 mousePosition)
        {
            _mousePosition = mousePosition;
            // Debug.Log($"SetMousePositionServerRpc: {mousePosition}");
        }
        
        public void SetMousePosition(Vector2 mousePosition)
        {
            SetMousePositionServerRpc(Input.mousePosition);
        }

        public void Freeze(bool freeze)
        {
            _isFreezed = freeze;
        }
    }
}