using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

namespace Board
{
    public class PlayerLogic : MonoBehaviour
    {
        public float cellSize;
        public Vector2Int gridSize;
        public GameObject tilePrefab;
        public GameBoard board;
        public GameObject[,] tiles;
        public GameObject hitObject;
        public GameObject missObject;

        int deadShips;
        private List<RuntimeShip> shipList = new List<RuntimeShip>();

        private void Awake()
        {
            RuntimeShip.OnDeath += HandleDeadShip;
            
            board = new GameBoard(gridSize);
            tiles = new GameObject[gridSize.x, gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
                for (int y = 0; y < gridSize.y; y++)
                    SetupTile(x, y);
        }

        private void SetupTile(int x, int y)
        {
            var tile = Instantiate(tilePrefab, transform);
            tile.transform.localPosition = new Vector3(x * cellSize, 0, y * cellSize);
            tile.name = $"Tile {x} {y}";
            tiles[x, y] = tile;
            board.SetTile(x, y);
        }

        private void HandleDeadShip(RuntimeShip _ship)
        {
            if (_ship.owner == "AI")
                return;

            deadShips++;

            if (deadShips >= shipList.Count)
                TurnManager.AllShipsSunk?.Invoke("Player");
        }

        public bool IsValidTile(Transform _transform)
        {
            return board.IsValidTile(_transform);
        }

        public bool IsValidTile(int x, int y)
        {
            return board.IsValidTile(x, y);
        }

        public bool AreTilesOccupied(Vector2Int[] _tiles)
        {
            return board.AreTilesOccupied(_tiles);
        }

        public Vector2Int[] GatherTiles(Vector2 _start, Vector2 _end, int _length)
        {
            return board.GatherTiles(_start, _end, _length);
        }

        public void SetOccupiedTiles(Vector2Int[] _tiles)
        {
            board.SetOccupiedTiles(_tiles);
            board.SetShipInMap(_tiles, shipList.Count);
        }

        public void AddShip(GameObject _ship)
        {
            var ship = _ship.GetComponent<RuntimeShip>();
            if (ship != null)
                shipList.Add(ship);
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
            {
                SetIndicator(x, y, false);
                return false;
            }

            shipList[shipId - 1].GetHit();
            SetIndicator(x, y, true);
            return true;
        }

        public List<Vector2Int> GetNotGuessedTiles()
        {
            return board.GetNotGuessedTiles();
        }

        private void SetIndicator(int x, int y, bool _wasHit)
        {
            GameObject indicator = _wasHit ? hitObject : missObject;
            Transform parent = tiles[x, y].transform;
            Vector3 pos = parent.position;

            Instantiate(indicator, pos, Quaternion.identity, parent);
        }
    }
}