using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Entities;
using UnityEngine.UIElements;
using Board;

namespace UI
{
    public class ShipButtonManager : MonoBehaviour
    {
        public List<Ship> ships = new List<Ship>();
        public VisualTreeAsset shipUITemplate;
        public static UnityAction<Ship> SendSelectedShip;
        public static UnityAction ShipPlaced;

        UIDocument uiDocument;
        VisualElement shipSelectionRoot;
        ListView listView;

        void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            shipSelectionRoot = uiDocument.rootVisualElement.Q<VisualElement>("ShipSelection");
            listView = shipSelectionRoot.Q<ListView>("ButtonListView");
            listView.onSelectionChange += OnShipSelected;
            ShipPlaced += OnShipPlaced;

            shipSelectionRoot.style.display = DisplayStyle.Flex;

            InitializeListOfShipButtons();
        }

        void InitializeListOfShipButtons()
        {
            listView.makeItem = () =>
            {
                var newEntry = shipUITemplate.Instantiate();
                var logic = new ShipButton();
                newEntry.userData = logic;

                logic.SetVisualElement(newEntry);
                return newEntry;
            };

            listView.bindItem = (item, index) =>
            {
                ShipButton shipButton = (item.userData as ShipButton);
                shipButton.SetData(ships[index]);
            };

            listView.fixedItemHeight = 100;
            listView.itemsSource = ships;
        }

        void OnShipSelected(IEnumerable<object> _selectedItems)
        {
            var selectedShip = listView.selectedItem as Ship;

            SendSelectedShip?.Invoke(selectedShip);
        }

        void OnShipPlaced()
        {
            ships.RemoveAt(listView.selectedIndex);
            listView.Rebuild();
            if (ships.Count <= 0)
            {
                shipSelectionRoot.style.display = DisplayStyle.None;
                TurnManager.AllShipsPlaced?.Invoke();
            }
        }
    }
}