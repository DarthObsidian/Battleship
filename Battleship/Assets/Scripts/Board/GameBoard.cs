using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    public class GameBoard
    {
        public float cellSize;
        public Vector2Int gridSize;
        public GameObject tilePrefab;

        private BoardTile[,] tiles;
        private int[,] shipMap;

        public GameBoard(Vector2Int _size)
        {
            gridSize = _size;
            shipMap = new int[gridSize.x, gridSize.y];
            tiles = new BoardTile[gridSize.x, gridSize.y];
        }

        public int GetShipMapItem(int x, int y)
        {
            return shipMap[x, y];
        }

        public void SetShipInMap(Vector2Int[] _tiles, int _shipId)
        {
            foreach (var tile in _tiles)
                shipMap[tile.x, tile.y] = _shipId;
        }

        public void SetTile(int x, int y)
        {
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
            return x >= 0 && x < tiles.GetLength(0) && y >= 0 && y < tiles.GetLength(1);
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

        public Vector2Int[] GatherTiles(Vector2 _start, Vector2 _end, int _length)
        {

            Vector2Int[] foundTiles = new Vector2Int[_length];
            
            int x = (int)_start.x;
            int y = (int)_start.y;
            if (!IsValidTile(x, y) || !IsValidTile((int)_end.x, (int)_end.y))
                return foundTiles;

            var direction = GetDirection(_start, _end);

            for (int i = 0; i < _length; i++)
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

        public void SetTileGuessed(int x, int y)
        {
            tiles[x, y].alreadyGuessed = true;
        }

        public List<Vector2Int> GetNotGuessedTiles()
        {
            List<Vector2Int> notGuessedTiles = new List<Vector2Int>();

            for (int x = 0; x < tiles.GetLength(0); x++)
                for (int y = 0; y < tiles.GetLength(1); y++)
                    if (!tiles[x, y].alreadyGuessed)
                        notGuessedTiles.Add(new Vector2Int(x, y));

            return notGuessedTiles;
        }

        public bool IsTileGuessed(int x, int y)
        {
            return tiles[x, y].alreadyGuessed;
        }
    }
}