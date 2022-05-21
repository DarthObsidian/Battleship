using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using Board;

namespace UI
{
    public class RadarLogic : MonoBehaviour
    {
        public Sprite hitSprite;
        public Sprite missSprite;
        public AILogic ai;

        UIDocument uiDocument;
        VisualElement radarRoot;
        VisualElement radarButtonRoot;
        Button grid;
        Button radarButton;

        private void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            radarRoot = uiDocument.rootVisualElement.Q<VisualElement>("RadarRoot");
            radarButtonRoot = uiDocument.rootVisualElement.Q<VisualElement>("RadarButtonRoot");
            radarButton = radarButtonRoot.Q<Button>("radarButton");
            grid = radarRoot.Q<AspectRatioPanel>("RadarAspectRatioPanel")
                .Q<VisualElement>("Body")
                .Q<Button>("Grid");

            radarButton.clicked += OnRadarButtonClick;
            grid.clicked += OnRadarGridClicked;
            TurnManager.AllShipsPlaced += ActivateButton;
            TurnManager.ToggleUI += HandleToggleUI;

            ToggleDisplay(radarRoot, false);
            ToggleDisplay(radarButtonRoot, false);
            grid.SetEnabled(false);
        }

        private void HandleToggleUI(bool _state)
        {
            grid.SetEnabled(_state);
        }

        private void OnRadarButtonClick()
        {
            bool currentState = radarRoot.style.display == DisplayStyle.Flex;
            ToggleDisplay(radarRoot, !currentState);
        }

        private void ActivateButton()
        {
            ToggleDisplay(radarButtonRoot, true);
        }

        private void ToggleDisplay(VisualElement _displayElement, bool _on)
        {
            if (_on)
                _displayElement.style.display = DisplayStyle.Flex;
            else
                _displayElement.style.display = DisplayStyle.None;
        }

        private void OnRadarGridClicked()
        {
            Vector2 relativePos = MouseToGrid();
            Vector2 cell = CalculateCellNumber(relativePos);

            if (cell.x < 0 || cell.y < 0)
                return;

            var uiCell = FindUICell(new Vector2(Mathf.Abs(relativePos.x), Mathf.Abs(relativePos.y)));
            if (uiCell == null)
                return;

            int x = (int)cell.x;
            int y = (int)cell.y;

            if (!ai.IsValidTile(x, y))
                return;

            if (ai.CheckIfTileGuessed(x, y))
                return;

            bool hit = ai.WasTileHit(x, y);

            if (hit)
                uiCell.style.backgroundImage = new StyleBackground(hitSprite);
            else
                uiCell.style.backgroundImage = new StyleBackground(missSprite);

            TurnManager.TurnFinished?.Invoke();
            grid.SetEnabled(false);
        }

        private Vector2 MouseToGrid()
        {
            var mousePos = Input.mousePosition;
            var x = mousePos.x - grid.worldBound.position.x;
            var y = mousePos.y - (grid.layout.height) - grid.worldBound.position.y;
            return new Vector2(x, y);
        }

        private Vector2 CalculateCellNumber(Vector2 _pos)
        {
            var cellSize = grid.layout.width / 11;

            int newX = (int)(Mathf.Abs(_pos.x) / cellSize);
            int newY = (int)(Mathf.Abs(_pos.y) / cellSize);
            newX--;
            newY--;
            return new Vector2(newX, newY);
        }

        private VisualElement FindUICell(Vector2 _pointerPos)
        {
            return grid.Children().Where(x => x.layout.Contains(_pointerPos)).FirstOrDefault();
        }
    }
}