using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class FreezeGun : Item
    {
        [SerializeField] private FreezeBullet _bulletPrefab;

        public override void Do()
        {
            FreezeBullet freezeBullet = Instantiate(_bulletPrefab, transform.position + transform.up, Quaternion.identity);
            freezeBullet.Direction = transform.up;
            freezeBullet.GetComponent<NetworkObject>().Spawn(true);
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
