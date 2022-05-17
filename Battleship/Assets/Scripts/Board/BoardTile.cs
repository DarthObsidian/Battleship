using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Board
{
    public class BoardTile : MonoBehaviour, IPointerDownHandler
    {
        public bool occupied;
        public bool alreadyGuessed;

        public void OnPointerDown(PointerEventData eventData)
        {
            alreadyGuessed = true;
        }
    }
}