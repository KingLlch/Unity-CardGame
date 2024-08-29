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
    public TextMeshProUGUI NeedCountCard;

    public AudioSource AudioSourceVoice;
    public AudioSource AudioSourceEffects;

    private List<Card> Deck = new List<Card>();
    private List<GameObject> AllCards = new List<GameObject>();

    private List<CardInfoScript> CardInfoDeckList = new List<CardInfoScript>();
    private List<CardInfoScript> RandomDeckList = new List<CardInfoScript>();

    private int NeedCountCardInDeck = 20;
    private int CountCardInDeck = 0;

    public GameObject[] HowToPlayList;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        NeedCountCard.text = "/ " + NeedCountCardInDeck.ToString();

        ChangeCountCard();

        foreach (Card card in CardManagerList.AllCards)
        {
            GameObject newCard = Instantiate(CardPref, Vector3.zero, Quaternion.identity, CardContentView.transform);
            newCard.transform.position = new Vector3(0, 0, 100);
            newCard.AddComponent<ClickCardOnDeckBuild>().IsMainCard = true;
            CardInfoScript cardInfo = newCard.GetComponent<CardInfoScript>();
            cardInfo.ShowCardInfo(card);
            cardInfo.IsDeckBuildCard = true;
            newCard.GetComponent<CardMove>().enabled = false;

            AllCards.Add(newCard);
            CardInfoDeckList.Add(cardInfo);
        }

        float height = (Mathf.Ceil((float)CardContentView.transform.childCount / 6) * 150 + (Mathf.Ceil((float)CardContentView.transform.childCount / 6) - 1) * 100) - 1065 + 250;
        CardContentView.GetComponent<RectTransform>().sizeDelta = new Vector2(CardContentView.GetComponent<RectTransform>().sizeDelta.x, height);
        CardContentView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -height/2);

        if (Object.FindObjectOfType<HowToPlay>() != null)
            HowToPlay.Instance.HowToPlayDeckBuild(HowToPlayList);
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
        cardInDeck.Image.sprite = card.SelfCard.BaseCard.Sprite;
        cardInDeck.Name.text = card.Name.text + ": " + card.SecondName.text;
        cardInDeck.Points.text = card.Point.text;

        Deck.Add(card.SelfCard);

        float height = (DeckGameObject.transform.childCount * CardInDeckPref.GetComponent<RectTransform>().sizeDelta.y + DeckGameObject.transform.childCount * 5) - 1065 + 100; 

        if (height > 0)
            DeckGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(DeckGameObject.GetComponent<RectTransform>().sizeDelta.x, height);

        CardSound(card);
    }

    public void RemoveCard(CardInfoScript card, GameObject cardInDeck) 
    {
        CountCardInDeck--;
        ChangeCountCard();

        Deck.Remove(card.SelfCard);
        card.GetComponent<ClickCardOnDeckBuild>().IsInDeck = false;

        Destroy(cardInDeck);

        float height = DeckGameObject.transform.childCount * CardInDeckPref.GetComponent<RectTransform>().sizeDelta.y + DeckGameObject.transform.childCount * 5 - 1065 + 100;

        if (height > 0)
            DeckGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(DeckGameObject.GetComponent<RectTransform>().sizeDelta.x, height);
    }

    public void StartGame()
    {
        if (CountCardInDeck == NeedCountCardInDeck)
        {
            DeckManager.Instance.SetDeck(Deck);
            SceneManager.LoadScene("Game");
        }
    }

    public void RandomDeck()
    {
        RandomDeckList = new List<CardInfoScript>(CardInfoDeckList);
        int currentCardInDeck = CountCardInDeck;

        List<CardInfoScript> removeCards = new List<CardInfoScript>();

        foreach (CardInfoScript card in RandomDeckList)
        {
            if (Deck.Contains(card.SelfCard))
            {
                removeCards.Add(card);
            }
        }

        foreach (CardInfoScript card in removeCards)
        {
            RandomDeckList.Remove(card);
        }

        for (int i = 0; i < NeedCountCardInDeck - currentCardInDeck; i++)
        {
            int random = Random.Range(0, RandomDeckList.Count);
            CardInfoScript newCard = RandomDeckList[random];

            AddCard(newCard);
            RandomDeckList.Remove(newCard);
        }

        foreach (CardInfoScript card in CardInfoDeckList)
        {
            if (Deck.Contains(card.SelfCard))
            {
                card.GetComponent<ClickCardOnDeckBuild>().CardInDeck(card);
            }
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ClearDeck()
    {
        CountCardInDeck = 0;
        ChangeCountCard();

        for (int i = DeckGameObject.transform.childCount - 1; i >= 0; i--)
        {
            DeckGameObject.transform.GetChild(i).GetComponent<ClickCardOnDeckBuild>().CardRemoveFromDeck(DeckGameObject.transform.GetChild(i).GetComponent<ClickCardOnDeckBuild>().CardInfoScript);
            Destroy(DeckGameObject.transform.GetChild(i).gameObject);
        }

        Deck.Clear();

        DeckGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(DeckGameObject.GetComponent<RectTransform>().sizeDelta.x, 1075);
    }

    private void ChangeCountCard()
    {
        CountCard.text = CountCardInDeck.ToString();

        if (CountCardInDeck != NeedCountCardInDeck)
        {
            CountCard.color = Color.red;
        }

        else
            CountCard.color = Color.green;
    }

    private void CardSound(CardInfoScript card)
    {
        AudioSourceVoice.clip = Resources.Load<AudioClip>("Sounds/Cards/Deployment/" + card.SelfCard.BaseCard.Name + Random.Range(0, 6));
        AudioSourceVoice.Play();

        AudioSourceEffects.clip = card.SelfCard.BaseCard.CardPlaySound;
        AudioSourceEffects.Play();
    }
}
