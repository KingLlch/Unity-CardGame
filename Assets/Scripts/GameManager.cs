using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Game
{
    public List<Card> EnemyDeck, PlayerDeck;

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();
    }

    private List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(CardManagerList.AllCards[Random.Range(0, CardManagerList.AllCards.Count)]);
        }
        return list;
    }
}

public class GameManager : MonoBehaviour
{
    private Game _currentGame;
    private Transform _enemyHand;
    private Transform _playerHand;
    private Transform _enemyField;
    private Transform _playerField;

    private UnityEngine.UI.Image[] _imageTurnTime = new UnityEngine.UI.Image[2];

    private int _turn;
    private int _turnTime;

    public GameObject CardPref;
    public UnityEngine.UI.Button EndTurnButton;

    public List<CardInfoScript> PlayerHandCards = new List<CardInfoScript>();
    public List<CardInfoScript> PlayerFieldCards = new List<CardInfoScript>();

    public List<CardInfoScript> EnemyHandCards = new List<CardInfoScript>();
    public List<CardInfoScript> EnemyFieldCards = new List<CardInfoScript>();

    public bool IsPlayerTurn
    {
        get
        {
            return _turn % 2 == 0;
        }
    }

    private void Awake()
    {
        _enemyHand = GameObject.Find("UI/MainCanvas/EnemyHand/HandLayout").transform;
        _playerHand = GameObject.Find("UI/MainCanvas/PlayerHand/HandLayout").transform;
        _enemyField = GameObject.Find("UI/MainCanvas/EnemyTable/TableLayout").transform;
        _playerField = GameObject.Find("UI/MainCanvas/PlayerTable/TableLayout").transform;

        _imageTurnTime[0] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime").GetComponent<UnityEngine.UI.Image>();
        _imageTurnTime[1] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime1").GetComponent<UnityEngine.UI.Image>();
    }

    private void Start()
    {
        _turn = 0;

        _currentGame = new Game();

        GiveHandCards(_currentGame.EnemyDeck, _enemyHand);
        GiveHandCards(_currentGame.PlayerDeck, _playerHand);

        StartCoroutine(TurnFunk());
    }

    private void GiveHandCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while (i++ < 10)
        {
            GiveCardtoHand(deck, hand);
        }
    }

    private void GiveCardtoHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0) return;

        Card card = deck[0];

        GameObject cardHand = Instantiate(CardPref, hand, false);

        if (hand == _enemyHand)
        {
            cardHand.GetComponent<CardInfoScript>().HideCardInfo(card);
            EnemyHandCards.Add(cardHand.GetComponent<CardInfoScript>());
        }

        else
        {
            cardHand.GetComponent<CardInfoScript>().ShowCardInfo(card);
            PlayerHandCards.Add(cardHand.GetComponent<CardInfoScript>());
        }

        deck.RemoveAt(0);
    }

    public void EnemyTurn(List<CardInfoScript> enemyHandCards)
    {
        int countCards = enemyHandCards.Count;

        if (countCards != 1)
        {
            countCards = Random.Range(0, enemyHandCards.Count);
        }
        else countCards = 1;

        for (int i = 0; i < countCards; i++)
        {
            if (EnemyFieldCards.Count > 8) return;

            enemyHandCards[0].ShowCardInfo(enemyHandCards[0].SelfCard);
            enemyHandCards[0].transform.SetParent(_enemyField);

            EnemyHandCards.Remove(enemyHandCards[0]);
            EnemyFieldCards.Add(enemyHandCards[0]);
        }
    }

    public void ChangeTurn()
    {
        StopAllCoroutines();

        _turn++;
        EndTurnButton.interactable = IsPlayerTurn;

        StartCoroutine(TurnFunk());
    }

    private IEnumerator TurnFunk()
    {
        _turnTime = 30;

        _imageTurnTime[0].fillAmount = (float)_turnTime / 30;
        _imageTurnTime[1].fillAmount = (float)_turnTime / 30;

        if (IsPlayerTurn)
        {
            while (_turnTime-- > 0)
            {
                _imageTurnTime[0].fillAmount = (float)_turnTime / 30;
                _imageTurnTime[1].fillAmount = (float)_turnTime / 30;
                yield return new WaitForSeconds(1);
            }
        }

        else
        {
            while (_turnTime-- > 27)
            {

                _imageTurnTime[0].fillAmount = (float)_turnTime / 30;
                _imageTurnTime[1].fillAmount = (float)_turnTime / 30;
                yield return new WaitForSeconds(1);
            }

            if (EnemyHandCards.Count > 0)
            {
                EnemyTurn(EnemyHandCards);
            }
        }

        ChangeTurn();
    }
}
