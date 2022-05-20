using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    public class GameBoard : MonoBehaviour
    {
        public float cellSize;
        public Vector2Int gridSize;
        public GameObject tilePrefab;

        private BoardTile[,] tiles;

        private void Start()
        {
            tiles = new BoardTile[gridSize.x, gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
                for (int y = 0; y < gridSize.y; y++)
                    SetupTile(x, y);
        }

        private void SetupTile(int x, int y)
        {
            var tile = Instantiate(tilePrefab, transform);
            tile.transform.localPosition = new Vector3(x * cellSize, 0, y * cellSize);
            tile.name = $"Tile {x} {y}";
            
            tiles[x, y] = new BoardTile();
        }

        public bool IsValidTile(Transform _transform)
        {
            if (_transform != null && _transform.name.Contains("Tile"))
                return true;
            return false;
        }

        public bool IsValidTile(int x, int y)
        {
            return x >= 0 && x < tiles.Length && y >= 0 && y < tiles.Length;
        }

        public bool AreTilesOccupied(Vector2Int[] _tiles)
        {
            foreach (var tile in _tiles)
            {
                if (!IsValidTile(tile.x, tile.y))
                    continue;

                if (tiles[tile.x, tile.y].occupied)
                    return true;
            }
            return false;
            
        }

        public void SetOccupiedTiles(Vector2Int[] _tiles)
        {
            foreach (var tile in _tiles)
            {
                if (!IsValidTile(tile.x, tile.y))
                    continue;

                tiles[tile.x, tile.y].occupied = true;
            }
        }

        public Vector2Int[] GatherTiles(Vector2 _start, Vector2 _end, int _shipLength)
        {

            Vector2Int[] foundTiles = new Vector2Int[_shipLength];
            
            int x = (int)_start.x;
            int y = (int)_start.y;
            if (!IsValidTile(x, y) || !IsValidTile((int)_end.x, (int)_end.y))
                return foundTiles;

            var direction = GetDirection(_start, _end);

            for (int i = 0; i < _shipLength; i++)
            {
                if (!IsValidTile(x, y))
                    continue;

                foundTiles[i] = new Vector2Int(x, y);
                x += direction.x;
                y += direction.y;
            }
            return foundTiles;
        }    

        public Vector2Int GetDirection(Vector2 _start, Vector2 _end)
        {
            Vector2 direction = _start - _end;
            if (direction.x > 0)
                return Vector2Int.left;
            else if (direction.x < 0)
                return Vector2Int.right;
            else if (direction.y > 0)
                return Vector2Int.down;
            else
                return Vector2Int.up;
        }

        public bool IsTileOccupied(int x, int y)
        {
            return IsValidTile(x, y) && tiles[x, y].occupied;
        }
    }
}