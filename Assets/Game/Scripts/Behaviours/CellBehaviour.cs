using System;
using System.Collections;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class CellBehaviour : MonoBehaviour
    {
        [Header("Sprite Renderers")] 
        [SerializeField] private SpriteRenderer cellBg;
        [SerializeField] private SpriteRenderer cellX;

        [Header("Interaction Options")] 
        [SerializeField] private Color clickBgColor = Color.cyan;
        [SerializeField] private Color originalBgColor = Color.white;
        [SerializeField] private float animationDuration = 0.1f;

        private bool _isInteractable;
        private Coroutine _clickAnimationRoutine;
        
        public Action<CellBehaviour> OnCellClicked;
        
        public bool IsMarked { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        
        public void Initialize(int row, int column)
        {
            Row = row;
            Column = column;
            ClearCell();
            _isInteractable = true;
        }

        public void Dispose()
        {
            _isInteractable = false;

            if (_clickAnimationRoutine != null)
            {
                StopCoroutine(_clickAnimationRoutine);
                _clickAnimationRoutine = null;
            }
            
            Destroy(gameObject);
        }

        // Detect click on cell
        private void OnMouseDown()
        {
            if (!IsMarked && _isInteractable)
            {
                _clickAnimationRoutine = StartCoroutine(AnimateClick());
                
                MarkCell();
                
                OnCellClicked?.Invoke(this);
            }
        }

        // Add X mark
        private void MarkCell()
        {
            IsMarked = true;

            UpdateVisuals();
        }

        // Remove X mark
        public void ClearCell()
        {
            IsMarked = false;
            UpdateVisuals();
        }

        //Handle visuals of cell according to its state
        private void UpdateVisuals()
        {
            cellX.gameObject.SetActive(IsMarked);
        }

        IEnumerator AnimateClick()
        {
            float timer = 0f;
            var animStepDuration = animationDuration * .5f;
            // Lerp to click color
            while (timer < animStepDuration)
            {
                cellBg.color = Color.Lerp(originalBgColor, clickBgColor, timer / animStepDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            // Lerp back to original color
            timer = 0f;
            while (timer < animStepDuration)
            {
                cellBg.color = Color.Lerp(clickBgColor, originalBgColor, timer / animStepDuration);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}