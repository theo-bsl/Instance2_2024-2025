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
        private Vector2 _screenMousePosition = Vector2.zero;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            _screenMousePosition = cam.ScreenToWorldPoint(_mousePosition);
            _rotation.Set(_screenMousePosition.x - transform.position.x, _screenMousePosition.y - transform.position.y);
            _transform.up = _rotation;
        }
        
        [ServerRpc]
        private void SetMousePositionServerRpc(Vector2 mousePosition)
        {
            _mousePosition = mousePosition;
            Debug.Log($"SetMousePositionServerRpc: {mousePosition}");
        }
        public void SetMousePosition(Vector2 mousePosition)
        {
            SetMousePositionServerRpc(mousePosition);
        }
    }
}