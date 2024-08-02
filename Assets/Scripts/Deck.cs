using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private static Deck _instance;

    public static Deck Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Deck>();
            }

            return _instance;
        }
    }

    public GameObject CardPref;
    public GameObject DeckClosePanel;
    public GameObject DeckPanel;
    public List<GameObject> DeckList = new List<GameObject>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void ShowDeck()
    {
        DeckClosePanel.SetActive(true);
    }

    public void HideDeck()
    {
        DeckClosePanel.SetActive(false);
    }

    public void CreateDeck(List<Card> deck)
    {
        foreach (Card card in deck)
        {
            GameObject cardGameObject = Instantiate(CardPref, DeckPanel.transform, false);

            DeckList.Add(cardGameObject);

            cardGameObject.GetComponent<ChoseCard>().enabled = false;
            cardGameObject.GetComponent<CardInfoScript>().ShowCardInfo(card);
        }
    }

    public void DeleteFirstCardFromDeck()
    {
        GameObject differenceCard = DeckList[0];
        Destroy(differenceCard);
        DeckList.Remove(differenceCard);
    }
}
