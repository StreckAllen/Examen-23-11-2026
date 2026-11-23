using UnityEngine;
namespace ClaseProgAvanzada
{
    //clase abstracta para armas
    public abstract class Weapon : MonoBehaviour
    {
        //metodo que permite que cada arma se utilice a su modo
        public abstract void Use();

    }
}

