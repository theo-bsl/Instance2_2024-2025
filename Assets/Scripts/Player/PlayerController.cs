using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAttack _playerAttack;
        [SerializeField] private PlayerRotation _playerRotation;
        
        public void OnMove(InputAction.CallbackContext context)
        {
            _playerMovement.SetDirection(context.ReadValue<Vector2>());
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            _playerAttack.Attack();
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            _playerRotation.SetMousePosition(context.ReadValue<Vector2>());
        }
        
        public void OnItemUsed(InputAction.CallbackContext context)
        {
            _playerManager.UseItem();
        }
    }
}