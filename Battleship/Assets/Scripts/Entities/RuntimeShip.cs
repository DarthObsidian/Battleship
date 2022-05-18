using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class RuntimeShip : MonoBehaviour
    {
        public int hp;
        public string shipName;

        public void Setup(Ship _ship)
        {
            hp = _ship.hp;
            shipName = _ship.name;
        }

        public void GetHit()
        {
            hp--;
            if (hp <= 0)
                Die();
        }

        private void Die()
        {
            throw new System.NotImplementedException();
        }
    }
}