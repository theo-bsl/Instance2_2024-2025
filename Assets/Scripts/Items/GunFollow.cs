using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class GunFollow : NetworkBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _target;
        private Vector3 _offset;

        public override void OnNetworkSpawn()
        {
            _transform = transform;
            
            if (!IsServer)
                enabled = false;
        }

        private void Update()
        {
            _target.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);
            _transform.position = pos;
            _transform.rotation = rot;
        }

        public Transform Target { get => _target; set => _target = value; }
    }
}