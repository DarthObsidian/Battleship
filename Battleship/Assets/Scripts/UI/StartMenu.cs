using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class StartMenu : MonoBehaviour
{
    public static UnityAction StartClicked;

    UIDocument uiDocument;
    VisualElement startMenuRoot;
    Button startButton;
    
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        startMenuRoot = uiDocument.rootVisualElement.Q<VisualElement>("StartScreen");
        startButton = startMenuRoot.Q<VisualElement>("Body")
            .Q<Button>("StartButton");
        startMenuRoot.style.display = DisplayStyle.Flex;
        startButton.clicked += HandleStartClicked;
    }

    private void HandleStartClicked()
    {
        StartClicked?.Invoke();
        startMenuRoot.style.display = DisplayStyle.None;
    }
}
