using UnityEngine.UIElements;
using Entities;

public class ShipButton
{
    Label label;
    Ship ship;

    public void SetVisualElement(VisualElement _visualElement)
    {
        label = _visualElement.Q<Label>("ShipName");
    }

    public void SetData(Ship _ship)
    {
        ship = _ship;
        label.text = ship.name;
    }
}
