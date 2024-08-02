using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck : MonoBehaviour, IPointerClickHandler
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
        transform.GetComponent<Image>().raycastTarget = true;
        DeckClosePanel.SetActive(true);
    }

    public void CreateDeck(List<Card> deck)
    {
        foreach (Card card in deck)
        {
            GameObject cardGameObject = Instantiate(CardPref, DeckPanel.transform, false);

            DeckList.Add(cardGameObject);

            cardGameObject.GetComponent<ChoseCard>().enabled = false;
            cardGameObject.GetComponent<CardMove>().enabled = false;

            cardGameObject.GetComponent<CardInfoScript>().ShowCardInfo(card);
        }

        ShuffleVisualDeck();
    }

    public void DeleteDeck()
    {
        for (int i = DeckList.Count - 1; i >= 0; i--)
        {
            GameObject card = DeckList[i];
            Destroy(card);
            DeckList.Remove(card);
        }
    }

    public void DeleteFirstCardFromDeck()
    {
        GameObject differenceCard = DeckList[0];
        Destroy(differenceCard);
        DeckList.Remove(differenceCard);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.GetComponent<Image>().raycastTarget = false;
        DeckClosePanel.SetActive(false);
    }

    public void ShuffleVisualDeck()
    {
        foreach (GameObject card in DeckList)
        {
            card.transform.SetSiblingIndex(Random.Range(0, DeckList.Count));
        }
    }
}
