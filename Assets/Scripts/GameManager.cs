using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

    private TextMeshProUGUI _playerPointsTMPro;
    private TextMeshProUGUI _enemyPointsTMPro;

    private Camera _mainCamera;
    private UnityEngine.UI.Image[] _imageTurnTime = new UnityEngine.UI.Image[2];
    private LineRenderer _line;

    private int _turn;
    private int _turnTime;
    private int _playerPoints;
    private int _enemyPoints;

    public bool IsDrag;

    public GameObject CardPref;
    public UnityEngine.UI.Button EndTurnButton;

    public List<CardInfoScript> PlayerHandCards = new List<CardInfoScript>();
    public List<CardInfoScript> PlayerFieldCards = new List<CardInfoScript>();

    public List<CardInfoScript> EnemyHandCards = new List<CardInfoScript>();
    public List<CardInfoScript> EnemyFieldCards = new List<CardInfoScript>();

    public Color ColorBase;
    public bool IsChoosing;
    public bool IsSingleCardPlaying;

    private CardInfoScript _choosenCard;

    public UnityEvent<CardInfoScript> EnemyDropCardEvent;

    public UnityEvent<CardInfoScript> OrderCard;

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

        _playerPointsTMPro = GameObject.Find("UI/MainCanvas/RightUI/Points/PlayerAllPointsImage/PlayerAllPoints").GetComponent<TextMeshProUGUI>();
        _enemyPointsTMPro = GameObject.Find("UI/MainCanvas/RightUI/Points/EnemyAllPointsImage/EnemyAllPoints").GetComponent<TextMeshProUGUI>();

        _playerField.GetComponent<DropField>().DropCard.AddListener(PlayerDropCard);

        _imageTurnTime[0] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime").GetComponent<UnityEngine.UI.Image>();
        _imageTurnTime[1] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime1").GetComponent<UnityEngine.UI.Image>();
        _line = GameObject.Find("UI/MainCanvas/Line").GetComponent<LineRenderer>();
        _mainCamera = Camera.main;
    }


    private void Start()
    {
        _turn = 0;
        _playerPoints = 0;
        _enemyPoints = 0;

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

        cardHand.GetComponent<ChoseCard>().enabled = false;

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

        for (int i = 0; i < 1; i++)
        {
            if (EnemyFieldCards.Count > 8) return;

            enemyHandCards[0].ShowCardInfo(enemyHandCards[0].SelfCard);
            enemyHandCards[0].transform.SetParent(_enemyField);

            EnemyDropCard(enemyHandCards[0]);
        }
    }

    public void ChangeTurn()
    {
        EndTurnActions();

        StopAllCoroutines();

        _turn++;
        IsSingleCardPlaying = false;
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

    private void EnemyDropCard(CardInfoScript card)
    {
        EnemyHandCards.Remove(card);
        EnemyFieldCards.Add(card);
        ChangeEnemyPoints();

        if (card.SelfCard.Boost != 0)
        {
            if (EnemyFieldCards.Count == 1) return;

            ChangePoints(ChooseOurCard(false), card);
        }

        if (card.SelfCard.Damage != 0)
        {
            if (PlayerFieldCards.Count == 0) return;

            ChangePoints(ChooseEnemyCard(false), card);

            ChangePlayerPoints();
        }

        if ((card.SelfCard.SelfBoost != 0) || (card.SelfCard.SelfDamage != 0))
        {
            ChangePoints(card, card);
            OrderCard.Invoke(card);
        }

        ChangeEnemyPoints();
        EnemyDropCardEvent.Invoke(card);
    }

    private void PlayerDropCard(CardInfoScript card)
    {
        IsSingleCardPlaying = true;

        PlayerHandCards.Remove(card);
        PlayerFieldCards.Add(card);
        ChangePlayerPoints();

        if (card.SelfCard.Boost != 0)
        {
            if (PlayerFieldCards.Count == 1) return;

            foreach (CardInfoScript cardd in PlayerFieldCards)
            {
                cardd.transform.GetComponent<ChoseCard>().enabled = true;
            }

            StartCoroutine(ChoseCardCoroutine(card, card.SelfCard.Boost != 0, card.SelfCard.Damage != 0));
        }

        if (card.SelfCard.Damage != 0)
        {
            if (EnemyFieldCards.Count == 0) return;

            foreach (CardInfoScript cardd in EnemyFieldCards)
            {
                cardd.transform.GetComponent<ChoseCard>().enabled = true;
            }

            StartCoroutine(ChoseCardCoroutine(card, card.SelfCard.Boost != 0, card.SelfCard.Damage != 0));
        }

        if ((card.SelfCard.SelfBoost != 0) || (card.SelfCard.SelfDamage != 0))
        {
            ChangePoints(card, card);
            OrderCard.Invoke(card);
        }

        ChangePlayerPoints();
    }

    private void ChangeEnemyPoints()
    {
        _enemyPoints = 0;

        foreach (CardInfoScript card in EnemyFieldCards)
        {
            _enemyPoints += card.ShowPoints(card.SelfCard);

            CheckColorPointsCard(card);
        }

        ShowPoints();
    }

    private void ChangePlayerPoints()
    {
        _playerPoints = 0;

        foreach (CardInfoScript card in PlayerFieldCards)
        {
            _playerPoints += card.ShowPoints(card.SelfCard);

            CheckColorPointsCard(card);
        }

        ShowPoints();
    }

    private void CheckColorPointsCard(CardInfoScript card)
    {

        if (card.SelfCard.Points == card.SelfCard.MaxPoints)
        {
            card.Point.color = Color.black;
        }

        else if (card.SelfCard.Points < card.SelfCard.MaxPoints)
        {
            card.Point.color = Color.red;
        }

        else
        {
            card.Point.color = Color.green;
        }
    }

    private void ShowPoints()
    {
        _playerPointsTMPro.text = _playerPoints.ToString();
        _enemyPointsTMPro.text = _enemyPoints.ToString();
    }

    private CardInfoScript ChooseEnemyCard(bool isPlayerChoose)
    {
        if (isPlayerChoose)
        {
            return _choosenCard;
        }

        else
        {
            return PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)];
        }
    }

    private CardInfoScript ChooseOurCard(bool isPlayerChoose)
    {
        if (isPlayerChoose)
        {
            return _choosenCard;
        }

        else
        {
            return EnemyFieldCards[Random.Range(0, EnemyFieldCards.Count)];
        }
    }

    private void IsDestroyCard(CardInfoScript card)
    {
        if (card.SelfCard.Points <= 0)
        {
            if (PlayerFieldCards.Contains(card))
                PlayerFieldCards.Remove(card);

            else if (EnemyFieldCards.Contains(card))
                EnemyFieldCards.Remove(card);

            Destroy(card.DescriptionObject);
            Destroy(card.gameObject);
        }
    }

    private IEnumerator ChoseCardCoroutine(CardInfoScript card, bool isBoost, bool isDamage)
    {
        _choosenCard = null;

        card.ImageEdge.color = Color.green;
        card.transform.position += new UnityEngine.Vector3(0, 0, 0);
        EndTurnButton.interactable = false;

        yield return StartCoroutine(WaitForChoseCard(card));

        if (isBoost)
        {
            ChangePoints(ChooseOurCard(true), card);

            ChangePlayerPoints();

            foreach (CardInfoScript cardd in PlayerFieldCards)
            {
                cardd.transform.GetComponent<ChoseCard>().enabled = false;
            }

        }

        if (isDamage)
        {
            ChangePoints(ChooseEnemyCard(true), card);

            ChangeEnemyPoints();

            foreach (CardInfoScript cardd in EnemyFieldCards)
            {
                cardd.transform.GetComponent<ChoseCard>().enabled = false;
            }
        }

        _line.SetPosition(0, UnityEngine.Vector3.zero);
        _line.SetPosition(1, UnityEngine.Vector3.zero);

        card.ImageEdge.color = ColorBase;
        card.transform.position -= new UnityEngine.Vector3(0, 0, 0);
        EndTurnButton.interactable = true;

        OrderCard.Invoke(card);
    }

    private IEnumerator WaitForChoseCard(CardInfoScript card)
    {
        _choosenCard = null;

        while (_choosenCard == null)
        {

            _line.SetPosition(0, card.transform.position);
            _line.SetPosition(1, _mainCamera.ScreenToWorldPoint(Input.mousePosition));

            yield return null;
        }
    }

    public void ChoseCard(CardInfoScript card)
    {
        _choosenCard = card;
    }

    public void EndTurnActions()
    {
        if (IsPlayerTurn)
        {
            foreach (CardInfoScript card in PlayerFieldCards)
            {
                if (card.SelfCard.EndTurnAction == true)
                {
                    if ((card.SelfCard.EndTurnDamage != 0) && (EnemyFieldCards.Count > 0))
                    {
                        ChangePoints(EnemyFieldCards[Random.Range(0, EnemyFieldCards.Count)], card, true);
                    }

                    if ((card.SelfCard.EndTurnBoost != 0) && (PlayerFieldCards.Count > 0))
                    {
                        ChangePoints(PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)], card, true);
                    }
                }
            }

        }

        else
        {
            foreach (CardInfoScript card in EnemyFieldCards)
            {
                if (card.SelfCard.EndTurnAction == true)
                {
                    if ((card.SelfCard.EndTurnDamage != 0) && (PlayerFieldCards.Count > 0))
                    {
                        ChangePoints(PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)], card, true);
                    }

                    if ((card.SelfCard.EndTurnBoost != 0) && (EnemyFieldCards.Count > 0))
                    {
                        ChangePoints(EnemyFieldCards[Random.Range(0, EnemyFieldCards.Count)], card, true);
                    }
                }
            }
        }
    }

    private void ChangePoints(CardInfoScript targetCard, CardInfoScript startCard, bool endTurnAction = false)
    {
        if (startCard.SelfCard.Boost != 0) targetCard.ChangePoints(ref targetCard.SelfCard, startCard.SelfCard.Boost, startCard.SelfCard);
        if (startCard.SelfCard.Damage != 0) targetCard.ChangePoints(ref targetCard.SelfCard, -startCard.SelfCard.Damage, startCard.SelfCard);
        if (startCard.SelfCard.SelfBoost != 0) targetCard.ChangePoints(ref startCard.SelfCard, startCard.SelfCard.SelfBoost, startCard.SelfCard);
        if (startCard.SelfCard.SelfDamage != 0) targetCard.ChangePoints(ref startCard.SelfCard, -startCard.SelfCard.SelfDamage, startCard.SelfCard);

        if (endTurnAction)
        {
            if (startCard.SelfCard.EndTurnDamage != 0) targetCard.ChangePoints(ref targetCard.SelfCard, -startCard.SelfCard.EndTurnDamage, startCard.SelfCard);
            if (startCard.SelfCard.EndTurnBoost != 0) targetCard.ChangePoints(ref targetCard.SelfCard, startCard.SelfCard.EndTurnBoost, startCard.SelfCard);
        }

        IsDestroyCard(targetCard);
    }
}
