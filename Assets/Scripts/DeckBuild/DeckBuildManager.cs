using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuildManager : MonoBehaviour
{
    private static DeckBuildManager _instance;

    public static DeckBuildManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DeckBuildManager>();
            }

            return _instance;
        }
    }

    public GameObject CardPref;
    public GameObject CardInDeckPref;

    public GameObject DeckGameObject;
    public GameObject CardContentView;
    public TextMeshProUGUI CountCard;

    public AudioSource AudioSource;

    private List<Card> Deck = new List<Card>();
    private List<GameObject> AllCards = new List<GameObject>();

    private List<CardInfoScript> CardInfoDeckList = new List<CardInfoScript>();
    private List<CardInfoScript> RandomDeckList = new List<CardInfoScript>();

    private int CountCardInDeck;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        ChangeCountCard();

        foreach (Card card in CardManagerList.AllCards)
        {
            GameObject newCard = Instantiate(CardPref, Vector3.zero, Quaternion.identity, CardContentView.transform);
            newCard.transform.position = new Vector3(0, 0, 100);
            newCard.AddComponent<ClickCardOnDeckBuild>().IsMainCard = true;
            newCard.GetComponent<CardInfoScript>().ShowCardInfo(card);
            newCard.GetComponent<CardMove>().enabled = false;

            AllCards.Add(newCard);
            CardInfoDeckList.Add(newCard.GetComponent<CardInfoScript>());
        }
    }

    public void AddCard(CardInfoScript card)
    {
        CountCardInDeck++;

        ChangeCountCard();

        GameObject newCardInDeck = Instantiate(CardInDeckPref, Vector3.zero,Quaternion.identity, DeckGameObject.transform);
        newCardInDeck.transform.position = new Vector3(0, 0, 100);

        ClickCardOnDeckBuild click = newCardInDeck.AddComponent<ClickCardOnDeckBuild>();
        click.IsInDeck = true;
        click.CardInfoScript = card;

        CardInDeck cardInDeck = newCardInDeck.GetComponent<CardInDeck>();
        cardInDeck.Image.sprite = card.SelfCard.Sprite;
        cardInDeck.Name.text = card.Name.text + ": " + card.SecondName.text;
        cardInDeck.Points.text = card.Point.text;

        Deck.Add(card.SelfCard);

        CardSound(card);
    }

    public void RemoveCard(CardInfoScript card, GameObject cardInDeck) 
    {
        CountCardInDeck--;
        ChangeCountCard();

        Deck.Remove(card.SelfCard);
        card.GetComponent<ClickCardOnDeckBuild>().IsInDeck = false;

        Destroy(cardInDeck);
    }

    public void StartGame()
    {
        if (CountCardInDeck == 25)
        {
            DeckManager.Instance.SetDeck(Deck);
            SceneManager.LoadScene("Game");
        }
    }

    public void RandomDeck()
    {
        ClearDeck();

        RandomDeckList = new List<CardInfoScript>(CardInfoDeckList);

        for (int i = 0; i < 25; i++)
        {

            int random = Random.Range(0, RandomDeckList.Count);
            CardInfoScript newCard = RandomDeckList[random];

            AddCard(newCard);
            RandomDeckList.Remove(newCard);
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    private void ClearDeck()
    {
        Deck.Clear();
        CountCardInDeck = 0;

        for (int i = DeckGameObject.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(DeckGameObject.transform.GetChild(i).gameObject);
        }

        foreach (GameObject card in AllCards)
        {
            card.GetComponent<ClickCardOnDeckBuild>().IsInDeck = false;
        }
    }

    private void ChangeCountCard()
    {
        CountCard.text = CountCardInDeck.ToString();

        if (CountCardInDeck != 25)
        {
            CountCard.color = Color.red;
        }

        else
            CountCard.color = Color.green;
    }

    private void CardSound(CardInfoScript card)
    {
        AudioSource.clip = Resources.Load<AudioClip>("Sounds/Cards/Deployment/" + card.SelfCard.Name + Random.Range(0, 6));
        AudioSource.Play();
    }
}
