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
        List<Card> DeckList = new List<Card>();
        for (int i = 0; i < GameManager.Instance.ValueDeckCards; i++)
        {
            DeckList.Add(CardManagerList.AllCards[Random.Range(1, CardManagerList.AllCards.Count)]);

            //DeckList.Add(CardManagerList.AllCards[26]);
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

    private TextMeshProUGUI _playerPointsTMPro;
    private TextMeshProUGUI _enemyPointsTMPro;

    [SerializeField] private TextMeshProUGUI _playerDeckTMPro;
    [SerializeField] private TextMeshProUGUI _enemyDeckTMPro;

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

    public CardInfoScript StartChoseCard;
    private CardInfoScript _choosenCard;

    [HideInInspector] public List<CardInfoScript> PlayerHandCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> PlayerFieldCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> PlayerFieldInvulnerabilityCards = new List<CardInfoScript>();

    [HideInInspector] public List<CardInfoScript> EnemyHandCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> EnemyFieldCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> EnemyFieldInvulnerabilityCards = new List<CardInfoScript>();

    [HideInInspector] public bool IsChoosing;
    [HideInInspector] public bool IsHandCardPlaying;

    [HideInInspector] public UnityEvent<CardInfoScript> EnemyDropCardEvent;
    [HideInInspector] public UnityEvent<CardInfoScript> PlayerDropCardEvent;
    [HideInInspector] public UnityEvent<CardInfoScript> EnemyOrderCardEvent;
    [HideInInspector] public UnityEvent<CardInfoScript> PlayerOrderCard;

    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _endGamePanelWin;
    [SerializeField] private GameObject _endGamePanelLose;
    [SerializeField] private GameObject _endGamePanelDraw;

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

        _playerPointsTMPro = GameObject.Find("UI/MainCanvas/RightUI/Points/PlayerAllPointsImage/PlayerAllPoints").GetComponent<TextMeshProUGUI>();
        _enemyPointsTMPro = GameObject.Find("UI/MainCanvas/RightUI/Points/EnemyAllPointsImage/EnemyAllPoints").GetComponent<TextMeshProUGUI>();

        _playerField.GetComponent<DropField>().DropCard.AddListener(PlayerDropCartStartCoroutine);

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

        CurrentGame = new Game();

        //DebugGame();
        GiveHandCards(CurrentGame.EnemyDeck, _enemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, _playerHand);

        _playerDeckTMPro.text = CurrentGame.PlayerDeck.Count.ToString();
        _enemyDeckTMPro.text = CurrentGame.EnemyDeck.Count.ToString();

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
        }

        deck.RemoveAt(0);
    }

    private IEnumerator TurnFunk()
    {
        _turnTime = TurnDuration;

        _imageTurnTime[0].fillAmount = (float)_turnTime / TurnDuration;
        _imageTurnTime[1].fillAmount = (float)_turnTime / TurnDuration;

        if (IsPlayerTurn)
        {
            while (_turnTime-- > 0)
            {
                _imageTurnTime[0].fillAmount = (float)_turnTime / TurnDuration;
                _imageTurnTime[1].fillAmount = (float)_turnTime / TurnDuration;


                yield return new WaitForSeconds(1);

                if (_turnTime == 0 && !IsHandCardPlaying && PlayerHandCards.Count != 0)
                {
                    ThrowCard(PlayerHandCards[Random.Range(0, PlayerHandCards.Count)], true);
                }
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

        _playerDeckTMPro.text = CurrentGame.PlayerDeck.Count.ToString();
        _enemyDeckTMPro.text = CurrentGame.EnemyDeck.Count.ToString();

        CardMechanics.Instance.EndTurnActions();

        ChangeEnemyPoints();
        ChangePlayerPoints();

        StopAllCoroutines();

        _turn++;
        IsHandCardPlaying = false;
        EndTurnButton.interactable = IsPlayerTurn;
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

        EnemyDropCard(enemyHandCards[enemyPlayedCard]);

        ChangeTurn();
    }

    private void EnemyDropCard(CardInfoScript card)
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

        card.CheckStatusEffects();

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
            botChoosedCard = ChooseOurCard(false);
            CardMechanics.Instance.ChangePoints(botChoosedCard, card, true);

            if (card.SelfCard.RangeBoost > 0)
            {
                botChoosedCard.CheckSiblingIndex();

                if (botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeBoost) != null)
                {
                    for (int i = 0; i < botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeBoost).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeBoost)[i], card, true, false, false, i + 1);
                    }
                }

                if (botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeBoost) != null)
                {
                    for (int i = 0; i < botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeBoost).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeBoost)[i], card, true, false, false, i + 1);
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
            botChoosedCard = ChooseEnemyCard(false);
            CardMechanics.Instance.ChangePoints(botChoosedCard, card, true);

            if (card.SelfCard.RangeDamage > 0)
            {
                botChoosedCard.CheckSiblingIndex();

                if (botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeDamage) != null)
                {
                    for (int i = 0; i < botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeDamage).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeDamage)[i], card, true, false, false, i + 1);
                    }
                }

                if (botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeDamage) != null)
                {
                    for (int i = 0; i < botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeDamage).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeDamage)[i], card, true, false, false, i + 1);
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
            cardMove.PlayerMoveToField(_enemyField.GetComponent<DropField>(), _playerHand.GetComponent<DropField>().EmptyHandCard);

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
        card.CheckStatusEffects();

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
                    }
                }

                card.transform.GetComponent<ChoseCard>().enabled = false;

                _line.startColor = Color.white;
                _line.endColor = Color.green;

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
                    }
                }

                _line.startColor = Color.white;
                _line.endColor = Color.red;

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
    }

    private void ChangeEnemyPoints()
    {
        _enemyPoints = 0;

        foreach (CardInfoScript card in EnemyFieldCards)
        {
            _enemyPoints += card.ShowPoints(card.SelfCard);

            CardMechanics.Instance.CheckColorPointsCard(card);
        }

        ShowPoints();
    }

    private void ChangePlayerPoints()
    {
        _playerPoints = 0;

        foreach (CardInfoScript card in PlayerFieldCards)
        {
            _playerPoints += card.ShowPoints(card.SelfCard);

            CardMechanics.Instance.CheckColorPointsCard(card);
        }

        ShowPoints();
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
            return EnemyFieldCards[Random.Range(0, EnemyFieldCards.Count - 1)];
        }
    }

    private IEnumerator ChoseCardCoroutine(CardInfoScript card, bool isBoost, bool isDamage)
    {
        _choosenCard = null;
        StartChoseCard = card;
        card.ImageEdge1.color = Color.green;
        EndTurnButton.interactable = false;

        yield return StartCoroutine(WaitForChoseCard(card));

        if (isBoost)
        {
            if ((card.SelfCard.AddictionWithSelfField && (PlayerFieldCards.Count != 1 && PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count != 1)) ||
            (card.SelfCard.AddictionWithEnemyField && (EnemyFieldCards.Count != 0) &&
            (EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count != 0)))
            {
                CardMechanics.Instance.ChangePoints(card, card, false, true);
            }

            CardMechanics.Instance.ChangePoints(ChooseOurCard(true), card, true);

            foreach (CardInfoScript cardd in PlayerFieldCards)
            {
                cardd.transform.GetComponent<ChoseCard>().enabled = false;
                cardd.ImageEdge1.color = Color.white;
                cardd.IsOrderCard = false;
            }

            if (card.SelfCard.RangeBoost > 0)
            {
                ChooseOurCard(true).CheckSiblingIndex();

                if (ChooseOurCard(true).ReturnRightNearCard(card.SelfCard.RangeBoost) != null)
                {
                    for (int i = 0; i < ChooseOurCard(true).ReturnRightNearCard(card.SelfCard.RangeBoost).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(ChooseOurCard(true).ReturnRightNearCard(card.SelfCard.RangeBoost)[i], card, true, false, false, i + 1);
                    }
                }

                if (ChooseOurCard(true).ReturnLeftNearCard(card.SelfCard.RangeBoost) != null)
                {
                    for (int i = 0; i < ChooseOurCard(true).ReturnLeftNearCard(card.SelfCard.RangeBoost).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(ChooseOurCard(true).ReturnLeftNearCard(card.SelfCard.RangeBoost)[i], card, true, false, false, i + 1);
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

            CardMechanics.Instance.ChangePoints(ChooseEnemyCard(true), card, true);
            if (card.SelfCard.StatusEffects.IsStun)
            {
                ChooseEnemyCard(true).SelfCard.StatusEffects.IsStunned = true;
                ChooseEnemyCard(true).CheckStatusEffects();
            }

            foreach (CardInfoScript cardd in EnemyFieldCards)
            {
                cardd.transform.GetComponent<ChoseCard>().enabled = false;
                cardd.ImageEdge1.color = Color.white;
                cardd.IsOrderCard = false;
            }

            if (card.SelfCard.RangeDamage > 0)
            {
                ChooseEnemyCard(true).CheckSiblingIndex();

                if (ChooseEnemyCard(true).ReturnRightNearCard(card.SelfCard.RangeDamage) != null)
                {
                    for (int i = 0; i < ChooseEnemyCard(true).ReturnRightNearCard(card.SelfCard.RangeDamage).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(ChooseEnemyCard(true).ReturnRightNearCard(card.SelfCard.RangeDamage)[i], card, true, false, false, i + 1);

                        if (card.SelfCard.StatusEffects.IsStun)
                        {
                            ChooseEnemyCard(true).ReturnRightNearCard(card.SelfCard.RangeDamage)[i].SelfCard.StatusEffects.IsStunned = true;
                            ChooseEnemyCard(true).ReturnRightNearCard(card.SelfCard.RangeDamage)[i].CheckStatusEffects();
                        }
                    }
                }

                if (ChooseEnemyCard(true).ReturnLeftNearCard(card.SelfCard.RangeDamage) != null)
                {
                    for (int i = 0; i < ChooseEnemyCard(true).ReturnLeftNearCard(card.SelfCard.RangeDamage).Count; i++)
                    {
                        CardMechanics.Instance.ChangePoints(ChooseEnemyCard(true).ReturnLeftNearCard(card.SelfCard.RangeDamage)[i], card, true, false, false, i + 1);

                        if (card.SelfCard.StatusEffects.IsStun)
                        {
                            ChooseEnemyCard(true).ReturnLeftNearCard(card.SelfCard.RangeDamage)[i].SelfCard.StatusEffects.IsStunned = true;
                            ChooseEnemyCard(true).ReturnLeftNearCard(card.SelfCard.RangeDamage)[i].CheckStatusEffects();
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

        _line.SetPosition(0, UnityEngine.Vector3.zero);
        _line.SetPosition(1, UnityEngine.Vector3.zero);

        card.ImageEdge1.color = Color.white;
        EndTurnButton.interactable = true;

        ChangeEnemyPoints();
        ChangePlayerPoints();

        PlayerOrderCard.Invoke(card);
    }

    private IEnumerator WaitForChoseCard(CardInfoScript card)
    {
        _choosenCard = null;

        while (_choosenCard == null)
        {

            _line.SetPosition(0, card.transform.position);
            _line.SetPosition(1, _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1)));

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
        _endGamePanel.SetActive(true);

        if (_playerPoints < _enemyPoints)
        {
            _endGamePanelLose.SetActive(true);
        }

        else if (_playerPoints > _enemyPoints)
        {
            _endGamePanelWin.SetActive(true);
        }

        else
        {
            _endGamePanelDraw.SetActive(true);
        }
    }

    public void NewGame()
    {
        _endGamePanel.SetActive(false);
        _endGamePanelWin.SetActive(false);
        _endGamePanelLose.SetActive(false);
        _endGamePanelDraw.SetActive(false);

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
