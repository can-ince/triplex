using Game.Scripts.Behaviours;
using Game.Scripts.Helpers;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GridController : Singleton<GridController>
    {
        [Header("Grid Options")]
        [Tooltip("Grid Size to be generated (n x n)")]
        [SerializeField] private int gridSize = 5;
        [SerializeField] private CellBehaviour cellPrefab;
        [SerializeField] private float gridPadding = 0.1f;
        [SerializeField] private Transform gridParent;
        private CellBehaviour [,] _cells;
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
            // Clear cells

            foreach (var cell in _cells)
            {
                cell.Dispose();
            }
            
        }

        private void GenerateGrid()
        {
            var screenHeight = _mainCam.orthographicSize * 2;
            var screenWidth = screenHeight * _mainCam.aspect;
            
            // Cell size is determined by the smallest part of the screen
            float cellSize = Mathf.Min(screenWidth, screenHeight) / gridSize;
        
            _cells = new CellBehaviour [gridSize, gridSize];
            
            // Start at the top left of the screen
            Vector2 startPos = new Vector2(-screenWidth / 2 + cellSize / 2, screenHeight / 2 - cellSize / 2);

            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    Vector2 position = new Vector2(startPos.x + col * cellSize, startPos.y - row * cellSize);
                    var cell = Instantiate(cellPrefab, position, Quaternion.identity, gridParent);
                    cell.transform.localScale = Vector3.one * (cellSize - gridPadding);

                    cell.Initialize();
                    cell.OnCellClicked += OnCellClicked;
                    _cells[row, col] = cell;
                }
            }
        }

        private void OnCellClicked(CellBehaviour cell)
        {
            //todo: check for marked cells  
        }
    }
}
