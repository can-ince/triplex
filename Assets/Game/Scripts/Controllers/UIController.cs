using Game.Scripts.Helpers;
using Game.Scripts.UI;
using Game.Scripts.UI.Views;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class UIController : Singleton<UIController>
    {
        [SerializeField] private InGamePanelView inGamePanelView;
        public void Initialize()
        {
            inGamePanelView.Initialize();
            OpenInGamePanel();
        }
    
        public void Dispose()
        {
            inGamePanelView.Dispose();
        }
        
        public void OpenInGamePanel()
        {
            inGamePanelView.Open();
        }
        
        public void CloseInGamePanel()
        {
            inGamePanelView.Close();
        }
    }
}
