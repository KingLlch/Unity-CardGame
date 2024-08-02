using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game
{
    public List<Card> EnemyDeck;
    public List<Card> PlayerDeck;

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();
    }

    private List<Card> GiveDeckCard()
    {
        List<Card> DeckList = new List<Card>();
        for (int i = 0; i < GameManager.Instance.ValueDeckCards; i++)
        {
            DeckList.Add(CardManagerList.AllCards[Random.Range(1, CardManagerList.AllCards.Count)]);
            //DeckList.Add(CardManagerList.AllCards[30]);
        }
        return DeckList;
    }
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    public static Game CurrentGame;

    private Transform _enemyHand;
    private Transform _playerHand;
    private Transform _enemyField;
    private Transform _playerField;

    private int _turn;
    private int _turnTime;
    private int _playerPoints;
    private int _enemyPoints;

    public bool IsDrag;
    public bool IsChooseCard;
    public GameObject CardPref;

    public CardInfoScript StartChoseCard;
    private CardInfoScript _choosenCard;

    [HideInInspector] public List<CardInfoScript> PlayerHandCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> PlayerFieldCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> PlayerFieldInvulnerabilityCards = new List<CardInfoScript>();

    [HideInInspector] public List<CardInfoScript> EnemyHandCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> EnemyFieldCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> EnemyFieldInvulnerabilityCards = new List<CardInfoScript>();

    [HideInInspector] public List<CardInfoScript> CardsCanChoose = new List<CardInfoScript>();

    [HideInInspector] public bool IsChoosing;
    [HideInInspector] public bool IsHandCardPlaying;

    [HideInInspector] public UnityEvent<CardInfoScript> EnemyDropCardEvent;
    [HideInInspector] public UnityEvent<CardInfoScript> PlayerDropCardEvent;
    [HideInInspector] public UnityEvent<CardInfoScript> EnemyOrderCardEvent;
    [HideInInspector] public UnityEvent<CardInfoScript> PlayerOrderCard;

    private Camera _mainCamera;

    //ChangeGameCharacteristics
    public int MaxNumberCardInField = 10;
    public int TurnDuration = 30;
    public int ValueDeckCards = 25;
    public int ValueHandCards = 10;

    public bool IsPlayerTurn
    {
        get
        {
            return _turn % 2 == 0;
        }
    }

    private void DebugGame()
    {
        int i = 0;

        while (i++ < MaxNumberCardInField)
        {
            GameObject cardDebug = Instantiate(CardPref, _enemyField, false);
            cardDebug.GetComponent<CardInfoScript>().ShowCardInfo(CardManagerList.AllCards[0]);
            cardDebug.GetComponent<ChoseCard>().enabled = false;
            cardDebug.transform.SetParent(_enemyField);
            EnemyFieldCards.Add(cardDebug.GetComponent<CardInfoScript>());
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _enemyHand = GameObject.Find("UI/MainCanvas/EnemyHand/HandLayout").transform;
        _playerHand = GameObject.Find("UI/MainCanvas/PlayerHand/HandLayout").transform;
        _enemyField = GameObject.Find("UI/MainCanvas/EnemyTable/TableLayout").transform;
        _playerField = GameObject.Find("UI/MainCanvas/PlayerTable/TableLayout").transform;

        _playerField.GetComponent<DropField>().DropCard.AddListener(PlayerDropCartStartCoroutine);
        _enemyField.GetComponent<DropField>().DropCard.AddListener(PlayerDropCartStartCoroutine);

        _mainCamera = Camera.main;
    }


    private void Start()
    {
        _turn = 0;
        _playerPoints = 0;
        _enemyPoints = 0;

        CurrentGame = new Game();

        //DebugGame();
        Deck.Instance.CreateDeck(CurrentGame.PlayerDeck);
        GiveHandCards(CurrentGame.EnemyDeck, _enemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, _playerHand);

        StartCoroutine(TurnFunk());
    }

    private void GiveHandCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while (i++ < ValueHandCards)
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

            Deck.Instance.DeleteFirstCardFromDeck();
        }

        deck.RemoveAt(0);

        UIManager.Instance.ChangeDeckCount(CurrentGame);
    }

    private IEnumerator TurnFunk()
    {
        _turnTime = TurnDuration;

        UIManager.Instance.ChangeWick(_turnTime);

        if (IsPlayerTurn)
        {
            while (_turnTime-- > 0)
            {
                UIManager.Instance.ChangeWick(_turnTime);

                yield return new WaitForSeconds(1);

                if (_turnTime == 0 && !IsHandCardPlaying && PlayerHandCards.Count != 0)
                {
                    ThrowCard(PlayerHandCards[Random.Range(0, PlayerHandCards.Count)], true);
                }
            }

            if (IsChooseCard == true)
            {
                _choosenCard = CardsCanChoose[Random.Range(0, CardsCanChoose.Count)];
                yield return null;
            }

            ChangeTurn();
        }

        else
        {
            StartCoroutine(EnemyTurn(EnemyHandCards));
        }
    }

    private void ChangeTurn()
    {
        if ((PlayerHandCards.Count == 0) && (EnemyHandCards.Count == 0))
        {
            EndGame();
        }

        if (IsPlayerTurn && !IsHandCardPlaying && PlayerHandCards.Count != 0)
            ThrowCard(PlayerHandCards[Random.Range(0, PlayerHandCards.Count)], true);

        CardMechanics.Instance.EndTurnActions();

        ChangeEnemyPoints();
        ChangePlayerPoints();

        StopAllCoroutines();

        _turn++;
        IsHandCardPlaying = false;
        UIManager.Instance.ChangeEndTurnButtonInteractable(IsPlayerTurn);
        StartCoroutine(TurnFunk());
    }

    private IEnumerator EnemyTurn(List<CardInfoScript> enemyHandCards)
    {
        yield return new WaitForSeconds(1.0f);

        int enemyPlayedCard = Random.Range(0, enemyHandCards.Count);

        if ((EnemyFieldCards.Count >= MaxNumberCardInField) || (EnemyHandCards.Count == 0))
        {
            if (EnemyFieldCards.Count >= MaxNumberCardInField)
            {
                ThrowCard(enemyHandCards[enemyPlayedCard], false);
            }

            ChangeTurn();
            yield break;
        }

        if (!enemyHandCards[enemyPlayedCard].SelfCard.StatusEffects.IsInvisibility)
            enemyHandCards[enemyPlayedCard].GetComponent<CardMove>().EnemyMoveToField(_enemyField.transform);

        else
            enemyHandCards[enemyPlayedCard].GetComponent<CardMove>().EnemyMoveToField(_playerField.transform);

        yield return new WaitForSeconds(0.6f);

        enemyHandCards[enemyPlayedCard].ShowCardInfo(enemyHandCards[enemyPlayedCard].SelfCard);

        if (!enemyHandCards[enemyPlayedCard].SelfCard.StatusEffects.IsInvisibility)
            enemyHandCards[enemyPlayedCard].transform.SetParent(_enemyField);
        else
            enemyHandCards[enemyPlayedCard].transform.SetParent(_playerField);

        yield return StartCoroutine(EnemyDropCard(enemyHandCards[enemyPlayedCard]));

        ChangeTurn();
    }

    private IEnumerator EnemyDropCard(CardInfoScript card)
    {
        CardInfoScript botChoosedCard;

        EnemyHandCards.Remove(card);

        if (!card.SelfCard.StatusEffects.IsInvisibility)
        {
            EnemyFieldCards.Add(card);
            ChangeEnemyPoints();
            if (card.SelfCard.StatusEffects.IsInvulnerability)
                EnemyFieldInvulnerabilityCards.Add(card);
        }

        if (card.SelfCard.StatusEffects.IsInvisibility)
        {

            PlayerFieldCards.Add(card);
            ChangePlayerPoints();
            if (card.SelfCard.StatusEffects.IsInvulnerability)
                PlayerFieldInvulnerabilityCards.Add(card);
        }

        CardMechanics.Instance.CheckStatusEffects(card);

        EnemyDropCardEvent.Invoke(card);

        if (card.SelfCard.RangeBoost == -1)
        {
            for (int i = EnemyFieldCards.Count - 1; i >= 0; i--)
            {
                CardMechanics.Instance.ChangePoints(EnemyFieldCards[i], card, true);
            }
            EnemyOrderCardEvent.Invoke(card);
        }


        else if ((card.SelfCard.Boost != 0) && (EnemyFieldCards.Count != 1) && (EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count != 0))
        {
            botChoosedCard = ChooseCard(false);
            CardMechanics.Instance.ChangePoints(botChoosedCard, card, true);

            if (card.SelfCard.RangeBoost > 0)
            {
                botChoosedCard.CheckSiblingIndex();

                if (CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, true) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, true).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, true)[i], card, true, false, false, i + 1);
                    }
                }

                if (CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, false) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, false).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, false)[i], card, true, false, false, i + 1);
                    }
                }
            }

            EnemyOrderCardEvent.Invoke(card);
        }

        if (card.SelfCard.RangeDamage == -1)
        {
            for (int i = PlayerFieldCards.Count - 1; i >= 0; i--)
            {
                CardMechanics.Instance.ChangePoints(PlayerFieldCards[i], card, true);

                if (card.SelfCard.StatusEffects.IsStun)
                    PlayerFieldCards[i].SelfCard.StatusEffects.IsStunned = true;
            }

            EnemyOrderCardEvent.Invoke(card);
        }

        else if ((card.SelfCard.Damage != 0) && (PlayerFieldCards.Count != 0) && (PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count != 0))
        {
            botChoosedCard = ChooseCard(false, false);
            CardMechanics.Instance.ChangePoints(botChoosedCard, card, true);

            if (card.SelfCard.RangeDamage > 0)
            {
                botChoosedCard.CheckSiblingIndex();

                if (CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, true) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, true).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, true)[i], card, true, false, false, i + 1);
                    }
                }

                if (CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, false) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, false).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.RangeBoost, false)[i], card, true, false, false, i + 1);
                    }
                }
            }

            EnemyOrderCardEvent.Invoke(card);
        }

        if ((card.SelfCard.SelfBoost != 0 || card.SelfCard.SelfDamage != 0) && ((!card.SelfCard.AddictionWithSelfField || !card.SelfCard.AddictionWithEnemyField) ||
           (card.SelfCard.AddictionWithSelfField && (EnemyFieldCards.Count != 1) ||
           (card.SelfCard.AddictionWithEnemyField && (PlayerFieldCards.Count != 0)))))
        {
            CardMechanics.Instance.ChangePoints(card, card, false, true);
            EnemyOrderCardEvent.Invoke(card);
        }

        if (card.SelfCard.Summon && (!card.SelfCard.AddictionWithEnemyField) || (card.SelfCard.AddictionWithEnemyField && PlayerFieldCards.Count > 0))
        {
            EnemyOrderCardEvent.Invoke(card);
            CardMechanics.Instance.SpawnCard(card, false);
            ChangeEnemyPoints();
        }

        if (card.SelfCard.DrawCardCount != 0)
        {
            for (int i = 0; i < card.SelfCard.DrawCardCount; i++)
            {
                EffectsManager.Instance.DrawCardEffect(_enemyHand, false);
                UIManager.Instance.ChangeEndTurnButtonInteractable(false);
                yield return new WaitForSeconds(0.3f);
                UIManager.Instance.ChangeEndTurnButtonInteractable(true);
                EffectsManager.Instance.HideDrawCardEffect();
                GiveCardtoHand(CurrentGame.EnemyDeck, _enemyHand);
            }
        }

        ChangeEnemyPoints();
        ChangePlayerPoints();
    }

    private void PlayerDropCartStartCoroutine(CardInfoScript card)
    {
        StartCoroutine(PlayerDropCard(card));
    }

    private IEnumerator PlayerDropCard(CardInfoScript card)
    {
        IsHandCardPlaying = true;

        CardMove cardMove = card.GetComponent<CardMove>();
        cardMove.MoveTopHierarchy();

        if (!card.SelfCard.StatusEffects.IsInvisibility)
            cardMove.PlayerMoveToField(_playerField.GetComponent<DropField>(), _playerHand.GetComponent<DropField>().EmptyHandCard);

        if (card.SelfCard.StatusEffects.IsInvisibility)
            cardMove.PlayerMoveToField(_enemyField.GetComponent<DropField>(), _playerHand.GetComponent<DropField>().EmptyHandCard, true);

        yield return new WaitForSeconds(0.6f);

        card.IsAnimationCard = false;
        cardMove.MoveBackHierarchy();

        PlayerHandCards.Remove(card);

        if (!card.SelfCard.StatusEffects.IsInvisibility)
        {
            PlayerFieldCards.Add(card);
            ChangePlayerPoints();
            if (card.SelfCard.StatusEffects.IsInvulnerability)
                PlayerFieldInvulnerabilityCards.Add(card);
        }

        if (card.SelfCard.StatusEffects.IsInvisibility)
        {
            EnemyFieldCards.Add(card);
            ChangeEnemyPoints();
            if (card.SelfCard.StatusEffects.IsInvulnerability)
                EnemyFieldInvulnerabilityCards.Add(card);
        }
        CardMechanics.Instance.CheckStatusEffects(card);

        PlayerDropCardEvent.Invoke(card);

        if (card.SelfCard.RangeBoost == -1)
        {
            for (int i = PlayerFieldCards.Count - 1; i >= 0; i--)
            {
                CardMechanics.Instance.ChangePoints(PlayerFieldCards[i], card, true);
            }

            PlayerOrderCard.Invoke(card);
        }

        else if ((card.SelfCard.Boost != 0) && PlayerFieldCards.Count != 1 && (PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count != 1))
        {
            if (card.SelfCard.RangeBoost != -1)
            {
                foreach (CardInfoScript cardd in PlayerFieldCards)
                {
                    if (!cardd.SelfCard.StatusEffects.IsInvulnerability)
                    {
                        cardd.transform.GetComponent<ChoseCard>().enabled = true;
                        cardd.IsOrderCard = true;
                        CardsCanChoose.Add(cardd);
                    }

                    else
                    {
                        cardd.IsOrderCard = true;
                    }
                }

                card.transform.GetComponent<ChoseCard>().enabled = false;
                CardsCanChoose.Remove(card);


                UIManager.Instance.ChangeLineColor(Color.white, Color.green);

                StartCoroutine(ChoseCardCoroutine(card, card.SelfCard.Boost != 0, card.SelfCard.Damage != 0));
            }
        }

        if (card.SelfCard.RangeDamage == -1)
        {
            for (int i = EnemyFieldCards.Count - 1; i >= 0; i--)
            {
                CardMechanics.Instance.ChangePoints(EnemyFieldCards[i], card, true);

                if (card.SelfCard.StatusEffects.IsStun)
                    EnemyFieldCards[i].SelfCard.StatusEffects.IsStunned = true;
                CardMechanics.Instance.CheckStatusEffects(EnemyFieldCards[i]);
            }

            PlayerOrderCard.Invoke(card);
        }

        else if ((card.SelfCard.Damage != 0) && (EnemyFieldCards.Count != 0) && (EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count != 0))
        {
            if (card.SelfCard.RangeDamage != -1)
            {

                foreach (CardInfoScript cardd in EnemyFieldCards)
                {
                    if (!cardd.SelfCard.StatusEffects.IsInvulnerability)
                    {
                        cardd.transform.GetComponent<ChoseCard>().enabled = true;
                        cardd.IsOrderCard = true;
                        CardsCanChoose.Add(cardd);

                    }

                    else
                    {
                        cardd.IsOrderCard = true;
                    }
                }

                card.transform.GetComponent<ChoseCard>().enabled = false;
                CardsCanChoose.Remove(card);

                UIManager.Instance.ChangeLineColor(Color.white, Color.red);

                StartCoroutine(ChoseCardCoroutine(card, card.SelfCard.Boost != 0, card.SelfCard.Damage != 0));

            }
        }

        if (((card.SelfCard.SelfBoost != 0) || (card.SelfCard.SelfDamage != 0)) && (!card.SelfCard.AddictionWithSelfField && !card.SelfCard.AddictionWithEnemyField))
        {
            CardMechanics.Instance.ChangePoints(card, card, false, true);

            ChangeEnemyPoints();
            ChangePlayerPoints();

            PlayerOrderCard.Invoke(card);
        }

        if (card.SelfCard.Summon && !card.SelfCard.AddictionWithEnemyField)
        {
            PlayerOrderCard.Invoke(card);
            CardMechanics.Instance.SpawnCard(card, true);
            ChangePlayerPoints();
        }

        if (card.SelfCard.DrawCardCount != 0)
        {
            for (int i = 0; i < card.SelfCard.DrawCardCount; i++)
            {
                EffectsManager.Instance.DrawCardEffect(_playerHand, true);
              

                yield return new WaitForSeconds(0.3f);


                EffectsManager.Instance.HideDrawCardEffect();
                GiveCardtoHand(CurrentGame.PlayerDeck, _playerHand);
            }
        }
    }

    private void ChangeEnemyPoints()
    {
        _enemyPoints = 0;

        foreach (CardInfoScript card in EnemyFieldCards)
        {
            _enemyPoints += card.ShowPoints(card.SelfCard);
        }

        UIManager.Instance.ChangePoints(_playerPoints, _enemyPoints);
    }

    private void ChangePlayerPoints()
    {
        _playerPoints = 0;

        foreach (CardInfoScript card in PlayerFieldCards)
        {
            _playerPoints += card.ShowPoints(card.SelfCard);
        }

        UIManager.Instance.ChangePoints(_playerPoints, _enemyPoints);
    }

    private CardInfoScript ChooseCard(bool isPlayerChoose, bool isFriendlyCard = true)
    {
        if (isPlayerChoose)
        {
            return _choosenCard;
        }

        else
        {
            List<CardInfoScript> choosenCardList;

            if (!isFriendlyCard)
            {
                choosenCardList = PlayerFieldCards;

                foreach (CardInfoScript card in PlayerFieldInvulnerabilityCards)
                {
                    if (choosenCardList.Contains(card))
                        choosenCardList.Remove(card);
                }

                return choosenCardList[Random.Range(0, choosenCardList.Count - 1)];
            }

            else
            {
                choosenCardList = EnemyFieldCards;

                foreach (CardInfoScript card in EnemyFieldInvulnerabilityCards)
                {
                    if (choosenCardList.Contains(card))
                        choosenCardList.Remove(card);
                }

                return choosenCardList[Random.Range(0, choosenCardList.Count - 1)];
            }
        }
    }

    private IEnumerator ChoseCardCoroutine(CardInfoScript card, bool isBoost, bool isDamage)
    {
        StartChoseCard = card;
        card.ImageEdge1.color = Color.green;
        UIManager.Instance.ChangeEndTurnButtonInteractable(false);

        yield return StartCoroutine(WaitForChoseCard(card));
        IsChooseCard = false;

        if (isBoost)
        {
            if ((card.SelfCard.AddictionWithSelfField && (PlayerFieldCards.Count != 1 && PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count != 1)) ||
            (card.SelfCard.AddictionWithEnemyField && (EnemyFieldCards.Count != 0) &&
            (EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count != 0)))
            {
                CardMechanics.Instance.ChangePoints(card, card, false, true);
            }

            if (card.SelfCard.StatusEffects.IsShield)
                ChooseCard(true).SelfCard.StatusEffects.IsShielded = true;

            CardMechanics.Instance.ChangePoints(ChooseCard(true), card, true);

            foreach (CardInfoScript cardd in PlayerFieldCards)
            {
                cardd.transform.GetComponent<ChoseCard>().enabled = false;
                CardsCanChoose.Remove(cardd);
                cardd.ImageEdge1.color = Color.white;
                cardd.IsOrderCard = false;
            }

            if (card.SelfCard.RangeBoost > 0)
            {
                ChooseCard(true).CheckSiblingIndex();

                if (CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeBoost, true) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeBoost, true).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeBoost, true)[i], card, true, false, false, i + 1);
                    }
                }

                if (CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeBoost, false) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeBoost, false).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeBoost, false)[i], card, true, false, false, i + 1);
                    }
                }
            }
        }

        if (isDamage)
        {
            if ((card.SelfCard.AddictionWithSelfField && (PlayerFieldCards.Count != 1 && PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count != 1)) ||
            (card.SelfCard.AddictionWithEnemyField && (EnemyFieldCards.Count != 0)) &&
            (EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count != 0))
            {
                CardMechanics.Instance.ChangePoints(card, card, false, true);
            }

            CardMechanics.Instance.ChangePoints(ChooseCard(true, false), card, true);
            if (card.SelfCard.StatusEffects.IsStun)
            {
                ChooseCard(true, false).SelfCard.StatusEffects.IsStunned = true;
                CardMechanics.Instance.CheckStatusEffects(ChooseCard(true, false));
            }

            foreach (CardInfoScript cardd in EnemyFieldCards)
            {
                cardd.transform.GetComponent<ChoseCard>().enabled = false;
                CardsCanChoose.Remove(cardd);
                cardd.ImageEdge1.color = Color.white;
                cardd.IsOrderCard = false;
            }

            if (card.SelfCard.RangeDamage > 0)
            {
                ChooseCard(true, false).CheckSiblingIndex();

                if (CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeDamage, true) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeDamage, true).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeDamage, true)[i], card, true, false, false, i + 1);

                        if (card.SelfCard.StatusEffects.IsStun)
                        {
                            CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeDamage, true)[i].SelfCard.StatusEffects.IsStunned = true;
                            CardMechanics.Instance.CheckStatusEffects(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeDamage, true)[i]);
                        }
                    }
                }

                if (CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeDamage, false) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeDamage, false).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeDamage, false)[i], card, true, false, false, i + 1);

                        if (card.SelfCard.StatusEffects.IsStun)
                        {
                            CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeDamage, false)[i].SelfCard.StatusEffects.IsStunned = true;
                            CardMechanics.Instance.CheckStatusEffects(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), card.SelfCard.RangeDamage, false)[i]);
                        }
                    }
                }
            }

            if (card.SelfCard.RangeDamage == -1)
            {
                foreach (CardInfoScript cardd in EnemyFieldCards)
                {
                    CardMechanics.Instance.ChangePoints(cardd, card, true);
                }
            }
        }

        if (card.SelfCard.Summon && card.SelfCard.AddictionWithEnemyField && EnemyFieldCards.Count > 0 && EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count != 0)
        {
            CardMechanics.Instance.SpawnCard(card, true);
        }

        UIManager.Instance.ChangeLinePosition(0, Vector3.zero);
        UIManager.Instance.ChangeLinePosition(1, Vector3.zero);

        card.ImageEdge1.color = Color.white;
        UIManager.Instance.ChangeEndTurnButtonInteractable(true);

        ChangeEnemyPoints();
        ChangePlayerPoints();

        PlayerOrderCard.Invoke(card);
    }

    private IEnumerator WaitForChoseCard(CardInfoScript card)
    {
        IsChooseCard = true;
        _choosenCard = null;

        while (_choosenCard == null)
        {
            UIManager.Instance.ChangeLinePosition(0, new Vector3(card.transform.position.x, card.transform.position.y, 1));
            UIManager.Instance.ChangeLinePosition(1, _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1)));

            yield return null;
        }
    }

    public void ChoseCard(CardInfoScript card)
    {
        _choosenCard = card;
    }

    private void EndGame()
    {
        StopAllCoroutines();
        UIManager.Instance.EndGamePanel.SetActive(true);

        if (_playerPoints < _enemyPoints)
        {
            UIManager.Instance.EndGamePanelLose.SetActive(true);
        }

        else if (_playerPoints > _enemyPoints)
        {
            UIManager.Instance.EndGamePanelWin.SetActive(true);
        }

        else
        {
            UIManager.Instance.EndGamePanelDraw.SetActive(true);
        }
    }

    public void NewGame()
    {
        UIManager.Instance.EndGamePanel.SetActive(false);
        UIManager.Instance.EndGamePanelWin.SetActive(false);
        UIManager.Instance.EndGamePanelLose.SetActive(false);
        UIManager.Instance.EndGamePanelDraw.SetActive(false);

        _turn = 0;
        _playerPoints = 0;
        _enemyPoints = 0;

        for (int i = EnemyHandCards.Count - 1; i >= 0; i--)
        {
            Destroy(EnemyHandCards[i].gameObject);
            EnemyHandCards.Remove(EnemyHandCards[i]);
        }

        for (int i = PlayerHandCards.Count - 1; i >= 0; i--)
        {
            Destroy(PlayerHandCards[i].gameObject);
            PlayerHandCards.Remove(PlayerHandCards[i]);
        }

        for (int i = EnemyFieldCards.Count - 1; i >= 0; i--)
        {
            Destroy(EnemyFieldCards[i].gameObject);
            EnemyFieldCards.Remove(EnemyFieldCards[i]);
        }

        for (int i = PlayerFieldCards.Count - 1; i >= 0; i--)
        {
            Destroy(PlayerFieldCards[i].gameObject);
            PlayerFieldCards.Remove(PlayerFieldCards[i]);
        }

        PlayerFieldInvulnerabilityCards.Clear();
        EnemyFieldInvulnerabilityCards.Clear();

        CurrentGame = new Game();

        GiveHandCards(CurrentGame.EnemyDeck, _enemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, _playerHand);

        StartCoroutine(TurnFunk());
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ThrowCard(CardInfoScript card, bool isPlayer)
    {
        if (isPlayer)
        {
            PlayerHandCards.Remove(card);
        }
        else
        {
            EnemyHandCards.Remove(card);
        }

        Destroy(card.transform.gameObject);
    }
}
