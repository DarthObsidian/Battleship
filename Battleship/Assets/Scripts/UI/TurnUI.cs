using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Board;

public class TurnUI : MonoBehaviour
{

    UIDocument uiDocument;
    VisualElement turnRoot;
    Button okayButton;
    Label currentTurnLabel;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        turnRoot = uiDocument.rootVisualElement.Q<VisualElement>("TurnNotification");
        currentTurnLabel = uiDocument.rootVisualElement.Q<Label>("TurnNameLabel");
        okayButton = uiDocument.rootVisualElement.Q<Button>("Okay");

        okayButton.clicked += OnOkayButtonClick;
        TurnManager.TurnFinished += HandleTurnFinished;
    }

    private void OnOkayButtonClick()
    {
        TurnManager.PlayerReady?.Invoke();
        turnRoot.style.translate = new Translate(0, Length.Percent(-300), 0);
    }

    private void HandleTurnFinished(bool _isPlayerTurn)
    {
        string output = _isPlayerTurn ? "PLAYER" : "ENEMY";
        currentTurnLabel.text = output;
        turnRoot.style.translate = new Translate(0, 0, 0);
    }
}
