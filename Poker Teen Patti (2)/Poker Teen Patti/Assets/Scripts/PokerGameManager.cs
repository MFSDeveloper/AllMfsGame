using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokerGameManager : MonoBehaviour
{
    public List<PlayerManager> players;
    public CardManager cardManager;
    public GameObject cardPrefab;
    public Transform deckPosition;
    public Transform cardParent;
    public float cardDealDelay = 0.1f;
    public float turnTime = 10f;
    public int deckStackSize = 52;
    public TMP_Text potText; // Assign in inspector (center of table)

    private List<GameObject> visualDeckStack = new List<GameObject>();
    private int currentPlayerIndex = 0;
    private float timer;
    private bool gameStarted = false;
    private int dealerIndex = 0;
    private int potAmount = 0;
    public int currentBetAmount = 0;

    private int roundStartIndex = 0;
    private int roundEndIndex = 0;
    private bool isFirstRound = true;

    public List<Image> communityCardSlots; // 5 image slots for flop, turn, river
    private List<int> communityCardIDs = new List<int>();
    private int bettingRound = 0;

    private void Start()
    {
        foreach (var player in players)
        {
            player.SetCardManager(cardManager);
            player.SetGameManager(this);
        }

        GenerateVisualDeck();
        StartCoroutine(StartNewRound());
    }

    private void Update()
    {
        if (!gameStarted) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            EndTurn();
        }
    }

    private IEnumerator StartNewRound()
    {
        // Clear all blind texts
        foreach (var player in players)
        {
            player.blindIndicatorText.text = "";
        }

        potAmount = 0;
        UpdatePotUI();

        int smallBlindIndex = (dealerIndex + 1) % players.Count;
        int bigBlindIndex = (dealerIndex + 2) % players.Count;

        // SMALL BLIND
        players[smallBlindIndex].blindIndicatorText.text = "Small Blind";
        Debug.Log("Player " + smallBlindIndex + " is Small Blind");
        yield return new WaitForSeconds(3f);

        players[smallBlindIndex].blindIndicatorText.text = "+100";
        players[smallBlindIndex].DeductMoney(100);
        potAmount += 100;
        UpdatePotUI();
        Debug.Log("Player " + smallBlindIndex + " placed 100");
        yield return new WaitForSeconds(3f);
        players[smallBlindIndex].blindIndicatorText.text = "";

        // BIG BLIND
        players[bigBlindIndex].blindIndicatorText.text = "Big Blind";
        Debug.Log("Player " + bigBlindIndex + " is Big Blind");
        yield return new WaitForSeconds(3f);

        players[bigBlindIndex].blindIndicatorText.text = "+200";
        players[bigBlindIndex].DeductMoney(200);
        potAmount += 200;
        currentBetAmount = 200;
        UpdatePotUI();
        Debug.Log("Player " + bigBlindIndex + " placed 200");
        yield return new WaitForSeconds(3f);
        players[bigBlindIndex].blindIndicatorText.text = "";

        dealerIndex = (dealerIndex + 1) % players.Count;
        //currentPlayerIndex = (dealerIndex + 3) % players.Count;
        currentPlayerIndex = (bigBlindIndex + 1) % players.Count;

        StartCoroutine(DealCardsAnimation());
    }

    private void UpdatePotUI()
    {
        if (potText != null)
            potText.text = "POT : " + potAmount.ToString();
    }

    private void GenerateVisualDeck()
    {
        for (int i = 0; i < deckStackSize; i++)
        {
            GameObject cardGO = Instantiate(cardPrefab, deckPosition.position, Quaternion.identity, cardParent);
            Image cardImage = cardGO.GetComponent<Image>();
            cardImage.sprite = cardManager.cardBackSprite;

            //cardGO.transform.localScale = new Vector3(2.02f, 0.71f, 0.26f);


            cardGO.transform.localPosition += new Vector3(0, -i * 0.5f, 0);

            //cardGO.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-2f, 2f));
            visualDeckStack.Add(cardGO);
        }
    }

    private IEnumerator DealCardsAnimation()
    {
        cardManager.ResetDeck();

        for (int i = 0; i < 2; i++)
        {
            foreach (var player in players)
            {
                int cardId = cardManager.DealCard();

                if (visualDeckStack.Count == 0)
                {
                    Debug.LogError("Visual deck stack is empty!");
                    yield break;
                }

                GameObject cardGO = visualDeckStack[visualDeckStack.Count - 1];
                visualDeckStack.RemoveAt(visualDeckStack.Count - 1);
                cardGO.transform.SetParent(cardParent);

                Transform targetSlot = player.cardSlots[i].transform;
                Vector3 targetPos = targetSlot.position;
                print("Target: " + targetPos);

                //LeanTween.move(cardGO.GetComponent<RectTransform>(), targetPos, 0.5f).setEaseOutCubic();
                var v = LeanTween.move(cardGO.GetComponent<RectTransform>(), targetPos, 0.5f).setEaseOutCubic();
                print("Delay: " + v.delay);
                yield return new WaitForSeconds(v.delay);
                v.pause();

                Image cardImage = cardGO.GetComponent<Image>();
                Sprite assignedSprite = player.CompareTag("HumanPlayer") ? cardManager.deck[cardId] : cardManager.cardBackSprite;
                cardImage.sprite = assignedSprite;

                if (player.CompareTag("HumanPlayer"))
                {
                    cardGO.transform.localScale = new Vector3(0.59f, 0.82f, 1.88f);
                }
                cardGO.transform.SetParent(targetSlot);
                cardGO.transform.localPosition = Vector3.zero;
                player.ReceiveCard(cardImage, cardId);
            }
        }

        yield return new WaitForSeconds(0.15f);
        //StartNextBettingRound();
        StartPreFlopBetting();
    }


    private void StartPreFlopBetting()
    {
        bettingRound = 0; // Pre-Flop round
        int smallBlindIndex = (dealerIndex + 1) % players.Count;
        int bigBlindIndex = (dealerIndex + 2) % players.Count;

        roundStartIndex = (bigBlindIndex + 0) % players.Count;
        roundEndIndex = bigBlindIndex;
        currentPlayerIndex = roundStartIndex;

        Debug.Log("--- Pre-Flop Betting Started ---");

        StartPlayerTurn();
    }


    private void StartPlayerTurn()
    {
        timer = turnTime;
        players[currentPlayerIndex].EnableTurn();
        gameStarted = true;
    }

    private void ClearAllActionMessages()
    {
        foreach (var player in players)
        {
            player.HideActionMessage(); // This must exist in PlayerManager
        }
    }


    public void EndTurn()
    {
        players[currentPlayerIndex].DisableTurn();

        int nextPlayerIndex = currentPlayerIndex;

        do
        {
            nextPlayerIndex = (nextPlayerIndex + 1) % players.Count;
        }
        while (players[nextPlayerIndex].hasFolded && nextPlayerIndex != currentPlayerIndex);

        currentPlayerIndex = nextPlayerIndex;

        if (IsBettingRoundComplete())
        {
            CompleteBettingRound();
        }
        else
        {
            StartPlayerTurn();
        }
    }



    private bool IsBettingRoundComplete()
    {
        int activePlayerCount = 0;
        foreach (var player in players)
        {
            if (!player.hasFolded)
                activePlayerCount++;
        }

        // If only one player is left, no need for more betting
        if (activePlayerCount <= 1)
            return true;

        // If all players except currentPlayerIndex have acted, end the round
        return currentPlayerIndex == roundEndIndex;
    }


    private void CompleteBettingRound()
    {
        StartCoroutine(HandleBettingRoundCompletion());
    }

    private IEnumerator HandleBettingRoundCompletion()
    {
        gameStarted = false; // Prevent further turns
        players[currentPlayerIndex].DisableTurn(); // Just in case

        Debug.Log($"Betting round {bettingRound} completed. Preparing to reveal cards...");

        // Wait 1 second after big blind finishes action
        yield return new WaitForSeconds(1f);

        ClearAllActionMessages();

        if (bettingRound >= 3)
        {
            Debug.Log("All betting rounds complete. Proceeding to showdown.");
            // TODO: Add showdown logic here
            yield break;
        }

        // Move to next betting round
        bettingRound++;

        switch (bettingRound)
        {
            case 1:
                ShowCommunityCards(0, 3); // FLOP
                break;
            case 2:
                ShowCommunityCards(3, 1); // TURN
                break;
            case 3:
                ShowCommunityCards(4, 1); // RIVER
                break;
        }

        // Delay before next betting round begins
        yield return new WaitForSeconds(1f);

        int smallBlindIndex = (dealerIndex + 1) % players.Count;
        int bigBlindIndex = (dealerIndex + 2) % players.Count;

        roundStartIndex = (bigBlindIndex + 0) % players.Count;
        roundEndIndex = bigBlindIndex;
        currentPlayerIndex = roundStartIndex;

        while (players[currentPlayerIndex].hasFolded)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            // 🛑 Safety break: all folded?
            if (currentPlayerIndex == roundStartIndex)
            {
                Debug.LogWarning("All players folded! Ending round early.");
                yield break;
            }
        }

        Debug.Log($"---  Betting Round {bettingRound} Started ---");

        StartPlayerTurn();
    }


    // Add this method below in your script:
    private void ShowCommunityCards(int startIndex, int count)
    {
        ClearAllActionMessages();
        for (int i = 0; i < count; i++)
        {
            int cardId = cardManager.DealCard();
            communityCardIDs.Add(cardId);

            Image slot = communityCardSlots[startIndex + i];
            slot.sprite = cardManager.deck[cardId];
            slot.gameObject.SetActive(true);
        }
    }


    public void AddToPot(int amount)
    {
        potAmount += amount;
        UpdatePotUI();
    }
}
