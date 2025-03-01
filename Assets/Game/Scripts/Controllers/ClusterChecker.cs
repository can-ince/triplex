using System.Collections.Generic;
using Game.Scripts.Behaviours;
using Game.Scripts.Interfaces;

namespace Game.Scripts.Controllers
{
    public class ClusterChecker : IClusterChecker
    {
        /// <summary>
        /// Find all cells that are part of the same cluster
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="gridSize"></param>
        /// <param name="startCell"></param>
        /// <returns></returns>
        public List<CellBehaviour> GetCluster(CellBehaviour[,] grid, int gridSize, CellBehaviour startCell)
        {
            var cluster = new List<CellBehaviour>();
            bool[,] visited = new bool[gridSize, gridSize];
            FloodFill(grid, gridSize, startCell.Row, startCell.Column, cluster, visited);
            return cluster;
        }
    
        private void FloodFill(CellBehaviour[,] grid, int gridSize, int row, int col, List<CellBehaviour> cluster, bool[,] visited)
        {
            if (row < 0 || col < 0 || row >= gridSize || col >= gridSize)
                return;
            if (visited[row, col])
                return;
            visited[row, col] = true;

            var cell = grid[row, col];
            if (!cell.IsMarked)
                return;
        
            cluster.Add(cell);
        
            // Check for horizontal and vertical cell neighbors
            FloodFill(grid, gridSize, row - 1, col, cluster, visited);
            FloodFill(grid, gridSize, row + 1, col, cluster, visited);
            FloodFill(grid, gridSize, row, col - 1, cluster, visited);
            FloodFill(grid, gridSize, row, col + 1, cluster, visited);
        }
    }
}
