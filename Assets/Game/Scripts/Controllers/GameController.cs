using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Initialize()
        {
            UIController.Instance.Initialize();
            GridController.Instance.Initialize();
        }
    
        private void Dispose()
        {
            GridController.Instance.Dispose();
            UIController.Instance.Dispose();
        }
    }
}
