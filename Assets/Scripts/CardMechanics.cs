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

                if (startCard.SelfCard.Boost > 0) EffectsManager.Instance.ParticleEffects(startCard.transform, targetCard.transform, true);
                else EffectsManager.Instance.ParticleEffects(startCard.transform, targetCard.transform, false);
            }
            if (startCard.SelfCard.Damage != 0)
            {
                targetCard.ChangePoints(ref targetCard.SelfCard, -startCard.SelfCard.Damage - distanceNearCard * startCard.SelfCard.ChangeDamage, startCard.SelfCard);

                if (startCard.SelfCard.Damage > 0) EffectsManager.Instance.ParticleEffects(startCard.transform, targetCard.transform, false);
                else EffectsManager.Instance.ParticleEffects(startCard.transform, targetCard.transform, true);
            }
        }

        if (selfAction)
        {
            if (startCard.SelfCard.SelfBoost != 0)
            {
                targetCard.ChangePoints(ref startCard.SelfCard, startCard.SelfCard.SelfBoost, startCard.SelfCard);
                EffectsManager.Instance.ParticleEffects(startCard.transform, targetCard.transform, true, true);
            }
            if (startCard.SelfCard.SelfDamage != 0)
            {
                targetCard.ChangePoints(ref startCard.SelfCard, -startCard.SelfCard.SelfDamage, startCard.SelfCard);
                EffectsManager.Instance.ParticleEffects(startCard.transform, targetCard.transform, false, true);
            }
        }

        if (endTurnAction)
        {
            if (startCard.SelfCard.EndTurnBoost != 0)
            {
                targetCard.ChangePoints(ref targetCard.SelfCard, startCard.SelfCard.EndTurnBoost, startCard.SelfCard);

                if (startCard.SelfCard.EndTurnBoost > 0) EffectsManager.Instance.ParticleEffects(startCard.transform, targetCard.transform, true);
                else EffectsManager.Instance.ParticleEffects(startCard.transform, targetCard.transform, false);
            }
            if (startCard.SelfCard.EndTurnDamage != 0)
            {
                targetCard.ChangePoints(ref targetCard.SelfCard, -startCard.SelfCard.EndTurnDamage, startCard.SelfCard);

                if (startCard.SelfCard.EndTurnDamage > 0) EffectsManager.Instance.ParticleEffects(startCard.transform, targetCard.transform, false);
                else EffectsManager.Instance.ParticleEffects(startCard.transform, targetCard.transform, true);
            }
        }

        UIManager.Instance.CheckColorPointsCard(targetCard);
        UIManager.Instance.CheckColorPointsCard(startCard);

        CheckStatusEffects(targetCard);

        IsDestroyCard(targetCard);
        IsDestroyCard(startCard);
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
                    CheckStatusEffects(card);
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
                    CheckStatusEffects(card);
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
                CheckStatusEffects(summonCardInfo);
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

    public void CheckStatusEffects(CardInfoScript card)
    {
        if (card.SelfCard.StatusEffects.IsShielded && card.StatusEffectShield == null)
        {
            card.CardStatusEffectImage.material = new Material(EffectsManager.Instance.shieldMaterial);
            card.StatusEffectShield = Instantiate(card.StatusEffectPrefab, card.CardStatusEffectImage.gameObject.transform);
            card.StatusEffectShield.GetComponent<StatusEffect>().Initialize(StatusEffectsType.shield);
        }

        else if (card.SelfCard.StatusEffects.IsShielded && card.StatusEffectShield != null)
        {
            card.CardStatusEffectImage.material = null;
            Destroy(card.StatusEffectShield);
            card.StatusEffectShield = null;
        }

        if (card.SelfCard.StatusEffects.IsIllusion && card.StatusEffectIllusion == null)
        {
            card.CardStatusEffectImage.material = new Material(EffectsManager.Instance.illusionMaterial);
            card.StatusEffectIllusion = Instantiate(card.StatusEffectPrefab, card.CardStatusEffectImage.gameObject.transform);
            card.StatusEffectIllusion.GetComponent<StatusEffect>().Initialize(StatusEffectsType.illusion);
        }

        if (card.SelfCard.StatusEffects.IsStunned && card.StatusEffectStunned == null)
        {
            card.StatusEffectStunned = Instantiate(card.StatusEffectPrefab, card.CardStatusEffectImage.gameObject.transform);
            card.StatusEffectStunned.GetComponent<StatusEffect>().Initialize(StatusEffectsType.stun);
        }

        else if (card.SelfCard.StatusEffects.IsStunned && card.StatusEffectStunned != null)
        {
            Destroy(card.StatusEffectStunned);
            card.StatusEffectStunned = null;
        }

        if (card.SelfCard.StatusEffects.IsInvulnerability && card.StatusEffectInvulnerability == null)
        {
            card.CardStatusEffectImage.material = new Material(EffectsManager.Instance.invulnerabilityMaterial);
            card.StatusEffectInvulnerability = Instantiate(card.StatusEffectPrefab, card.CardStatusEffectImage.gameObject.transform);
            card.StatusEffectInvulnerability.GetComponent<StatusEffect>().Initialize(StatusEffectsType.invulnerability);
        }

        if (card.SelfCard.StatusEffects.IsInvisibility && card.StatusEffectInvisibility == null)
        {
            card.CardStatusEffectImage.material = new Material(EffectsManager.Instance.invisibilityMaterial);
            card.StatusEffectInvisibility = Instantiate(card.StatusEffectPrefab, card.CardStatusEffectImage.gameObject.transform);
            card.StatusEffectInvisibility.GetComponent<StatusEffect>().Initialize(StatusEffectsType.invisibility);
        }
    }

}
