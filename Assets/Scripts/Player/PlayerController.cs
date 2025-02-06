using Unity.Netcode;
using Unity.Netcode.Components;
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
        [SerializeField] private Animator _playerAnimator;
        
        public void OnMove(InputAction.CallbackContext context)
        {
            _playerMovement.SetDirection(context.ReadValue<Vector2>());
            PlayAnimationRPC("isWalking", true);
            if (context.canceled)
            {
                PlayAnimationRPC("isWalking", false);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            _playerAttack.Attack();
            PlayAnimationRPC("isAttacking", true);
            if (context.canceled)
            {
                PlayAnimationRPC("isAttacking", false);
            }
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            _playerRotation.SetMousePosition(context.ReadValue<Vector2>());
        }
        
        public void OnItemUsed(InputAction.CallbackContext context)
        {
            if (context.started)
                _playerManager.UseItem();
        }

        [Rpc(SendTo.Server)]
        public void PlayAnimationRPC(string paramName, bool value)
        {
            _playerAnimator.SetBool(paramName, value);
        }
    }
}