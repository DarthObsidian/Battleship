using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(menuName = "ScriptableObject/Ship", fileName = "New ship")]
    public class Ship : ScriptableObject
    {
        public GameObject visual;
        public int hp;
    }
}