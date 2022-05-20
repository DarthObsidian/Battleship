using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public class RuntimeShip : MonoBehaviour
    {
        public int hp;
        public string shipName;
        public string owner;
        public bool dead;
        public static UnityAction<RuntimeShip> OnDeath;

        public void Setup(Ship _ship, string _owner)
        {
            hp = _ship.hp;
            shipName = _ship.name;
            owner = _owner;
        }

        public void GetHit()
        {
            hp--;
            if (hp <= 0)
                Die();
        }

        private void Die()
        {
            dead = true;
            OnDeath?.Invoke(this);
        }
    }
}