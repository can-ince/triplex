using System;
using Game.Scripts.Behaviours;
using Game.Scripts.Helpers;
using Game.Scripts.Interfaces;
using Game.Scripts.UI.Views;
using UnityEngine;
using Zenject;

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
        [SerializeField] private int clusterSize = 3;

        private CellBehaviour[,] _cells;
        private Camera _mainCam;
        private IClusterChecker _clusterChecker;
        private int _currentMatchCount;

        public int CurrentMatchCount
        {
            get => _currentMatchCount;

            set
            {
                _currentMatchCount = value;
                OnMatchCountChanged?.Invoke(_currentMatchCount);
            }
        }
        public static event Action<int> OnMatchCountChanged;
        
        public void Initialize()
        {
            _mainCam = Camera.main;
            CurrentMatchCount = 0;

            GenerateGrid();
            
            InGamePanelView.RebuildButtonClicked += OnRebuildGridRequested;
        }

        public void Dispose()
        {
            InGamePanelView.RebuildButtonClicked -= OnRebuildGridRequested;
        }
        
        // Injecting ClusterChecker service via DI
        [Inject]
        public void Construct(IClusterChecker clusterChecker)
        {
            _clusterChecker = clusterChecker;
        }
        
        private void OnRebuildGridRequested(int newGridSize)
        {
            RebuildGrid(newGridSize);
        }

        private void OnCellClicked(CellBehaviour cell)
        {
            HandleCellClicked(cell);
        }

        private void GenerateGrid()
        {
            var screenHeight = _mainCam.orthographicSize * 2;
            var screenWidth = screenHeight * _mainCam.aspect;

            // Cell size is determined by the smallest part of the screen
            var cellSize = Mathf.Min(screenWidth, screenHeight) / gridSize;

            _cells = new CellBehaviour [gridSize, gridSize];

            // Start at the top left of the screen
            var startPos = new Vector2(-screenWidth * .5f + cellSize * .5f, screenHeight * .5f - cellSize * .5f);

            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    var position = new Vector2(startPos.x + col * cellSize, startPos.y - row * cellSize);
                    var cell = Instantiate(cellPrefab, position, Quaternion.identity, gridParent);
                    cell.transform.localScale = Vector3.one * (cellSize - gridPadding);

                    cell.Initialize(row, col);
                    cell.OnCellClicked += OnCellClicked;
                    _cells[row, col] = cell;
                }
            }
        }

        // Clear all marked cells if cluster size is target amount or more
        private void HandleCellClicked(CellBehaviour clickedCell)
        {
            var cluster = _clusterChecker.GetCluster(_cells, gridSize, clickedCell);

            if (cluster.Count >= clusterSize)
            {
                foreach (CellBehaviour cell in cluster)
                {
                    cell.ClearCell();
                }

                CurrentMatchCount++;
            }
        }
        
        private void RebuildGrid(int newGridSize)
        {
            gridSize = newGridSize;

            CurrentMatchCount = 0;
            
            // clear old cells
            ClearCells();
            
            GenerateGrid();
        }
        
        private void ClearCells()
        {
            foreach (var cell in _cells)
            {
                cell.Dispose();
            }
        }

    }
}