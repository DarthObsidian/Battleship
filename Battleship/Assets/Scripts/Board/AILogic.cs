using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using System.Linq;

namespace Board
{
    public class AILogic : MonoBehaviour
    {
        public Vector2Int gridSize;
        public GameBoard board;
        public GameObject runtimeShipPrefab;
        public List<Ship> ships = new List<Ship>();
        private List<RuntimeShip> placedShips = new List<RuntimeShip>();

        int deadShips;

        private void Awake()
        {
            RuntimeShip.OnDeath += HandleDeadShip;

            board = new GameBoard(gridSize);

            for (int x = 0; x < gridSize.x; x++)
                for (int y = 0; y < gridSize.y; y++)
                    SetupTile(x, y);
        }

        private void SetupTile(int x, int y)
        {
            board.SetTile(x, y);
        }

        private void Start()
        {
            foreach (Ship ship in ships)
                PlaceShip(ship);

            string output = "";
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    int shipType = board.GetShipMapItem(x, y);
                    output += $"{shipType}\t";
                }
                output += "\n";
            }
            Debug.Log(output);
        }

        private void PlaceShip(Ship _ship)
        {
            Vector2Int start;
            Vector2Int direction;
            Vector2Int end = Vector2Int.zero;

            do
            {
                start = GetRandomTile();
                direction = GetRandomDirection();
                
                int length = _ship.hp - 1;
                end.x = (direction.x * length) + start.x;
                end.y = (direction.y * length) + start.y;
            }
            while (!board.IsValidTile(end.x, end.y) || !IsValidTile(start.x, start.y));
            Debug.Log($"{start} {end}");
            var tiles = board.GatherTiles(start, end, _ship.hp);
            if (board.AreTilesOccupied(tiles))
            {
                PlaceShip(_ship);
                return;
            }

            SetShipPlacementData(tiles, _ship);
        }

        private Vector2Int GetRandomTile()
        {
            int randomX = Random.Range(0, gridSize.x - 1);
            int randomY = Random.Range(0, gridSize.y - 1);
            return new Vector2Int(randomX, randomY);
        }

        private void SetShipPlacementData(Vector2Int[] _tiles, Ship _ship)
        {
            board.SetOccupiedTiles(_tiles);
            CreateRuntimeShip(_ship);
            board.SetShipInMap(_tiles, placedShips.Count);
        }

        private void CreateRuntimeShip(Ship _ship)
        {
            var shipObject = Instantiate(runtimeShipPrefab, transform);
            var runtimeShip = shipObject.GetComponent<RuntimeShip>();
            runtimeShip.Setup(_ship, "AI");
            placedShips.Add(runtimeShip);
        }

        private Vector2Int GetRandomDirection()
        {
            int random = Random.Range(0, 3);

            return random switch
            {
                0 => Vector2Int.up,
                1 => Vector2Int.down,
                2 => Vector2Int.left,
                _ => Vector2Int.right,
            };
        }

        private void HandleDeadShip(RuntimeShip _ship)
        {
            if(_ship.owner != "AI")
                return;

            TurnManager.Message?.Invoke($"You sunk my {_ship.shipName}!");
            deadShips++;

            if (deadShips >= placedShips.Count)
                TurnManager.AllShipsSunk?.Invoke("AI");
        }

        public bool CheckIfTileGuessed(int x, int y)
        {
            return board.IsTileGuessed(x, y);
        }

        public bool WasTileHit(int x, int y)
        {
            int shipId = board.GetShipMapItem(x, y);
            board.SetTileGuessed(x, y);
            if (shipId == 0)
                return false;

            placedShips[shipId-1].GetHit();
            return true;
        }

        public bool IsValidTile(int x, int y)
        {
            return board.IsValidTile(x, y);
        }

        public Vector2Int Fire()
        {
            return GetRandomTile();
        }
    }
}