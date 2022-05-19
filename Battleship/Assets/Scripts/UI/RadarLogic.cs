using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

namespace UI
{
    public class RadarLogic : MonoBehaviour
    {
        public Sprite hitSprite;
        public Sprite missSprite;

        UIDocument uiDocument;
        VisualElement radarRoot;
        VisualElement radarButtonRoot;
        Button grid;
        Button radarButton;

        void Start()
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
            ShipButtonManager.OutOfShips += ActivateButton;


            ToggleDisplay(radarRoot, false);
            ToggleDisplay(radarButtonRoot, false);
        }

        void OnRadarButtonClick()
        {
            bool currentState = radarRoot.style.display == DisplayStyle.Flex;
            ToggleDisplay(radarRoot, !currentState);
        }

        void ActivateButton()
        {
            ToggleDisplay(radarButtonRoot, true);
        }

        void ToggleDisplay(VisualElement _displayElement, bool _on)
        {
            if (_on)
                _displayElement.style.display = DisplayStyle.Flex;
            else
                _displayElement.style.display = DisplayStyle.None;
        }

        void OnRadarGridClicked()
        {
            Vector2 relativePos = MouseToGrid();
            Vector2 cell = CalculateCellNumber(relativePos);

            var uiCell = FindUICell(new Vector2(Mathf.Abs(relativePos.x), Mathf.Abs(relativePos.y)));
            if (uiCell != null)
                uiCell.style.backgroundImage = new StyleBackground(hitSprite);
            Debug.Log(cell);
        }

        Vector2 MouseToGrid()
        {
            var mousePos = Input.mousePosition;
            var x = mousePos.x - grid.worldBound.position.x;
            var y = mousePos.y - (grid.layout.height) - grid.worldBound.position.y;
            return new Vector2(x, y);
        }

        Vector2 CalculateCellNumber(Vector2 _pos)
        {
            var cellSize = grid.layout.width / 11;

            int newX = (int)(Mathf.Abs(_pos.x) / cellSize);
            int newY = (int)(Mathf.Abs(_pos.y) / cellSize);
            newX--;
            newY--;
            return new Vector2(newX, newY);
        }

        VisualElement FindUICell(Vector2 _pointerPos)
        {
            return grid.Children().Where(x => x.layout.Contains(_pointerPos)).FirstOrDefault();
        }
    }
}