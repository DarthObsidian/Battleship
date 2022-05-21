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
        public static UnityAction<bool> TurnFinished;
        public static UnityAction<bool> ToggleUI;
        public static UnityAction AllShipsPlaced;
        public static UnityAction PlayerReady;

        public PlayerLogic player;
        public AILogic ai;

        bool isPlayerTurn;
        bool gameOver;
        bool playerReady;
        Coroutine waiter;
        Coroutine aiTurn;

        private void Awake()
        {
            PlayerReady += HandlePlayerReady;
            AllShipsSunk += HandleAllShipsSunk;
            TurnFinished += HandleTurnFinished;
            AllShipsPlaced += HandleAllShipsPlaced;
        }

        private void HandleAllShipsPlaced()
        {
            isPlayerTurn = Random.value > 0.4f;
            TurnFinished?.Invoke(isPlayerTurn);
        }

        private void DoAITurn()
        {
            List<Vector2Int> guesses = player.GetNotGuessedTiles();
            Vector2Int target = ai.Fire(guesses);

            _ = player.WasTileHit(target.x, target.y);

            TurnFinished?.Invoke(true);
        }

        private void HandleAllShipsSunk(string _id)
        {
            gameOver = true;
            ToggleUI?.Invoke(false);

            string winner = "AI";
            if (_id == winner)
                winner = "Player";
            Message?.Invoke($"ALL SHIPS OF {_id} HAVE BEEN SUNK {winner} WON!");
        }

        private void HandleTurnFinished(bool _isPlayerTurn)
        {
            if (gameOver)
                return;
            isPlayerTurn = _isPlayerTurn;

            if (!isPlayerTurn)
            {
                if (aiTurn != null)
                    StopCoroutine(aiTurn);
                aiTurn = StartCoroutine(StartAITurn());
                return;
            }

            ToggleUI?.Invoke(true);
        }

        private void HandlePlayerReady()
        {
            if(!isPlayerTurn)
                playerReady = true;
        }

        private IEnumerator StartAITurn()
        {
            while (!playerReady)
                yield return null;

            if (waiter != null)
                StopCoroutine(waiter);
            waiter = StartCoroutine(Waiter());
            playerReady = false;
        }

        private IEnumerator Waiter()
        {
            float waitTime = Random.Range(0.5f, 2f);
            yield return new WaitForSeconds(waitTime);
            DoAITurn();
        }
    }
}