using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<Sprite> deck; // List of all card images
    public Sprite cardBackSprite;
    private List<int> availableCards = new List<int>();

    private void Start()
    {
        ResetDeck();
    }

    public void ResetDeck()
    {
        availableCards.Clear();
        for (int i = 0; i < deck.Count; i++)
        {
            availableCards.Add(i);
        }
    }

    public int DealCard()
    {
        if (availableCards.Count == 0)
        {
            Debug.LogError("No more cards in the deck!");
            return -1;
        }

        int randomIndex = Random.Range(0, availableCards.Count);
        int cardId = availableCards[randomIndex];
        availableCards.RemoveAt(randomIndex);

        return cardId;
    }
}