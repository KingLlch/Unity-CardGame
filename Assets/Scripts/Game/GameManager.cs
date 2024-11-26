using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game
{
    public List<Card> EnemyDeck;
    public List<Card> PlayerDeck;

    public Game()
    {
        EnemyDeck = GiveDeckCard();

        if (Object.FindObjectOfType<DeckManager>() != null && DeckManager.Instance.Deck != null)
        {
            DeckManager.Instance.Deck = ShuffleDeck();
            PlayerDeck = new List<Card>(DeckManager.Instance.Deck);
        }
        else
            PlayerDeck = GiveDeckCard();
    }

    private List<Card> ShuffleDeck()
    {
        List<Card> shuffleDeck = new List<Card>(DeckManager.Instance.Deck);

        for (int i = shuffleDeck.Count - 1; i > 0; i--)
        {
            int random = Random.Range(0, i + 1);

            Card temp = shuffleDeck[i];
            shuffleDeck[i] = shuffleDeck[random];
            shuffleDeck[random] = temp;
        }

        return shuffleDeck;
    }

    private List<Card> GiveDeckCard()
    {
        List<Card> DeckList = new List<Card>();
        for (int i = 0; i < GameManager.Instance.ValueDeckCards; i++)
        {
            DeckList.Add(CardManagerList.AllCards[Random.Range(0, CardManagerList.AllCards.Count)]);
            //DeckList.Add(CardManagerList.AllCards[45]);
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
    private GameObject _enemyHandPass;
    private GameObject _playerHandPass;

    private int _turn;
    private int _turnTime;
    private int _playerPoints;
    private int _enemyPoints;

    public bool IsDrag;
    public bool IsChooseCard;
    public GameObject CardPref;

    private bool isPlayerPassed;
    private bool isEnemyPassed;

    public CardInfoScript StartChoseCard;
    private CardInfoScript _choosenCard;

    [HideInInspector] public List<CardInfoScript> PlayerHandCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> PlayerFieldCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> PlayerFieldDestroyedInEndTurnCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> PlayerFieldInvulnerabilityCards = new List<CardInfoScript>();

    [HideInInspector] public List<CardInfoScript> EnemyHandCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> EnemyFieldCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> EnemyFieldDestroyedInEndTurnCards = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> EnemyFieldInvulnerabilityCards = new List<CardInfoScript>();

    [HideInInspector] public List<CardInfoScript> CardsCanChooseOnWickEnd = new List<CardInfoScript>();

    [HideInInspector] public List<Coroutine> AllCoroutine = new List<Coroutine>();

    [HideInInspector] public bool IsChoosing;
    [HideInInspector] public bool IsHandCardPlaying;

    private Camera _mainCamera;

    public GameObject[] HowToPlayList;
    public GameObject HowToPlayFon;

    //ChangeGameCharacteristics
    public int MaxNumberCardInField = 10;
    public int TurnDuration = 30;
    public int ValueDeckCards = 20;
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
            cardDebug.GetComponent<CardInfoScript>().ShowCardInfo(CardManagerList.DebugCards[0]);
            cardDebug.AddComponent<ChoseCard>();
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

        _enemyHandPass = GameObject.Find("UI/MainCanvas/EnemyHand/PassImage");
        _playerHandPass = GameObject.Find("UI/MainCanvas/PlayerHand/PassImage");

        _playerField.GetComponent<DropField>().DropCard.AddListener(PlayerDropCardStartCoroutine);
        _enemyField.GetComponent<DropField>().DropCard.AddListener(PlayerDropCardStartCoroutine);

        _mainCamera = Camera.main;
    }


    private void Start()
    {
        _turn = 0;
        _playerPoints = 0;
        _enemyPoints = 0;

        CurrentGame = new Game();

        DebugGame();
        Deck.Instance.CreateDeck(CurrentGame.PlayerDeck);

        GiveHandCards(CurrentGame.EnemyDeck, _enemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, _playerHand);

        if (Object.FindObjectOfType<HowToPlay>() != null && HowToPlay.Instance.IsHowToPlay)
        {
            HowToPlay.Instance.HowToPlayGame(HowToPlayList, HowToPlayFon);
        }

        else
            AllCoroutine.Add(StartCoroutine(TurnFunk()));
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

        cardHand.AddComponent<ChoseCard>();
        cardHand.GetComponent<ChoseCard>().enabled = false;

        if (hand == _enemyHand)
        {
            cardHand.GetComponent<CardInfoScript>().HideCardInfo(card);
            EnemyHandCards.Add(cardHand.GetComponent<CardInfoScript>());
            UIManager.Instance.CheckColorPointsCard(cardHand.GetComponent<CardInfoScript>());
        }

        else
        {
            cardHand.GetComponent<CardInfoScript>().ShowCardInfo(card);
            PlayerHandCards.Add(cardHand.GetComponent<CardInfoScript>());
            UIManager.Instance.CheckColorPointsCard(cardHand.GetComponent<CardInfoScript>());

            Deck.Instance.DeleteFirstCardFromDeck();
        }

        deck.RemoveAt(0);

        UIManager.Instance.ChangeDeckCount(CurrentGame);
    }

    public void StartTurnCoroutine()
    {
        AllCoroutine.Add(StartCoroutine(TurnFunk()));
    }

    private IEnumerator TurnFunk()
    {
        _turnTime = TurnDuration;

        UIManager.Instance.ChangeWick(_turnTime);

        if (IsPlayerTurn)
        {
            if (!isPlayerPassed)
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
                    _choosenCard = CardsCanChooseOnWickEnd[Random.Range(0, CardsCanChooseOnWickEnd.Count)];
                    yield return null;
                }

            }

            StartCoroutine(ChangeTurn());
        }

        else
        {
            AllCoroutine.Add(StartCoroutine(EnemyTurn(EnemyHandCards)));
        }
    }

    public void StartChangeTurn()
    {
        StartCoroutine(ChangeTurn());
    }

    private IEnumerator ChangeTurn()
    {
        if (PlayerHandCards.Count == 0)
        {
            _playerHandPass.SetActive(true);
            isPlayerPassed = true;
        }

        if (EnemyHandCards.Count == 0)
        {
            _enemyHandPass.SetActive(true);
            isEnemyPassed = true;
        }

        if (isPlayerPassed && isEnemyPassed)
        {
            EndGame();
            UIManager.Instance.EndGame(_playerPoints, _enemyPoints);
        }

        if (IsPlayerTurn && !IsHandCardPlaying && PlayerHandCards.Count != 0)
            ThrowCard(PlayerHandCards[Random.Range(0, PlayerHandCards.Count)], true);

        yield return StartCoroutine(CardMechanics.Instance.EndTurnActions());

        ClearDestroyedInEndTurnCards();

        foreach (Coroutine coroutine in AllCoroutine)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        AllCoroutine.Clear();

        ChangeEnemyPoints();
        ChangePlayerPoints();

        _turn++;
        IsHandCardPlaying = false;
        UIManager.Instance.ChangeEndTurnButtonInteractable(IsPlayerTurn);
        AllCoroutine.Add(StartCoroutine(TurnFunk()));

        yield break;
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

            StartCoroutine(ChangeTurn());
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

        StartCoroutine(ChangeTurn());
    }

    private IEnumerator EnemyDropCard(CardInfoScript card)
    {
        yield return new WaitForSeconds(0.1f);

        CardInfoScript botChoosedCard;

        EnemyHandCards.Remove(card);

        if (!card.SelfCard.StatusEffects.IsInvisibility)
        {
            EnemyFieldCards.Add(card);
            ChangeEnemyPoints();

            if (card.SelfCard.StatusEffects.IsInvulnerability)
                EnemyFieldInvulnerabilityCards.Add(card);
        }

        else if (card.SelfCard.StatusEffects.IsInvisibility)
        {
            PlayerFieldCards.Add(card);
            ChangePlayerPoints();

            if (card.SelfCard.StatusEffects.IsInvulnerability)
                PlayerFieldInvulnerabilityCards.Add(card);
        }

        CardMechanics.Instance.CheckStatusEffects(card);

        SoundManager.Instance.EnemyDeploymentSound(card);

        if (card.SelfCard.BoostOrDamage.NearBoost == -1)
        {
            for (int i = EnemyFieldCards.Count - 1; i >= 0; i--)
            {
                CardMechanics.Instance.Deployment(EnemyFieldCards[i], card);

                if (card.SelfCard.StatusEffects.EnduranceOrBleedingOther != 0 && !card.SelfCard.StatusEffects.IsEnemyTargetEnduranceOrBleeding)
                {
                    CardMechanics.Instance.BleedingOrEndurance(card, EnemyFieldCards[i]);
                    UIManager.Instance.CheckBleeding(EnemyFieldCards[i]);
                }

                if (card.SelfCard.EndTurnActions.ArmorOther > 0)
                {
                    EnemyFieldCards[i].SelfCard.BaseCard.ArmorPoints += card.SelfCard.EndTurnActions.ArmorOther;
                    UIManager.Instance.CheckArmor(EnemyFieldCards[i]);
                }
            }

            SoundManager.Instance.EnemyStartEffectSound(card);
        }


        else if ((card.SelfCard.BoostOrDamage.Boost != 0) && (EnemyFieldCards.Count != 1) && ((EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count) > 0))
        {
            botChoosedCard = ChooseCard(false);
            CardMechanics.Instance.Deployment(botChoosedCard, card);

            if (card.SelfCard.StatusEffects.IsShieldOther)
            {
                botChoosedCard.SelfCard.StatusEffects.IsSelfShielded = true;
                CardMechanics.Instance.CheckStatusEffects(botChoosedCard);
            }

            if (card.SelfCard.BoostOrDamage.NearBoost > 0)
            {
                botChoosedCard.CheckSiblingIndex();

                if (CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearBoost, true) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearBoost, true).Count; i++)
                    {
                        CardMechanics.Instance.Deployment(CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearBoost, true)[i], card, i + 1);
                    }
                }

                if (CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearBoost, false) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearBoost, false).Count; i++)
                    {
                        CardMechanics.Instance.Deployment(CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearBoost, false)[i], card, i + 1);
                    }
                }
            }

            SoundManager.Instance.EnemyStartEffectSound(card);
        }

        if (card.SelfCard.BoostOrDamage.NearDamage == -1)
        {
            for (int i = PlayerFieldCards.Count - 1; i >= 0; i--)
            {
                CardMechanics.Instance.Deployment(PlayerFieldCards[i], card);

                if (card.SelfCard.StatusEffects.IsStunOther)
                {
                    PlayerFieldCards[i].SelfCard.StatusEffects.IsSelfStunned = true;
                    CardMechanics.Instance.CheckStatusEffects(PlayerFieldCards[i]);
                }

                if (card.SelfCard.StatusEffects.EnduranceOrBleedingOther != 0 && card.SelfCard.StatusEffects.IsEnemyTargetEnduranceOrBleeding)
                {
                    CardMechanics.Instance.BleedingOrEndurance(card, PlayerFieldCards[i]);
                }
            }

            SoundManager.Instance.EnemyStartEffectSound(card);
        }

        else if ((card.SelfCard.BoostOrDamage.Damage != 0) && (PlayerFieldCards.Count != 0) && ((PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count) > 0))
        {
            botChoosedCard = ChooseCard(false, false);
            CardMechanics.Instance.Deployment(botChoosedCard, card);

            if (card.SelfCard.StatusEffects.IsStunOther)
            {
                botChoosedCard.SelfCard.StatusEffects.IsSelfStunned = true;
                CardMechanics.Instance.CheckStatusEffects(botChoosedCard);
            }

            if (card.SelfCard.BoostOrDamage.NearDamage > 0)
            {
                botChoosedCard.CheckSiblingIndex();

                if (CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearDamage, true) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearDamage, true).Count; i++)
                    {
                        CardMechanics.Instance.Deployment(CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearDamage, true)[i], card, i + 1);

                        if (card.SelfCard.StatusEffects.IsStunOther)
                        {
                            CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearDamage, true)[i].SelfCard.StatusEffects.IsSelfStunned = true;
                            CardMechanics.Instance.CheckStatusEffects(CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearDamage, true)[i]);
                        }
                    }
                }

                if (CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearDamage, false) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearDamage, false).Count; i++)
                    {
                        CardMechanics.Instance.Deployment(CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearDamage, false)[i], card, i + 1);

                        if (card.SelfCard.StatusEffects.IsStunOther)
                        {
                            CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearDamage, false)[i].SelfCard.StatusEffects.IsSelfStunned = true;
                            CardMechanics.Instance.CheckStatusEffects(CardMechanics.Instance.ReturnNearCard(botChoosedCard, card.SelfCard.BoostOrDamage.NearDamage, false)[i]);
                        }
                    }
                }
            }

            if (!card.SelfCard.BoostOrDamage.AddictionWithEnemyField)
                SoundManager.Instance.EnemyStartEffectSound(card);
        }

        if ((card.SelfCard.BoostOrDamage.SelfBoost != 0 || card.SelfCard.BoostOrDamage.SelfDamage != 0) && ((!card.SelfCard.BoostOrDamage.AddictionWithAlliedField && !card.SelfCard.BoostOrDamage.AddictionWithEnemyField) ||
           (card.SelfCard.BoostOrDamage.AddictionWithAlliedField && (EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count != 1) ||
           (card.SelfCard.BoostOrDamage.AddictionWithEnemyField && ((PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count) > 0)))))
        {
            CardMechanics.Instance.Self(card, card);
            SoundManager.Instance.EnemyStartEffectSound(card);
        }

        if (card.SelfCard.Spawns.SpawnCardCount != 0 && (!card.SelfCard.BoostOrDamage.AddictionWithEnemyField) || (card.SelfCard.BoostOrDamage.AddictionWithEnemyField && PlayerFieldCards.Count > 0))
        {
            SoundManager.Instance.EnemyStartEffectSound(card);
            CardMechanics.Instance.SpawnCard(card, false);
            ChangeEnemyPoints();
        }

        if (card.SelfCard.DrawCard.DrawCardCount != 0)
        {
            for (int i = 0; i < card.SelfCard.DrawCard.DrawCardCount; i++)
            {
                EffectsManager.Instance.DrawCardEffect(_enemyHand, false);
                yield return new WaitForSeconds(0.3f);
                EffectsManager.Instance.HideDrawCardEffect();
                GiveCardtoHand(CurrentGame.EnemyDeck, _enemyHand);
            }
        }

        if ((card.SelfCard.UniqueMechanics.DestroyCardPoints != 0) && (PlayerFieldCards.Count != 0) && ((PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count) > 0))
        {
            if (card.SelfCard.UniqueMechanics.DestroyCardPoints == -1)
            {
                botChoosedCard = ChooseCard(false, false);
                CardMechanics.Instance.DestroyCard(botChoosedCard, card);
            }

            else
            {
                List<CardInfoScript> possibleCards = new List<CardInfoScript>();

                foreach (CardInfoScript playerFieldCard in PlayerFieldCards)
                {
                    if (playerFieldCard.SelfCard.BaseCard.Points <= card.SelfCard.UniqueMechanics.DestroyCardPoints && !playerFieldCard.SelfCard.StatusEffects.IsInvulnerability)
                    {
                        possibleCards.Add(playerFieldCard);
                    }
                }

                if (possibleCards.Count > 0)
                {
                    botChoosedCard = possibleCards[Random.Range(0, possibleCards.Count - 1)];
                    CardMechanics.Instance.DestroyCard(botChoosedCard, card);
                }
            }
        }

        if (card.SelfCard.UniqueMechanics.SwapPoints)
        {
            if ((PlayerFieldCards.Count != 0) && ((PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count) > 0))
            {
                botChoosedCard = ChooseCard(false, false);
                CardMechanics.Instance.SwapPoints(card, botChoosedCard);
            }

            else if ((EnemyFieldCards.Count != 1) && ((EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count) > 0))
            {
                botChoosedCard = ChooseCard(false, true);
                CardMechanics.Instance.SwapPoints(card, botChoosedCard);
            }
        }

        if (card.SelfCard.StatusEffects.EnduranceOrBleedingOther != 0)
        {
            if (card.SelfCard.StatusEffects.IsEnemyTargetEnduranceOrBleeding && card.SelfCard.BoostOrDamage.NearBoost != -1 && ((PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count) > 0))
            {
                botChoosedCard = ChooseCard(false, false);
                CardMechanics.Instance.BleedingOrEndurance(card, botChoosedCard);
                UIManager.Instance.CheckBleeding(botChoosedCard);
            }

            else if (card.SelfCard.BoostOrDamage.NearDamage != -1 && (EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count) > 1)
            {
                botChoosedCard = ChooseCard(false, true);
                CardMechanics.Instance.BleedingOrEndurance(card, botChoosedCard);
                UIManager.Instance.CheckBleeding(botChoosedCard);
            }

        }

        if (card.SelfCard.UniqueMechanics.TransformationNumber != -1)
        {
            CardMechanics.Instance.Transformation(card);
        }

        ChangeEnemyPoints();
        ChangePlayerPoints();
    }

    private void PlayerDropCardStartCoroutine(CardInfoScript card)
    {
        AllCoroutine.Add(StartCoroutine(PlayerDropCard(card)));
    }

    private IEnumerator PlayerDropCard(CardInfoScript card)
    {
        IsHandCardPlaying = true;

        CardMove cardMove = card.GetComponent<CardMove>();
        cardMove.MoveTopHierarchy();

        if (!card.SelfCard.StatusEffects.IsInvisibility)
            cardMove.PlayerMoveToField(_playerField.GetComponent<DropField>(), _playerHand.GetComponent<DropField>().EmptyHandCard);

        else if (card.SelfCard.StatusEffects.IsInvisibility)
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

        else if (card.SelfCard.StatusEffects.IsInvisibility)
        {
            EnemyFieldCards.Add(card);
            ChangeEnemyPoints();

            if (card.SelfCard.StatusEffects.IsInvulnerability)
                EnemyFieldInvulnerabilityCards.Add(card);
        }

        CardMechanics.Instance.CheckStatusEffects(card);

        SoundManager.Instance.PlayerDeploymentSound(card);

        if (card.SelfCard.BoostOrDamage.NearBoost == -1)
        {
            for (int i = PlayerFieldCards.Count - 1; i >= 0; i--)
            {
                CardMechanics.Instance.Deployment(PlayerFieldCards[i], card);

                if (card.SelfCard.StatusEffects.EnduranceOrBleedingOther != 0 && !card.SelfCard.StatusEffects.IsEnemyTargetEnduranceOrBleeding)
                {
                    CardMechanics.Instance.BleedingOrEndurance(card, PlayerFieldCards[i]);
                    UIManager.Instance.CheckBleeding(PlayerFieldCards[i]);
                }

                if (card.SelfCard.EndTurnActions.ArmorOther > 0)
                {
                    PlayerFieldCards[i].SelfCard.BaseCard.ArmorPoints += card.SelfCard.EndTurnActions.ArmorOther;
                    UIManager.Instance.CheckArmor(PlayerFieldCards[i]);
                }
            }

            SoundManager.Instance.PlayerStartEffectSound(card);
        }

        else if ((card.SelfCard.BoostOrDamage.Boost != 0) && PlayerFieldCards.Count != 1 && ((PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count) > 1))
        {
            if (card.SelfCard.BoostOrDamage.NearBoost != -1)
            {
                PrepareToChoseCard(card, false);

                AllCoroutine.Add(StartCoroutine(ChoseCardCoroutine(card, isBoost: true)));
            }
        }

        if (card.SelfCard.BoostOrDamage.NearDamage == -1)
        {
            for (int i = EnemyFieldCards.Count - 1; i >= 0; i--)
            {
                CardMechanics.Instance.Deployment(EnemyFieldCards[i], card);

                if (card.SelfCard.StatusEffects.IsStunOther)
                {
                    EnemyFieldCards[i].SelfCard.StatusEffects.IsSelfStunned = true;
                    CardMechanics.Instance.CheckStatusEffects(EnemyFieldCards[i]);
                }

                if (card.SelfCard.StatusEffects.EnduranceOrBleedingOther != 0 && card.SelfCard.StatusEffects.IsEnemyTargetEnduranceOrBleeding)
                {
                    CardMechanics.Instance.BleedingOrEndurance(card, EnemyFieldCards[i]);
                }
            }

            SoundManager.Instance.PlayerStartEffectSound(card);
        }

        else if ((card.SelfCard.BoostOrDamage.Damage != 0) && (EnemyFieldCards.Count != 0) && ((EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count) > 0))
        {
            if (card.SelfCard.BoostOrDamage.NearDamage != -1)
            {
                PrepareToChoseCard(card, true);

                AllCoroutine.Add(StartCoroutine(ChoseCardCoroutine(card, isDamage: true)));
            }
        }

        if (((card.SelfCard.BoostOrDamage.SelfBoost != 0) || (card.SelfCard.BoostOrDamage.SelfDamage != 0)) && (!card.SelfCard.BoostOrDamage.AddictionWithAlliedField && !card.SelfCard.BoostOrDamage.AddictionWithEnemyField))
        {
            CardMechanics.Instance.Self(card, card);

            ChangePlayerPoints();

            SoundManager.Instance.PlayerStartEffectSound(card);
        }

        if (card.SelfCard.Spawns.SpawnCardCount != 0 && !card.SelfCard.BoostOrDamage.AddictionWithEnemyField)
        {
            SoundManager.Instance.PlayerStartEffectSound(card);
            CardMechanics.Instance.SpawnCard(card, true);
            ChangePlayerPoints();
        }

        if (card.SelfCard.DrawCard.DrawCardCount != 0)
        {
            UIManager.Instance.ChangeEndTurnButtonInteractable(false);
            for (int i = 0; i < card.SelfCard.DrawCard.DrawCardCount; i++)
            {
                EffectsManager.Instance.DrawCardEffect(_playerHand, true);


                yield return new WaitForSeconds(0.3f);


                EffectsManager.Instance.HideDrawCardEffect();
                GiveCardtoHand(CurrentGame.PlayerDeck, _playerHand);
            }
            UIManager.Instance.ChangeEndTurnButtonInteractable(true);
        }

        if (card.SelfCard.UniqueMechanics.DestroyCardPoints != 0)
        {
            if (card.SelfCard.UniqueMechanics.DestroyCardPoints == -1)
            {
                PrepareToChoseCard(card, true);

                AllCoroutine.Add(StartCoroutine(ChoseCardCoroutine(card, isDestroy: true)));
            }

            else
            {
                List<CardInfoScript> possibleCards = new List<CardInfoScript>();

                foreach (CardInfoScript enemyFieldCard in EnemyFieldCards)
                {
                    if (enemyFieldCard.SelfCard.BaseCard.Points <= card.SelfCard.UniqueMechanics.DestroyCardPoints && !enemyFieldCard.SelfCard.StatusEffects.IsInvulnerability)
                    {
                        possibleCards.Add(enemyFieldCard);
                    }
                }

                if (possibleCards.Count > 0)
                {
                    foreach (CardInfoScript possibleChoseCard in possibleCards)
                    {
                        possibleChoseCard.GetComponent<ChoseCard>().enabled = true;
                    }

                    AllCoroutine.Add(StartCoroutine(ChoseCardCoroutine(card, isDestroy: true)));
                }
            }
        }

        if (card.SelfCard.UniqueMechanics.SwapPoints && (((EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count) > 0) || ((PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count) > 1)))
        {
            PrepareToChoseCard(card, true);
            PrepareToChoseCard(card, false);

            AllCoroutine.Add(StartCoroutine(ChoseCardCoroutine(card, isSwapPoints: true)));
        }

        if (card.SelfCard.StatusEffects.EnduranceOrBleedingOther != 0)
        {
            if (card.SelfCard.StatusEffects.IsEnemyTargetEnduranceOrBleeding && card.SelfCard.BoostOrDamage.NearDamage != -1 && ((EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count) > 0))
            {
                PrepareToChoseCard(card, true);
                AllCoroutine.Add(StartCoroutine(ChoseCardCoroutine(card, isEnduranceOrBleeding: true, isEnduranceOrBleedingEnemy: true)));
            }

            else if (card.SelfCard.BoostOrDamage.NearBoost != -1 && (PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count) > 1)
            {
                PrepareToChoseCard(card, false);
                AllCoroutine.Add(StartCoroutine(ChoseCardCoroutine(card, isEnduranceOrBleeding: true, isEnduranceOrBleedingEnemy: false)));
            }
        }

        if (card.SelfCard.UniqueMechanics.TransformationNumber != -1)
        {
            CardMechanics.Instance.Transformation(card);
        }

        ChangeEnemyPoints();
        ChangePlayerPoints();
    }

    private void PrepareToChoseCard(CardInfoScript playedCard, bool isEnemyField)
    {
        if (isEnemyField)
        {
            foreach (CardInfoScript enemyFieldCard in EnemyFieldCards)
            {
                if (!enemyFieldCard.SelfCard.StatusEffects.IsInvulnerability)
                {
                    enemyFieldCard.transform.GetComponent<ChoseCard>().enabled = true;
                    enemyFieldCard.IsOrderCard = true;
                    CardsCanChooseOnWickEnd.Add(enemyFieldCard);
                }

                enemyFieldCard.IsOrderCard = true;
            }

            CardsCanChooseOnWickEnd.Remove(playedCard);

            UIManager.Instance.ChangeLineColor(Color.white, Color.red);

        }

        else
        {
            foreach (CardInfoScript playerFieldCard in PlayerFieldCards)
            {
                if (!playerFieldCard.SelfCard.StatusEffects.IsInvulnerability)
                {
                    playerFieldCard.transform.GetComponent<ChoseCard>().enabled = true;
                    playerFieldCard.IsOrderCard = true;
                    CardsCanChooseOnWickEnd.Add(playerFieldCard);
                }

                playerFieldCard.IsOrderCard = true;
            }

            playedCard.transform.GetComponent<ChoseCard>().enabled = false;
            CardsCanChooseOnWickEnd.Remove(playedCard);

            UIManager.Instance.ChangeLineColor(Color.white, Color.green);
        }
    }

    private void RemovePrepareToChoseCard(bool isEnemyField)
    {
        if (isEnemyField)
        {
            foreach (CardInfoScript enemyFieldCard in EnemyFieldCards)
            {
                enemyFieldCard.transform.GetComponent<ChoseCard>().enabled = false;
                CardsCanChooseOnWickEnd.Remove(enemyFieldCard);
                enemyFieldCard.ImageEdge1.color = Color.white;
                enemyFieldCard.IsOrderCard = false;
            }
        }

        else
        {
            foreach (CardInfoScript playerFieldCard in PlayerFieldCards)
            {
                playerFieldCard.transform.GetComponent<ChoseCard>().enabled = false;
                CardsCanChooseOnWickEnd.Remove(playerFieldCard);
                playerFieldCard.ImageEdge1.color = Color.white;
                playerFieldCard.IsOrderCard = false;
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
            if (!isFriendlyCard)
            {
                List<CardInfoScript> choosenCardList = new List<CardInfoScript>(PlayerFieldCards);

                foreach (CardInfoScript card in PlayerFieldInvulnerabilityCards)
                {
                    if (choosenCardList.Contains(card))
                        choosenCardList.Remove(card);
                }

                return choosenCardList[Random.Range(0, choosenCardList.Count - 1)];
            }

            else
            {
                List<CardInfoScript> choosenCardList = new List<CardInfoScript>(EnemyFieldCards);

                foreach (CardInfoScript card in EnemyFieldInvulnerabilityCards)
                {
                    if (choosenCardList.Contains(card))
                        choosenCardList.Remove(card);
                }

                return choosenCardList[Random.Range(0, choosenCardList.Count - 1)];
            }
        }
    }

    private IEnumerator ChoseCardCoroutine(CardInfoScript playedCard, bool isBoost = false, bool isDamage = false, bool isDestroy = false, bool isSwapPoints = false, bool isEnduranceOrBleeding = false, bool isEnduranceOrBleedingEnemy = false)
    {
        StartChoseCard = playedCard;
        playedCard.ImageEdge1.color = Color.green;
        UIManager.Instance.ChangeEndTurnButtonInteractable(false);

        yield return StartCoroutine(WaitForChoseCard(playedCard));
        IsChooseCard = false;

        if (isBoost)
        {
            if ((playedCard.SelfCard.BoostOrDamage.AddictionWithAlliedField && (PlayerFieldCards.Count != 1 && (PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count) > 1)) ||
            (playedCard.SelfCard.BoostOrDamage.AddictionWithEnemyField && (EnemyFieldCards.Count != 0) &&
            (EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count != 0)))
            {
                CardMechanics.Instance.Self(playedCard, playedCard);
            }

            if (playedCard.SelfCard.StatusEffects.IsShieldOther)
                ChooseCard(true).SelfCard.StatusEffects.IsSelfShielded = true;

            CardMechanics.Instance.Deployment(ChooseCard(true), playedCard);

            RemovePrepareToChoseCard(false);

            if (playedCard.SelfCard.BoostOrDamage.NearBoost > 0)
            {
                ChooseCard(true).CheckSiblingIndex();

                if (CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearBoost, true) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearBoost, true).Count; i++)
                    {
                        CardMechanics.Instance.Deployment(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearBoost, true)[i], playedCard, i + 1);
                    }
                }

                if (CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearBoost, false) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearBoost, false).Count; i++)
                    {
                        CardMechanics.Instance.Deployment(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearBoost, false)[i], playedCard, i + 1);
                    }
                }
            }
        }

        if (isDamage)
        {
            if ((playedCard.SelfCard.BoostOrDamage.AddictionWithAlliedField && (PlayerFieldCards.Count != 1 && (PlayerFieldCards.Count - PlayerFieldInvulnerabilityCards.Count) > 1)) ||
            (playedCard.SelfCard.BoostOrDamage.AddictionWithEnemyField && (EnemyFieldCards.Count != 0)) &&
            (EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count != 0))
            {
                CardMechanics.Instance.Self(playedCard, playedCard);
            }

            CardMechanics.Instance.Deployment(ChooseCard(true, false), playedCard);
            if (playedCard.SelfCard.StatusEffects.IsStunOther)
            {
                ChooseCard(true, false).SelfCard.StatusEffects.IsSelfStunned = true;
                CardMechanics.Instance.CheckStatusEffects(ChooseCard(true, false));
            }

            RemovePrepareToChoseCard(true);

            if (playedCard.SelfCard.BoostOrDamage.NearDamage > 0)
            {
                ChooseCard(true, false).CheckSiblingIndex();

                if (CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearDamage, true) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearDamage, true).Count; i++)
                    {
                        CardMechanics.Instance.Deployment(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearDamage, true)[i], playedCard, i + 1);

                        if (playedCard.SelfCard.StatusEffects.IsStunOther)
                        {
                            CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearDamage, true)[i].SelfCard.StatusEffects.IsSelfStunned = true;
                            CardMechanics.Instance.CheckStatusEffects(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearDamage, true)[i]);
                        }
                    }
                }

                if (CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearDamage, false) != null)
                {
                    for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearDamage, false).Count; i++)
                    {
                        CardMechanics.Instance.Deployment(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearDamage, false)[i], playedCard, i + 1);

                        if (playedCard.SelfCard.StatusEffects.IsStunOther)
                        {
                            CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearDamage, false)[i].SelfCard.StatusEffects.IsSelfStunned = true;
                            CardMechanics.Instance.CheckStatusEffects(CardMechanics.Instance.ReturnNearCard(ChooseCard(true), playedCard.SelfCard.BoostOrDamage.NearDamage, false)[i]);
                        }
                    }
                }
            }
        }

        if (playedCard.SelfCard.Spawns.SpawnCardCount != 0 && playedCard.SelfCard.BoostOrDamage.AddictionWithEnemyField && EnemyFieldCards.Count > 0 && (EnemyFieldCards.Count - EnemyFieldInvulnerabilityCards.Count) > 0)
        {
            CardMechanics.Instance.SpawnCard(playedCard, true);
        }

        if (isDestroy)
        {
            CardMechanics.Instance.DestroyCard(ChooseCard(true), playedCard);

            RemovePrepareToChoseCard(true);
        }

        if (isSwapPoints)
        {
            CardMechanics.Instance.SwapPoints(playedCard, ChooseCard(true));

            RemovePrepareToChoseCard(true);

            RemovePrepareToChoseCard(false);
        }

        if (isEnduranceOrBleeding)
        {
            CardMechanics.Instance.BleedingOrEndurance(playedCard, ChooseCard(true));
            UIManager.Instance.CheckBleeding(ChooseCard(true));

            if (isEnduranceOrBleedingEnemy)
                RemovePrepareToChoseCard(true);
            else
                RemovePrepareToChoseCard(false);
        }

        UIManager.Instance.ChangeLinePosition(0, Vector3.zero);
        UIManager.Instance.ChangeLinePosition(1, Vector3.zero);

        playedCard.ImageEdge1.color = Color.white;
        UIManager.Instance.ChangeEndTurnButtonInteractable(true);

        ChangeEnemyPoints();
        ChangePlayerPoints();

        SoundManager.Instance.PlayerStartEffectSound(playedCard);
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

    private void ClearDestroyedInEndTurnCards()
    {
        foreach (CardInfoScript destroyedCard in PlayerFieldDestroyedInEndTurnCards)
        {
            CardMechanics.Instance.DestroyCard(destroyedCard);
        }

        foreach (CardInfoScript destroyedCard in EnemyFieldDestroyedInEndTurnCards)
        {
            CardMechanics.Instance.DestroyCard(destroyedCard);
        }

        PlayerFieldDestroyedInEndTurnCards.Clear();
        EnemyFieldDestroyedInEndTurnCards.Clear();
    }

    public List<CardInfoScript> EndTurnOrderCard(List<CardInfoScript> cardsInField, bool isPlayerField)
    {
        List<CardInfoScript> temporyList = new List<CardInfoScript>(cardsInField);

        if (isPlayerField)
        {
            for (int i = 0; i < _playerField.childCount; i++)
            {
                    temporyList[i] = _playerField.GetChild(i).GetComponent<CardInfoScript>();
            }
        }
        else
        {
            for (int i = 0; i < _enemyField.childCount; i++)
            {
                temporyList[i] = _enemyField.GetChild(i).GetComponent<CardInfoScript>();
            }
        }

        return temporyList;
    }

    public void NewGame()
    {
        UIManager.Instance.EndGamePanel.SetActive(false);

        StopAllCoroutines();

        IsHandCardPlaying = false;

        _turn = 0;

        _playerPoints = 0;
        _enemyPoints = 0;

        UIManager.Instance.ChangePoints(_playerPoints, _enemyPoints);

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

        _enemyHandPass.SetActive(false);
        _playerHandPass.SetActive(false);
        isPlayerPassed = false;
        isEnemyPassed = false;

        CurrentGame = new Game();

        Deck.Instance.DeleteDeck();
        Deck.Instance.CreateDeck(CurrentGame.PlayerDeck);

        GiveHandCards(CurrentGame.EnemyDeck, _enemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, _playerHand);

        UIManager.Instance.ChangeEndTurnButtonInteractable(true);

        AllCoroutine.Add(StartCoroutine(TurnFunk()));
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void DeckBuild()
    {
        SceneManager.LoadScene("DeckBuild");
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

    private void EndGame()
    {
        StopAllCoroutines();
    }
}
