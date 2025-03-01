using System;
using Game.Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Views
{
    public class InGamePanelView : MonoBehaviour
    {
        [Header("UI refs")] 
        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI matchCountIndicator;
        [SerializeField] private TMP_InputField gridSizeField;
        [SerializeField] private Button rebuildButton;

        public static event Action<int> RebuildButtonClicked;

        public void Initialize()
        {
            GridController.OnMatchCountChanged += UpdateMatchCount;

            rebuildButton.onClick.AddListener(OnRebuildButtonClicked);
        }

        public void Dispose()
        {
            GridController.OnMatchCountChanged -= UpdateMatchCount;
            
            rebuildButton.onClick.RemoveListener(OnRebuildButtonClicked);
        }

        public void Open()
        {
            container.SetActive(true);
        }

        public void Close()
        {
            container.SetActive(false);
        }

        // Update match count coming from grid controller
        private void UpdateMatchCount(int count)
        {
            matchCountIndicator.text = "Match Count: " + count;
        }

        // Read gridSizeField and inform grid controller when rebuild button is clicked
        private void OnRebuildButtonClicked()
        {
            int newGridSize;
            if (int.TryParse(gridSizeField.text, out newGridSize) && newGridSize > 0)
            {
                RebuildButtonClicked?.Invoke(newGridSize);
            }
            else
            {
                Debug.LogError("Invalid grid size!");
            }
        }
    }
}