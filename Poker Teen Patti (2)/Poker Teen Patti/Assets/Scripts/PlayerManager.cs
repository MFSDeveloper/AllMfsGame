using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public PokerPanels pokerPanels;
    public List<Image> cardSlots;
    private CardManager cardManager;
    private PokerGameManager gameManager;
    public TMP_Text blindIndicatorText;

    public GameObject actionIndicatorPanel;
    public Image actionIndicatorIcon;
    public TMP_Text actionIndicatorText;

    private List<int> playerHand = new List<int>();
    private bool isMyTurn = false;

    public int balance = 10000;
    public TMP_Text balanceText;
    public TMP_Text actionMessageText;
    private int selectedRaiseAmount = 0;

    public bool hasFolded = false;
    [SerializeField] private Image playerAvatarImage;
    [SerializeField] private Sprite foldedAvatarSprite;
    [SerializeField] private Vector3 foldedAvatarScale = new Vector3(0.8f, 0.8f, 0.8f);
    [SerializeField] private Vector3 defaultAvatarScale = new Vector3(1f, 1f, 1f);

    public bool hasActed = false;


    void Start()
    {
        UpdateBalanceUI();

        if (CompareTag("HumanPlayer"))
        {
            pokerPanels.DisableActionButtons();
        }

        if (actionIndicatorPanel != null)
            actionIndicatorPanel.SetActive(false);

        if (actionIndicatorText != null)
            actionIndicatorText.text = "";

        if (actionMessageText != null)
            actionMessageText.text = "";
    }

    public void SetCardManager(CardManager cm) => cardManager = cm;
    public void SetGameManager(PokerGameManager gm) => gameManager = gm;

    public void ReceiveCard(Image cardImage, int cardId)
    {
        playerHand.Add(cardId);
        cardSlots[playerHand.Count - 1] = cardImage;
    }

    public void EnableTurn()
    {
        isMyTurn = true;
        if (CompareTag("HumanPlayer"))
            Invoke(nameof(EnableHumanTurn), 4f);
        else
            Invoke(nameof(BotAction), 4f);
    }

    public void DisableTurn()
    {
        isMyTurn = false;
        if (CompareTag("HumanPlayer"))
            pokerPanels.DisableActionButtons();
    }

    private void EnableHumanTurn()
    {
        pokerPanels.EnableActionButtons(); // Enable buttons after delay
    }

    private void BotAction()
    {
        if (!isMyTurn) return;

        int randomAction = Random.Range(0, 3);
        if (randomAction == 0) Call();
        else if (randomAction == 1) Raise();
        else Fold();
    }

    public void Fold()
    {
        if (CompareTag("HumanPlayer"))
            pokerPanels.DisableActionButtons();

        Debug.Log(gameObject.name + " Folded");

        hasFolded = true;
        ShowActionMessage("Fold");

        foreach (var slot in cardSlots)
        {
            if (slot != null)
                slot.gameObject.SetActive(false);
        }

        if (playerAvatarImage != null && foldedAvatarSprite != null)
        {
            playerAvatarImage.sprite = foldedAvatarSprite;
            playerAvatarImage.rectTransform.localScale = foldedAvatarScale; // ✅ Apply scale on fold
        }

        actionIndicatorPanel.gameObject.SetActive(false);
        gameManager.EndTurn();
    }

    public void Call()
    {
        if (CompareTag("HumanPlayer"))
            pokerPanels.DisableActionButtons();

        int callAmount = gameManager.currentBetAmount;

        if (balance >= callAmount)
        {
            DeductMoney(callAmount);
            ShowActionMessage("Call");
            actionIndicatorText.text = "" + callAmount;
            gameManager.AddToPot(callAmount);
        }
        else
        {
            callAmount = balance;
            DeductMoney(callAmount);
            ShowActionMessage("Call (All In)");
            actionIndicatorText.text = "" + callAmount;
            gameManager.AddToPot(callAmount);
        }

        gameManager.EndTurn();
    }

    public void Raise()
    {
        if (CompareTag("HumanPlayer"))
        {
            int minRaise = gameManager.currentBetAmount + 1;
            int maxRaise = balance;
            pokerPanels.ShowRaisePanel(minRaise, maxRaise);

            pokerPanels.raiseOkButton.onClick.RemoveAllListeners();
            pokerPanels.raiseOkButton.onClick.AddListener(() =>
            {
                selectedRaiseAmount = (int)pokerPanels.raiseSlider.value;

                DeductMoney(selectedRaiseAmount);
                ShowActionMessage("Raise");
                actionIndicatorText.text = "" + selectedRaiseAmount;

                pokerPanels.HideRaisePanel();
                pokerPanels.DisableActionButtons();
                gameManager.AddToPot(selectedRaiseAmount);
                gameManager.EndTurn();
            });
        }
        else
        {
            int minRaise = gameManager.currentBetAmount + 1;
            int maxRaise = Mathf.Min(balance, minRaise + Random.Range(10, 101));
            int botRaiseAmount = Mathf.Clamp(Random.Range(minRaise, maxRaise + 1), minRaise, balance);

            DeductMoney(botRaiseAmount);
            ShowActionMessage("Raise");
            actionIndicatorText.text = "" + botRaiseAmount;

            gameManager.AddToPot(botRaiseAmount);
            Debug.Log(gameObject.name + " Raised: " + botRaiseAmount);
            gameManager.EndTurn();
        }
    }

    public void DeductMoney(int amount)
    {
        balance -= amount;
        UpdateBalanceUI();
    }

    public void AddMoney(int amount)
    {
        balance += amount;
        UpdateBalanceUI();
    }

    private void UpdateBalanceUI()
    {
        if (balanceText != null)
            balanceText.text = balance.ToString();
    }

    public void ShowActionMessage(string message)
    {
        if (actionIndicatorPanel != null)
            actionIndicatorPanel.SetActive(true);

        if (actionIndicatorText != null)
            actionIndicatorText.text = message;

        if (actionMessageText != null)
            actionMessageText.text = message;
    }

    public void HideActionMessage()
    {
        if (actionMessageText != null)
            actionMessageText.text = "";

        if (actionIndicatorPanel != null)
            actionIndicatorPanel.SetActive(false);
    }

    // 🔄 Optional: Call this on new round to reset avatar
    public void ResetAvatar()
    {
        hasFolded = false;

        if (playerAvatarImage != null)
            playerAvatarImage.rectTransform.localScale = defaultAvatarScale;

        foreach (var slot in cardSlots)
        {
            if (slot != null)
                slot.gameObject.SetActive(true);
        }

        HideActionMessage();
    }
}
