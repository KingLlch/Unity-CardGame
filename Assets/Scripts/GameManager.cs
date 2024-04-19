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
        for (int i = 0; i < 10; i++)
        {
            DeckList.Add(CardManagerList.AllCards[Random.Range(1, CardManagerList.AllCards.Count)]);
        }
        return DeckList;
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
    [HideInInspector] public EffectsManager EffectsManager;

    public CardInfoScript StartChoseCard;
    private CardInfoScript _choosenCard;

    [HideInInspector] public List<CardInfoScript> PlayerHandCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> PlayerFieldCards = new List<CardInfoScript>();

    [HideInInspector] public List<CardInfoScript> EnemyHandCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> EnemyFieldCards = new List<CardInfoScript>();

    [HideInInspector] public bool IsChoosing;
    [HideInInspector] public bool IsSingleCardPlaying;

    [HideInInspector] public UnityEvent<CardInfoScript> EnemyDropCardEvent;
    [HideInInspector] public UnityEvent<CardInfoScript> PlayerDropCardEvent;
    [HideInInspector] public UnityEvent<CardInfoScript> OrderCard;

    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _endGamePanelWin;
    [SerializeField] private GameObject _endGamePanelLose;
    [SerializeField] private GameObject _endGamePanelDraw;

    //ChangeGameCharacteristics
    public int MaxNumberCardInField = 10;
    public int TurnDuration = 30;

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
            GameObject cardDebug = Instantiate(CardPref,_enemyField,false);
            cardDebug.GetComponent<CardInfoScript>().ShowCardInfo(CardManagerList.AllCards[0]);
            cardDebug.GetComponent<ChoseCard>().enabled = false;
            cardDebug.transform.SetParent(_enemyField);
            EnemyFieldCards.Add(cardDebug.GetComponent<CardInfoScript>());
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

        _playerField.GetComponent<DropField>().DropCard.AddListener(PlayerDropCartStartCoroutine);

        _imageTurnTime[0] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime").GetComponent<UnityEngine.UI.Image>();
        _imageTurnTime[1] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime1").GetComponent<UnityEngine.UI.Image>();
        _line = GameObject.Find("UI/MainCanvas/Line").GetComponent<LineRenderer>();

        EffectsManager = FindObjectOfType<EffectsManager>();
        _mainCamera = Camera.main;
    }


    private void Start()
    {
        _turn = 0;
        _playerPoints = 0;
        _enemyPoints = 0;

        _currentGame = new Game();

        DebugGame();
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

                if(_turnTime == 0 && !IsSingleCardPlaying && PlayerHandCards.Count !=0)
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
        if((PlayerHandCards.Count == 0) && (EnemyHandCards.Count == 0))
        {
            EndGame();
        }

        EndTurnActions();

        StopAllCoroutines();

        _turn++;
        IsSingleCardPlaying = false;
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

        enemyHandCards[enemyPlayedCard].GetComponent<CardMove>().EnemyMoveToField(_enemyField.transform);
        yield return new WaitForSeconds(0.6f);

        enemyHandCards[enemyPlayedCard].ShowCardInfo(enemyHandCards[enemyPlayedCard].SelfCard);
        enemyHandCards[enemyPlayedCard].transform.SetParent(_enemyField);

        EnemyDropCard(enemyHandCards[enemyPlayedCard]);

        ChangeTurn();
    }

    private void EnemyDropCard(CardInfoScript card)
    {
        CardInfoScript botChoosedCard;

        EnemyHandCards.Remove(card);
        EnemyFieldCards.Add(card);
        ChangeEnemyPoints();

        if ((card.SelfCard.Boost != 0) && (EnemyFieldCards.Count != 1))
        {
            botChoosedCard = ChooseOurCard(false);
            ChangePoints(botChoosedCard, card, true);

            if (card.SelfCard.RangeBoost > 0)
            {
                botChoosedCard.CheckSiblingIndex();

               if (botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeBoost) != null)
                {
                    for (int i = 0; i < botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeBoost).Count; i++)
                    {
                        ChangePoints(botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeBoost)[i], card, true, false, false, i + 1);
                    }
                }

                if (botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeBoost) != null)
                {
                    for (int i = 0; i < botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeBoost).Count; i++)
                    {
                        ChangePoints(botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeBoost)[i], card, true, false, false, i + 1);
                    }
                }
            }
        }

        if ((card.SelfCard.Damage != 0) && (PlayerFieldCards.Count != 0))
        {
            botChoosedCard = ChooseEnemyCard(false);
            ChangePoints(botChoosedCard, card, true);

            if (card.SelfCard.RangeDamage > 0)
            {
                botChoosedCard.CheckSiblingIndex();

                if (botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeDamage) != null)
                {
                    for (int i = 0; i < botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeDamage).Count; i++)
                    {
                        ChangePoints(botChoosedCard.ReturnRightNearCard(card.SelfCard.RangeDamage)[i], card, true, false, false, i + 1);
                    }
                }

                if (botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeDamage) != null)
                {
                    for (int i = 0; i < botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeDamage).Count; i++)
                    {
                        ChangePoints(botChoosedCard.ReturnLeftNearCard(card.SelfCard.RangeDamage)[i], card, true, false, false, i + 1);
                    }
                }
            }
        }

        if (((!card.SelfCard.AddictionWithSelfField && !card.SelfCard.AddictionWithEnemyField) ||
           ((card.SelfCard.AddictionWithSelfField && (EnemyFieldCards.Count != 1)) ||
           (card.SelfCard.AddictionWithEnemyField && (PlayerFieldCards.Count != 0)))))
        {
            ChangePoints(card, card, false, true);
            OrderCard.Invoke(card);
        }

        if (card.SelfCard.Summon)
        {
            OrderCard.Invoke(card);
            SpawnCard(card, false);
        }

        EnemyDropCardEvent.Invoke(card);
    }

    private void PlayerDropCartStartCoroutine(CardInfoScript card)
    {
        StartCoroutine(PlayerDropCard(card));
    }

    private IEnumerator PlayerDropCard(CardInfoScript card)
    {
        IsSingleCardPlaying = true;

        card.GetComponent<CardMove>().MoveTopHierarchy();
        card.GetComponent<CardMove>().PlayerMoveToField(_playerField, _playerHand.GetComponent<DropField>().EmptyHandCard);

        yield return new WaitForSeconds(0.6f);

        card.IsAnimationCard = false;
        card.GetComponent<CardMove>().MoveBackHierarchy();

        PlayerHandCards.Remove(card);
        PlayerFieldCards.Add(card);
        ChangePlayerPoints();

        PlayerDropCardEvent.Invoke(card);

        if ((card.SelfCard.Boost != 0) && (PlayerFieldCards.Count != 1))
        {
            foreach (CardInfoScript cardd in PlayerFieldCards)
            {
                cardd.transform.GetComponent<ChoseCard>().enabled = true;
                cardd.IsOrderCard = true;
            }

            card.transform.GetComponent<ChoseCard>().enabled = false;

            _line.startColor = Color.white;
            _line.endColor = Color.green;

            StartCoroutine(ChoseCardCoroutine(card, card.SelfCard.Boost != 0, card.SelfCard.Damage != 0));
        }

        if ((card.SelfCard.Damage != 0) && (EnemyFieldCards.Count != 0))
        {
            foreach (CardInfoScript cardd in EnemyFieldCards)
            {
                cardd.transform.GetComponent<ChoseCard>().enabled = true;
                cardd.IsOrderCard = true;
            }

            _line.startColor = Color.white;
            _line.endColor = Color.red;

            StartCoroutine(ChoseCardCoroutine(card, card.SelfCard.Boost != 0, card.SelfCard.Damage != 0));
        }

        if (((card.SelfCard.SelfBoost!=0)||(card.SelfCard.SelfDamage != 0)) && (!card.SelfCard.AddictionWithSelfField && !card.SelfCard.AddictionWithEnemyField))
        {
            ChangePoints(card, card, false, true);
            OrderCard.Invoke(card);
        }

        if (card.SelfCard.Summon)
        {
            OrderCard.Invoke(card);
            SpawnCard(card,true);
        }
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
            card.Point.colorGradient = new VertexGradient(Color.white, Color.white, Color.black, Color.black);
        }

        else if (card.SelfCard.Points < card.SelfCard.MaxPoints)
        {
            card.Point.colorGradient = new VertexGradient(Color.red, Color.red, Color.black, Color.black);
        }

        else
        {
            card.Point.colorGradient = new VertexGradient(Color.green, Color.green, Color.black, Color.black);
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
            return EnemyFieldCards[Random.Range(0, EnemyFieldCards.Count - 1)];
        }
    }

    private IEnumerator ChoseCardCoroutine(CardInfoScript card, bool isBoost, bool isDamage)
    {
        _choosenCard = null;
        StartChoseCard = card;
        card.ImageEdge1.color = Color.green;
        //card.transform.position += new UnityEngine.Vector3(0, 0, 0);
        EndTurnButton.interactable = false;

        yield return StartCoroutine(WaitForChoseCard(card));

        if (isBoost) //ñhoose self card
        {
            if ((card.SelfCard.AddictionWithSelfField && (PlayerFieldCards.Count != 1)) ||
            (card.SelfCard.AddictionWithEnemyField && (EnemyFieldCards.Count != 0)))
            {
                ChangePoints(card, card, false, true);
            }

            ChangePoints(ChooseOurCard(true), card, true);

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
                        ChangePoints(ChooseOurCard(true).ReturnRightNearCard(card.SelfCard.RangeBoost)[i], card, true, false, false, i + 1);
                    }
                }

                if (ChooseOurCard(true).ReturnLeftNearCard(card.SelfCard.RangeBoost) != null)
                {
                    for (int i = 0; i < ChooseOurCard(true).ReturnLeftNearCard(card.SelfCard.RangeBoost).Count; i++)
                    {
                        ChangePoints(ChooseOurCard(true).ReturnLeftNearCard(card.SelfCard.RangeBoost)[i], card, true, false, false, i + 1);
                    }
                }
            }
        }

        if (isDamage) //ñhoose enemy card
        {
            if ((card.SelfCard.AddictionWithSelfField && (PlayerFieldCards.Count != 1)) ||
            (card.SelfCard.AddictionWithEnemyField && (EnemyFieldCards.Count != 0)))
            {
                ChangePoints(card, card, false, true);
            }

            ChangePoints(ChooseEnemyCard(true), card, true);

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
                        ChangePoints(ChooseEnemyCard(true).ReturnRightNearCard(card.SelfCard.RangeDamage)[i], card, true, false, false, i + 1);
                    }
                }

                if (ChooseEnemyCard(true).ReturnLeftNearCard(card.SelfCard.RangeDamage) != null)
                {
                    for (int i = 0; i < ChooseEnemyCard(true).ReturnLeftNearCard(card.SelfCard.RangeDamage).Count; i++)
                    {
                        ChangePoints(ChooseEnemyCard(true).ReturnLeftNearCard(card.SelfCard.RangeDamage)[i], card, true, false, false, i + 1);
                    }
                }
            }
        }

        _line.SetPosition(0, UnityEngine.Vector3.zero);
        _line.SetPosition(1, UnityEngine.Vector3.zero);

        card.ImageEdge.color = card.SelfCard.ColorTheme;
        EndTurnButton.interactable = true;

        OrderCard.Invoke(card);
    }

    private IEnumerator WaitForChoseCard(CardInfoScript card)
    {
        _choosenCard = null;

        while (_choosenCard == null)
        {

            _line.SetPosition(0, card.transform.position);
            _line.SetPosition(1, _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.farClipPlane)));

            yield return null;
        }
    }

    public void ChoseCard(CardInfoScript card)
    {
        _choosenCard = card;
    }

    private void EndTurnActions()
    {
        if (IsPlayerTurn)
        {
            foreach (CardInfoScript card in PlayerFieldCards)
            {
                if (card.SelfCard.EndTurnAction == true)
                {
                    if ((card.SelfCard.EndTurnDamage != 0) && (EnemyFieldCards.Count > 0))
                    {
                        ChangePoints(EnemyFieldCards[Random.Range(0, EnemyFieldCards.Count)], card, false, false, true);
                    }

                    if ((card.SelfCard.EndTurnBoost != 0) && (PlayerFieldCards.Count > 0))
                    {
                        ChangePoints(PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)], card, false, false, true);
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
                        ChangePoints(PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)], card, false, false, true);
                    }

                    if ((card.SelfCard.EndTurnBoost != 0) && (EnemyFieldCards.Count > 0))
                    {
                        ChangePoints(EnemyFieldCards[Random.Range(0, EnemyFieldCards.Count)], card, false, false, true);
                    }
                }
            }
        }
    }

    private void SpawnCard(CardInfoScript card, bool player)
    {
        GameObject summonCard;

        if (card.SelfCard.SummonCardCount == -1)
        {
            if (!((player && PlayerFieldCards.Count < MaxNumberCardInField) || (!player && EnemyFieldCards.Count < MaxNumberCardInField))) { return; }
            summonCard = Instantiate(CardPref, card.transform.parent, false);
            card.CheckSiblingIndex();
            summonCard.transform.SetSiblingIndex(card.SiblingIndex + 1);
            if (player) PlayerFieldCards.Add(summonCard.GetComponent<CardInfoScript>());
            else EnemyFieldCards.Add(summonCard.GetComponent<CardInfoScript>());
            summonCard.GetComponent<CardInfoScript>().ShowCardInfo(card.SelfCard);
            summonCard.GetComponent<ChoseCard>().enabled = false;

            if (!((player && PlayerFieldCards.Count < MaxNumberCardInField) || (!player && EnemyFieldCards.Count < MaxNumberCardInField))) { return; }
            summonCard = Instantiate(CardPref, card.transform.parent, false);
            card.CheckSiblingIndex();
            summonCard.transform.SetSiblingIndex(card.SiblingIndex);
            if (player) PlayerFieldCards.Add(summonCard.GetComponent<CardInfoScript>());
            else EnemyFieldCards.Add(summonCard.GetComponent<CardInfoScript>());
            summonCard.GetComponent<CardInfoScript>().ShowCardInfo(card.SelfCard);
            summonCard.GetComponent<ChoseCard>().enabled = false;
        }

        else
        {
            if (!((player && PlayerFieldCards.Count < MaxNumberCardInField) || (!player && EnemyFieldCards.Count < MaxNumberCardInField))) { return; }
            summonCard = Instantiate(CardPref, card.transform.parent, false);
            card.CheckSiblingIndex();
            summonCard.transform.SetSiblingIndex(card.SiblingIndex + 1);
            if (player) PlayerFieldCards.Add(summonCard.GetComponent<CardInfoScript>());
            else EnemyFieldCards.Add(summonCard.GetComponent<CardInfoScript>());
            summonCard.GetComponent<CardInfoScript>().ShowCardInfo(CardManagerList.SummonCards[card.SelfCard.SummonCardCount]);
            Debug.Log(summonCard.GetComponent<CardInfoScript>().SelfCard.Points);
            summonCard.GetComponent<ChoseCard>().enabled = false;

            if (!((player && PlayerFieldCards.Count < MaxNumberCardInField) || (!player && EnemyFieldCards.Count < MaxNumberCardInField))) { return; }
            summonCard = Instantiate(CardPref, card.transform.parent, false);
            card.CheckSiblingIndex();
            summonCard.transform.SetSiblingIndex(card.SiblingIndex);
            if (player) PlayerFieldCards.Add(summonCard.GetComponent<CardInfoScript>());
            else EnemyFieldCards.Add(summonCard.GetComponent<CardInfoScript>());
            summonCard.GetComponent<CardInfoScript>().ShowCardInfo(CardManagerList.SummonCards[card.SelfCard.SummonCardCount]);
            summonCard.GetComponent<ChoseCard>().enabled = false;
        }

        ChangeEnemyPoints();
        ChangePlayerPoints();
    }

    private void ChangePoints(CardInfoScript targetCard, CardInfoScript startCard, bool deploymentAction = false, bool selfAction = false, bool endTurnAction = false, int distanceNearCard = 0)
    {
        if (deploymentAction)
        {
            if (startCard.SelfCard.Boost != 0)
            {
                targetCard.ChangePoints(ref targetCard.SelfCard, startCard.SelfCard.Boost + distanceNearCard * startCard.SelfCard.ChangeBoost, startCard.SelfCard);

                if (startCard.SelfCard.Boost > 0) EffectsManager.Boost(startCard.transform, targetCard.transform);
                else EffectsManager.Damage(startCard.transform, targetCard.transform);
            }
            if (startCard.SelfCard.Damage != 0)
            {
                targetCard.ChangePoints(ref targetCard.SelfCard, -startCard.SelfCard.Damage - distanceNearCard * startCard.SelfCard.ChangeDamage, startCard.SelfCard);

                if (startCard.SelfCard.Damage > 0) EffectsManager.Damage(startCard.transform, targetCard.transform);
                else EffectsManager.Boost(startCard.transform, targetCard.transform);
            }
        }

        if (selfAction)
        {
            if (startCard.SelfCard.SelfBoost != 0)
            {
                targetCard.ChangePoints(ref startCard.SelfCard, startCard.SelfCard.SelfBoost, startCard.SelfCard);
                EffectsManager.SelfBoost(startCard.transform);
            }
            if (startCard.SelfCard.SelfDamage != 0)
            {
                targetCard.ChangePoints(ref startCard.SelfCard, -startCard.SelfCard.SelfDamage, startCard.SelfCard);
                EffectsManager.SelfDamage(startCard.transform);
            }
        }

        if (endTurnAction)
        {
            if (startCard.SelfCard.EndTurnBoost != 0)
            {
                targetCard.ChangePoints(ref targetCard.SelfCard, startCard.SelfCard.EndTurnBoost, startCard.SelfCard);

                if (startCard.SelfCard.EndTurnBoost > 0) EffectsManager.EndTurnBoost(startCard.transform, targetCard.transform);
                else EffectsManager.EndTurnDamage(startCard.transform, targetCard.transform);
            }
            if (startCard.SelfCard.EndTurnDamage != 0)
            {
                targetCard.ChangePoints(ref targetCard.SelfCard, -startCard.SelfCard.EndTurnDamage, startCard.SelfCard);

                if (startCard.SelfCard.EndTurnDamage > 0) EffectsManager.EndTurnDamage(startCard.transform, targetCard.transform);
                else EffectsManager.EndTurnBoost(startCard.transform, targetCard.transform);
            }
        }

        CheckColorPointsCard(targetCard);
        CheckColorPointsCard(startCard);

        IsDestroyCard(targetCard);
        ChangeEnemyPoints();
        ChangePlayerPoints();
    }

    private void IsDestroyCard(CardInfoScript card)
    {
        if (card.SelfCard.Points <= 0)
        {
            card.SelfCard.Points = 0;
            if (PlayerFieldCards.Contains(card))
                PlayerFieldCards.Remove(card);

            else if (EnemyFieldCards.Contains(card))
                EnemyFieldCards.Remove(card);

            Destroy(card.DescriptionObject);
            Destroy(card.gameObject,0.5f);
        }
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

        _currentGame = new Game();

        GiveHandCards(_currentGame.EnemyDeck, _enemyHand);
        GiveHandCards(_currentGame.PlayerDeck, _playerHand);

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

        Destroy(card.transform.gameObject,0.5f);
    }
}
