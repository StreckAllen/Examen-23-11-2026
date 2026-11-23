
using UnityEngine;

namespace ClaseProgAvanzada
{
    public class Bow : Weapon
    {
        [SerializeField] private Transform shootPoint;


        public override void Use()
        {
               if (ProjectilePool.InstanceProjectilePool != null)
            {
                ProjectilePool.InstanceProjectilePool.GetProjectile(
                    shootPoint.position,
                    shootPoint.rotation
                );
            }
        
        }
    }
}
