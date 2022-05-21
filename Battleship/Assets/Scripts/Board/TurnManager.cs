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
        public static UnityAction<bool> ToggleUI;
        public static UnityAction AllShipsPlaced;

        public PlayerLogic player;
        public AILogic ai;

        bool isPlayerTurn;
        bool gameOver;

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
                ToggleUI?.Invoke(true);
        }

        private void HandleAllShipsPlaced()
        {
            if (!isPlayerTurn)
                StartCoroutine(Waiter());
        }

        private void DoAITurn()
        {
            List<Vector2Int> guesses = player.GetNotGuessedTiles();
            Vector2Int target = ai.Fire(guesses);

            _ = player.WasTileHit(target.x, target.y);

            isPlayerTurn = true;
            ToggleUI?.Invoke(true);
        }

        private void HandleAllShipsSunk(string _id)
        {
            gameOver = true;
            ToggleUI?.Invoke(false);

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
            if (gameOver)
                return;
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