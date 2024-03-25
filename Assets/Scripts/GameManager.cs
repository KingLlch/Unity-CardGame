using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        private List<Card> GiveDeckCard()
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
    private Game _currentGame;
    private Transform _enemyHand;
    private Transform _playerHand;

    private Image[] _imageTurnTime = new Image[2];

    private int _turn;
    private int _turnTime;

    public GameObject CardPref;
    public Button EndTurnButton;


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
        _imageTurnTime[0] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime").GetComponent<Image>();
        _imageTurnTime[1] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime1").GetComponent<Image>();
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
            GiveCardtoHand(deck,hand);
        }
    }

    private void GiveCardtoHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0) return;

        Card card = deck[0];

        GameObject cardGo = Instantiate(CardPref, hand, false);

        if (hand == _enemyHand) cardGo.GetComponent<CardInfoScript>().HideCardInfo(card);
        else cardGo.GetComponent<CardInfoScript>().ShowCardInfo(card);

        deck.RemoveAt(0);
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
                _imageTurnTime[0].fillAmount = (float) _turnTime / 30;
                _imageTurnTime[1].fillAmount = (float) _turnTime / 30;
                yield return new WaitForSeconds(1);
            }
        }

        else
        {
            while (_turnTime-- > 27)
            {
                _imageTurnTime[0].fillAmount = (float) _turnTime / 30;
                _imageTurnTime[1].fillAmount = (float) _turnTime / 30;
                yield return new WaitForSeconds(1);
            }
        }

        ChangeTurn();
    }
}
