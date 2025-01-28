using System.Collections.Generic;
using Player;
using Unity.Netcode;
using UnityEngine;

namespace Farm
{
    [RequireComponent(typeof(FarmEarnPoints))]
    public class FarmZone : MonoBehaviour
    {
        [SerializeField] private FarmEarnPoints farmEarnPoints;

        private readonly Stack<PlayerManager> _playersOrder = new();
        private PlayerManager _firstPlayer;

        private void Awake()
        {
            farmEarnPoints = GetComponent<FarmEarnPoints>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerEnter2DRPC(collision);   
        }
        
        
        [Rpc(SendTo.Server)]
        private void OnTriggerEnter2DRPC(Collider2D collision)
        {
            PlayerManager player = collision.GetComponent<PlayerManager>();
            if (!player)
                return;
            
            if (_firstPlayer)
                _playersOrder.Push(player);
            else
            {
                _firstPlayer = player;
                farmEarnPoints.PlayerEarningPoints = _firstPlayer;
            }
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            OnTriggerExit2DRPC(collision);
        }

        [Rpc(SendTo.Server)]
        private void OnTriggerExit2DRPC(Collider2D collision)
        {
            PlayerManager player = collision.GetComponent<PlayerManager>();
            if (_firstPlayer == player)
            {
                _firstPlayer = _playersOrder.Count > 0 ? _playersOrder.Pop() : null;
                farmEarnPoints.PlayerEarningPoints = _firstPlayer;
            }
            else
            {
                RemoveLeavingPlayer(player);
            }
        }


        private void RemoveLeavingPlayer(PlayerManager leavingPlayer)
        {
            Stack<PlayerManager> tempStack = new Stack<PlayerManager>();

            while (_playersOrder.Count > 0)
            {
                PlayerManager top = _playersOrder.Pop();
                if (top != leavingPlayer)
                {
                    tempStack.Push(top);
                }
            }

            while (tempStack.Count > 0)
            {
                _playersOrder.Push(tempStack.Pop());
            }

        }
    }
}
