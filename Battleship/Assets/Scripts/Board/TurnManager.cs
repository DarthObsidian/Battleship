using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Board
{
    public class TurnManager : MonoBehaviour
    {
        public static UnityAction<string> AllShipsSunk;
        public static UnityAction<string> Message;
        public static UnityAction TurnFinished;
        public static UnityAction ActivateUI;
        public static UnityAction AllShipsPlaced;

        public PlayerLogic player;
        public AILogic ai;

        bool isPlayerTurn;

        private void Awake()
        {
            AllShipsSunk += HandleAllShipsSunk;
            Message += HandleMessage;
            TurnFinished += HandleTurnFinished;
            AllShipsPlaced += HandleAllShipsPlaced;
        }

        private void Start()
        {
            isPlayerTurn = Random.value > 0.4f;
            if (isPlayerTurn)
                ActivateUI?.Invoke();
        }

        private void HandleAllShipsPlaced()
        {
            if (!isPlayerTurn)
                StartCoroutine(Waiter());
        }

        private void DoAITurn()
        {
            Debug.Log("Ai taking turn");

            Vector2Int target;
            bool alreadyGuessed = true;

            do
            {
                target = ai.Fire();
                if (!player.IsValidTile(target.x, target.y))
                    continue;

                alreadyGuessed = player.CheckIfTileGuessed(target.x, target.y);
            }
            while (alreadyGuessed);

            _ = player.WasTileHit(target.x, target.y);

            isPlayerTurn = true;
            ActivateUI?.Invoke();
            Debug.Log("ai turn done");
        }

        private void HandleAllShipsSunk(string _id)
        {
            string winner = "AI";
            if (_id == winner)
                winner = "Player";

            Debug.Log($"ALL SHIPS OF {_id} HAVE BEEN SUNK {winner} WON");
        }

        private void HandleMessage(string _message)
        {
            Debug.Log(_message);
        }

        private void HandleTurnFinished()
        {
            isPlayerTurn = false;
            StartCoroutine(Waiter());
        }

        private IEnumerator Waiter()
        {
            float waitTime = Random.Range(0.5f, 2f);
            yield return new WaitForSeconds(waitTime);
            DoAITurn();
        }
    }
}