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
            
            var component = tile.GetComponent<BoardTile>();
            tiles[x, y] = component;
        }

        public bool IsValidTile(Transform _transform)
        {
            if (_transform != null && _transform.GetComponent<BoardTile>() != null)
                return true;
            return false;
        }
    }
}