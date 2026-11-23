using UnityEngine;
namespace ClaseProgAvanzada
{
    //Concepto para el patron de estados
    public interface IEnemyState
    {
        //Cuando el proceso ingresa
        void Enter();

        //Cuando el proceso se ejecuta
        void Update();

        //Cuando el proceso sale
        void Exit();
    }
}


