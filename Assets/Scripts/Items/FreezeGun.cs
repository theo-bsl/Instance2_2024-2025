using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class FreezeGun : Item
    {
        [SerializeField] private FreezeBullet _bulletPrefab;
        [SerializeField] private float _distanceOffset = 0.25f;

        public override void Do()
        {
            FreezeBullet freezeBullet = Instantiate(_bulletPrefab, transform.position + transform.up * _distanceOffset, Quaternion.identity);
            freezeBullet.Direction = transform.up;
            freezeBullet.GetComponent<NetworkObject>().Spawn(true);
            _onDo.Invoke(null);
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
