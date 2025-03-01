using System.Collections.Generic;
using Game.Scripts.Behaviours;

namespace Game.Scripts.Interfaces
{
    public interface IClusterChecker 
    {
        List<CellBehaviour> GetCluster(CellBehaviour[,] grid, int gridSize, CellBehaviour startCell);
    }
}
