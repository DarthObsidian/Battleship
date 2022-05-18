using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Entities;
using UnityEngine.UIElements;

namespace UI
{
    public class ShipButtonManager : MonoBehaviour
    {
        public List<Ship> ships = new List<Ship>();
        public VisualTreeAsset shipUITemplate;
        public static UnityAction<Ship> SendSelectedShip;
        public static UnityAction ShipPlaced;

        UIDocument uiDocument;
        ListView listView;

        void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            listView = uiDocument.rootVisualElement.Q<ListView>("ButtonListView");
            listView.onSelectionChange += OnShipSelected;
            ShipPlaced += OnShipPlaced;

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
                listView.visible = false;
        }
    }
}