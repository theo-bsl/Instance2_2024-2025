using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        
        public void OnMove(InputAction.CallbackContext context)
        {
            _playerMovement.SetDirection(context.ReadValue<Vector2>());
        } 
    }
}