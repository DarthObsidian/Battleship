using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using UnityEngine.UIElements;

namespace UI
{
    public class ShipButtonManager : MonoBehaviour
    {
        public Ship[] ships;
        public VisualTreeAsset shipUITemplate;

        UIDocument uiDocument;
        ListView listView;

        public void ToggleShipButtons(bool _enabled)
        {
            listView.SetEnabled(_enabled);
        }

        void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            listView = uiDocument.rootVisualElement.Q<ListView>("ButtonListView");
            listView.onSelectionChange += OnShipSelected;

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

            if (selectedShip == null)
                return;

            //TODO: make the ships appear and place!
        }
    }
}