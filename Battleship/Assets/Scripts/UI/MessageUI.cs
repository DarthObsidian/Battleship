using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Board;

public class MessageUI : MonoBehaviour
{
    UIDocument uiDocument;
    VisualElement messageRoot;
    Button okayButton;
    Label messageText;

    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        messageRoot = uiDocument.rootVisualElement.Q<VisualElement>("MessageRoot");
        okayButton = messageRoot.Q<Button>("Okay");
        messageText = messageRoot.Q<Label>("MessageText");

        okayButton.clicked += HandleOkayClicked;
        TurnManager.Message += HandleMessage;
    }

    private void HandleOkayClicked()
    {
        TranslateUI(true);
    }

    private void HandleMessage(string _message)
    {
        Debug.Log("obtained message");
        messageText.text = _message;
        TranslateUI(false);
    }

    private void TranslateUI(bool _state)
    {
        if (_state)
            messageRoot.style.translate = messageRoot.style.translate = new Translate(0, Length.Percent(150), 0);
        else
            messageRoot.style.translate = new Translate(0, 0, 0);
    }
}
