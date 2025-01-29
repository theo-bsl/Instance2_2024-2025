using Unity.VisualScripting;
using UnityEngine;

public class FreezeGun : Item
{
    [SerializeField] private FreezeBullet Bullet; 


    protected override void Do()
    {
        FreezeBullet freezeBullet = Instantiate(Bullet,transform.position, Quaternion.identity);

        freezeBullet.Direction = transform.up;

    }

    
}
