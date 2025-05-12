using UnityEngine;

[System.Serializable]
public class PokerCard
{
    public string suit;    // Hearts, Diamonds, Clubs, Spades
    public string rank;    // 2,3,4,5,6,7,8,9,10,J,Q,K,A
    public int value;      // 2-14 (2 = Two, 14 = Ace)

    public PokerCard(string suit, string rank, int value)
    {
        this.suit = suit;
        this.rank = rank;
        this.value = value;
    }
}
