using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

namespace Lobby
{
    public class TimeUIManager : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TimeManager _timeManager;
        
        public override void OnNetworkSpawn()
        {
            _timeManager.OnUpdateUI.AddListener((minutes, seconds) => UpdateUIRpc(minutes, seconds));
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void UpdateUIRpc(int minutes, int seconds)
        {
            _timeText.SetText(minutes + " : " + seconds.ToString("00"));
        }

    }
}