using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private static DeckManager _instance;

    public static DeckManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DeckManager>();
            }

            return _instance;
        }
    }

    public List<Card> Deck;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDeck(List<Card> newDeck)
    {
        Deck = new List<Card>(newDeck);
    }
}
