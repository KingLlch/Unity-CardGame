using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class Game
    {
        public List<Card> EnemyDeck, PlayerDeck, EnemyHand, PlayerHand, EnemyField, PlayerField;

        public Game()
        {
            EnemyDeck = GiveDeckCard();
            PlayerDeck = GiveDeckCard();

            EnemyHand = new List<Card>();
            PlayerHand = new List<Card>();

            EnemyField = new List<Card>();
            PlayerField = new List<Card>();
        }

        List<Card> GiveDeckCard()
        {
            List<Card> list = new List<Card>();
            for(int i = 0; i < 10; i++)
            {
                list.Add(CardManagerList.AllCards[Random.Range(0,CardManagerList.AllCards.Count)]);
            }
                return list;
        }
    }

public class GameManager : MonoBehaviour
{
    public Game CurrentGame;
    public Transform EnemyHand, PlayerHand;

    private void Start()
    {
        CurrentGame = new Game();
    }

    private void GiveHandCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while (i++ < 10)
        {
            GiveCardtoHand(deck,hand);
        }
    }

    private void GiveCardtoHand(List<Card> deck, Transform hand)
    {
        throw new System.NotImplementedException();
    }
}
