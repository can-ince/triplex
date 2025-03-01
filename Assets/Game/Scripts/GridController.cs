using System;
using UnityEngine;

namespace Game.Scripts
{
    public class GridController : MonoBehaviour
    {
        [Header("Grid Options")]
        [Tooltip("Grid Size to be generated (n x n)")]
        public int GridSize = 5;
        public GameObject CellPrefab;
        public float GridPadding = 0.1f;
        
        private GameObject [,] _cells;
        private Camera _mainCam;

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Initialize()
        {
            _mainCam = Camera.main;
            GenerateGrid();
        }

        public void Dispose()
        {
            //todo: clear cells
        }

        private void GenerateGrid()
        {
            var screenHeight = _mainCam.orthographicSize * 2;
            var screenWidth = screenHeight * _mainCam.aspect;
            
            // Cell size is determined by the smallest part of the screen
            float cellSize = Mathf.Min(screenWidth, screenHeight) / GridSize;
        
            _cells = new GameObject [GridSize, GridSize];
            
            // Start at the top left of the screen
            Vector2 startPos = new Vector2(-screenWidth / 2 + cellSize / 2, screenHeight / 2 - cellSize / 2);

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Vector2 position = new Vector2(startPos.x + col * cellSize, startPos.y - row * cellSize);
                    GameObject cellObj = Instantiate(CellPrefab, position, Quaternion.identity, transform);
                    cellObj.transform.localScale = Vector3.one * (cellSize - GridPadding);

                    _cells[row, col] = cellObj;
                }
            }
        }
    }
}
