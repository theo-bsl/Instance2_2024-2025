using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Farm
{
    [RequireComponent(typeof(FarmEarn))]
    public class FarmZone : MonoBehaviour
    {
        [SerializeField] private FarmEarn _farmEarn;

        private readonly Stack<PlayerManager> _playersOrder = new();
        private PlayerManager _firstPlayer;

        private void Awake()
        {
            _farmEarn = GetComponent<FarmEarn>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerManager player = collision.GetComponent<PlayerManager>();
            if (!player)
                return;
            
            if (_firstPlayer)
                _playersOrder.Push(player);
            else
            {
                _firstPlayer = player;
                _farmEarn.PlayerEarningPoints = _firstPlayer;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            PlayerManager player = collision.GetComponent<PlayerManager>();
            if (_firstPlayer == player)
            {
                _firstPlayer = _playersOrder.Count > 0 ? _playersOrder.Pop() : null;
                _farmEarn.PlayerEarningPoints = _firstPlayer;
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
