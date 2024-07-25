using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CardMechanics : MonoBehaviour
{
    private static CardMechanics _instance;

    [HideInInspector] public UnityEvent<CardInfoScript> EndTurnCardEvent;

    public static CardMechanics Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CardMechanics>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void ChangePoints(CardInfoScript targetCard, CardInfoScript startCard, bool deploymentAction = false, bool selfAction = false, bool endTurnAction = false, int distanceNearCard = 0)
    {
        if (deploymentAction)
        {
            if (startCard.SelfCard.Boost != 0)
            {
                targetCard.ChangePoints(ref targetCard.SelfCard, startCard.SelfCard.Boost + distanceNearCard * startCard.SelfCard.ChangeBoost, startCard.SelfCard);

                if (startCard.SelfCard.Boost > 0) EffectsManager.Instance.Boost(startCard.transform, targetCard.transform);
                else EffectsManager.Instance.Damage(startCard.transform, targetCard.transform);
            }
            if (startCard.SelfCard.Damage != 0)
            {
               targetCard.ChangePoints(ref targetCard.SelfCard, -startCard.SelfCard.Damage - distanceNearCard * startCard.SelfCard.ChangeDamage, startCard.SelfCard);

                if (startCard.SelfCard.Damage > 0) EffectsManager.Instance.Damage(startCard.transform, targetCard.transform);
                else EffectsManager.Instance.Boost(startCard.transform, targetCard.transform);
            }
        }

        if (selfAction)
        {
            if (startCard.SelfCard.SelfBoost != 0)
            {
                targetCard.ChangePoints(ref startCard.SelfCard, startCard.SelfCard.SelfBoost, startCard.SelfCard);
                EffectsManager.Instance.SelfBoost(startCard.transform);
            }
            if (startCard.SelfCard.SelfDamage != 0)
            {
                targetCard.ChangePoints(ref startCard.SelfCard, -startCard.SelfCard.SelfDamage, startCard.SelfCard);
                EffectsManager.Instance.SelfDamage(startCard.transform);
            }
        }

        if (endTurnAction)
        {
            if (startCard.SelfCard.EndTurnBoost != 0)
            {
                targetCard.ChangePoints(ref targetCard.SelfCard, startCard.SelfCard.EndTurnBoost, startCard.SelfCard);

                if (startCard.SelfCard.EndTurnBoost > 0) EffectsManager.Instance.EndTurnBoost(startCard.transform, targetCard.transform);
                else EffectsManager.Instance.EndTurnDamage(startCard.transform, targetCard.transform);
            }
            if (startCard.SelfCard.EndTurnDamage != 0)
            {
                targetCard.ChangePoints(ref targetCard.SelfCard, -startCard.SelfCard.EndTurnDamage, startCard.SelfCard);

                if (startCard.SelfCard.EndTurnDamage > 0) EffectsManager.Instance.EndTurnDamage(startCard.transform, targetCard.transform);
                else EffectsManager.Instance.EndTurnBoost(startCard.transform, targetCard.transform);
            }
        }

        CheckColorPointsCard(targetCard);
        CheckColorPointsCard(startCard);

        targetCard.CheckStatusEffects();
        IsDestroyCard(targetCard);
    }

    public void CheckColorPointsCard(CardInfoScript card)
    {

        if (card.SelfCard.Points == card.SelfCard.MaxPoints)
        {
            card.Point.colorGradient = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
        }

        else if (card.SelfCard.Points < card.SelfCard.MaxPoints)
        {
            card.Point.colorGradient = new VertexGradient(Color.red, Color.red, Color.white, Color.white);
        }

        else
        {
            card.Point.colorGradient = new VertexGradient(Color.green, Color.green, Color.white, Color.white);
        }
    }

    public void IsDestroyCard(CardInfoScript card)
    {
        if (card.SelfCard.Points <= 0)
        {
            card.SelfCard.Points = 0;

            if (GameManager.Instance.PlayerFieldCards.Contains(card))
                GameManager.Instance.PlayerFieldCards.Remove(card);

            else if (GameManager.Instance.EnemyFieldCards.Contains(card))
                GameManager.Instance.EnemyFieldCards.Remove(card);

            if (GameManager.Instance.PlayerFieldInvulnerabilityCards.Contains(card))
                GameManager.Instance.PlayerFieldInvulnerabilityCards.Remove(card);

            else if (GameManager.Instance.EnemyFieldInvulnerabilityCards.Contains(card))
                    GameManager.Instance.EnemyFieldInvulnerabilityCards.Remove(card);

            EffectsManager.Instance.StartDestroyCoroutine(card);

            Destroy(card.DescriptionObject);
            Destroy(card.gameObject, 1f);
        }
    }

    public void EndTurnActions()
    {
        if (GameManager.Instance.IsPlayerTurn)
        {
            foreach (CardInfoScript card in GameManager.Instance.PlayerFieldCards)
            {
                if (card.SelfCard.EndTurnAction == true && !card.SelfCard.StatusEffects.IsStunned)
                {
                    for (int i = 0; i < card.SelfCard.EndTurnActionQuantity; i++)
                    {

                        if ((card.SelfCard.EndTurnDamage != 0) && (GameManager.Instance.EnemyFieldCards.Count > 0))
                        {
                            ChangePoints(GameManager.Instance.EnemyFieldCards[Random.Range(0, GameManager.Instance.EnemyFieldCards.Count)], card, false, false, true);
                            EndTurnCardEvent.Invoke(card);
                        }

                        if ((card.SelfCard.EndTurnBoost != 0) && (GameManager.Instance.PlayerFieldCards.Count > 0))
                        {
                            ChangePoints(GameManager.Instance.PlayerFieldCards[Random.Range(0, GameManager.Instance.PlayerFieldCards.Count)], card, false, false, true);
                            EndTurnCardEvent.Invoke(card);
                        }
                    }
                }

                if (card.SelfCard.StatusEffects.IsStunned)
                {
                    card.SelfCard.StatusEffects.IsStunned = false;
                    card.CheckStatusEffects();
                }
            }
        }

        else
        {
            foreach (CardInfoScript card in GameManager.Instance.EnemyFieldCards)
            {
                if (card.SelfCard.EndTurnAction == true && !card.SelfCard.StatusEffects.IsStunned)
                {
                    for (int i = 0; i < card.SelfCard.EndTurnActionQuantity; i++)
                    {

                        if ((card.SelfCard.EndTurnDamage != 0) && (GameManager.Instance.PlayerFieldCards.Count > 0))
                        {
                            ChangePoints(GameManager.Instance.PlayerFieldCards[Random.Range(0, GameManager.Instance.PlayerFieldCards.Count)], card, false, false, true);
                            EndTurnCardEvent.Invoke(card);
                        }

                        if ((card.SelfCard.EndTurnBoost != 0) && (GameManager.Instance.EnemyFieldCards.Count > 0))
                        {
                            ChangePoints(GameManager.Instance.EnemyFieldCards[Random.Range(0, GameManager.Instance.EnemyFieldCards.Count)], card, false, false, true);
                            EndTurnCardEvent.Invoke(card);
                        }
                    }
                }

                if (card.SelfCard.StatusEffects.IsStunned)
                {
                    card.SelfCard.StatusEffects.IsStunned = false;
                    card.CheckStatusEffects();
                }
            }
        }
    }

    public void SpawnCard(CardInfoScript card, bool player)
    {
        GameObject summonCard;

        if (card.SelfCard.SummonCardNumber == -1)
        {
            for (int i = 0; i < card.SelfCard.SummonCardCount; i++)
            {
                if (!((player && GameManager.Instance.PlayerFieldCards.Count < GameManager.Instance.MaxNumberCardInField) ||
                    (!player && GameManager.Instance.EnemyFieldCards.Count < GameManager.Instance.MaxNumberCardInField)))
                    return;

                summonCard = Instantiate(GameManager.Instance.CardPref, card.transform.parent, false);
                CardInfoScript summonCardInfo = summonCard.GetComponent<CardInfoScript>();

                card.CheckSiblingIndex();
                if ((i == 0) && (i % 2 == 0))
                    summonCard.transform.SetSiblingIndex(card.SiblingIndex);
                else
                    summonCard.transform.SetSiblingIndex(card.SiblingIndex + 1);

                if (player) GameManager.Instance.PlayerFieldCards.Add(summonCardInfo);
                else GameManager.Instance.EnemyFieldCards.Add(summonCardInfo);
                summonCardInfo.ShowCardInfo(card.SelfCard);
                summonCardInfo.SelfCard.StatusEffects.IsIllusion = true;
                summonCardInfo.CheckStatusEffects();
                summonCard.GetComponent<ChoseCard>().enabled = false;

            }
        }

        else
        {
            for (int i = 0; i < card.SelfCard.SummonCardCount; i++)
            {
                if (!((player && GameManager.Instance.PlayerFieldCards.Count < GameManager.Instance.MaxNumberCardInField) ||
                (!player && GameManager.Instance.EnemyFieldCards.Count < GameManager.Instance.MaxNumberCardInField)))
                    return;

                summonCard = Instantiate(GameManager.Instance.CardPref, card.transform.parent, false);
                CardInfoScript summonCardInfo = summonCard.GetComponent<CardInfoScript>();

                card.CheckSiblingIndex();
                if ((i == 0) && (i % 2 == 0))
                    summonCard.transform.SetSiblingIndex(card.SiblingIndex);
                else
                    summonCard.transform.SetSiblingIndex(card.SiblingIndex + 1);

                if (player) GameManager.Instance.PlayerFieldCards.Add(summonCardInfo);
                else GameManager.Instance.EnemyFieldCards.Add(summonCardInfo);
                summonCardInfo.ShowCardInfo(CardManagerList.SummonCards[card.SelfCard.SummonCardNumber]);
                summonCard.GetComponent<ChoseCard>().enabled = false;
            }
        }
    }

}
