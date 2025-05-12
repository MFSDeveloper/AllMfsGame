using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<PokerCard> deck = new List<PokerCard>();
    private string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
    private string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
    private int[] values = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

    private void Start()
    {
        CreateDeck();
        ShuffleDeck();
    }

    public void CreateDeck()
    {
        deck.Clear();
        for (int s = 0; s < suits.Length; s++)
        {
            for (int r = 0; r < ranks.Length; r++)
            {
                deck.Add(new PokerCard(suits[s], ranks[r], values[r]));
            }
        }
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            PokerCard temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public PokerCard DealCard()
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("Deck is empty, recreating deck...");
            CreateDeck();
            ShuffleDeck();
        }

        PokerCard dealtCard = deck[0];
        deck.RemoveAt(0);
        return dealtCard;
    }
}
