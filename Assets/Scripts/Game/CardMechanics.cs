using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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

    public void Deployment(CardInfoScript targetCard, CardInfoScript startCard,  int distanceNearCard = 0)
    {
        if (startCard.SelfCard.BoostOrDamage.Boost != 0)
        {
            ChangeCardPoints(startCard, targetCard, startCard.SelfCard.BoostOrDamage.Boost + distanceNearCard * startCard.SelfCard.BoostOrDamage.ChangeNearBoost);

            EffectsManager.Instance.StartParticleEffects(startCard.transform, targetCard.transform, startCard.SelfCard.BoostOrDamage.Boost);
        }

        if (startCard.SelfCard.BoostOrDamage.Damage != 0)
        {
            ChangeCardPoints(startCard, targetCard, -startCard.SelfCard.BoostOrDamage.Damage - distanceNearCard * startCard.SelfCard.BoostOrDamage.ChangeNearDamage);

            EffectsManager.Instance.StartParticleEffects(startCard.transform, targetCard.transform, startCard.SelfCard.BoostOrDamage.Damage);
        }

        CheckUICards(targetCard, startCard);
    }

    public void Self(CardInfoScript startCard, CardInfoScript targetCard)
    {
        if (startCard.SelfCard.BoostOrDamage.SelfBoost != 0)
        {
            ChangeCardPoints(startCard, startCard, startCard.SelfCard.BoostOrDamage.SelfBoost);

            EffectsManager.Instance.StartParticleEffects(startCard.transform, targetCard.transform, startCard.SelfCard.BoostOrDamage.SelfBoost);
        }

        if (startCard.SelfCard.BoostOrDamage.SelfDamage != 0)
        {
            ChangeCardPoints(startCard, startCard, -startCard.SelfCard.BoostOrDamage.SelfDamage);

            EffectsManager.Instance.StartParticleEffects(startCard.transform, targetCard.transform, startCard.SelfCard.BoostOrDamage.SelfDamage);
        }

        CheckUICards(targetCard,startCard);
    }

    public void EndTurn(CardInfoScript startCard, CardInfoScript targetCard = null, bool isSelfOrNear = false)
    {
        if (startCard.SelfCard.EndTurnActions.EndTurnSelfBoost != 0 && isSelfOrNear)
        {
            ChangeCardPoints(startCard, startCard, startCard.SelfCard.EndTurnActions.EndTurnSelfBoost);

            EffectsManager.Instance.StartParticleEffects(startCard.transform, startCard.transform, startCard.SelfCard.EndTurnActions.EndTurnSelfBoost);
        }

        if (startCard.SelfCard.EndTurnActions.EndTurnSelfDamage != 0 && isSelfOrNear)
        {
            ChangeCardPoints(startCard, startCard, startCard.SelfCard.EndTurnActions.EndTurnSelfDamage);

            EffectsManager.Instance.StartParticleEffects(startCard.transform, startCard.transform, startCard.SelfCard.EndTurnActions.EndTurnSelfDamage);
        }

        if (startCard.SelfCard.EndTurnActions.EndTurnNearBoost != 0 && isSelfOrNear)
        {
            startCard.CheckSiblingIndex();

            if (ReturnNearCard(startCard, 1, false) != null)
            {
                ChangeCardPoints(startCard, ReturnNearCard(startCard, 1, false)[0], startCard.SelfCard.EndTurnActions.EndTurnNearBoost);
                EffectsManager.Instance.StartParticleEffects(startCard.transform, ReturnNearCard(startCard, 1, false)[0].transform, startCard.SelfCard.EndTurnActions.EndTurnNearBoost);
            }

            if (ReturnNearCard(startCard, 1, true) != null)
            {
                ChangeCardPoints(startCard, ReturnNearCard(startCard, 1, true)[0], startCard.SelfCard.EndTurnActions.EndTurnNearBoost);
                EffectsManager.Instance.StartParticleEffects(startCard.transform, ReturnNearCard(startCard, 1, true)[0].transform, startCard.SelfCard.EndTurnActions.EndTurnNearBoost);
            }
        }

        if (startCard.SelfCard.EndTurnActions.EndTurnNearDamage != 0 && isSelfOrNear)
        {
            startCard.CheckSiblingIndex();

            if (ReturnNearCard(startCard, 1, false) != null)
            {
                ChangeCardPoints(startCard, ReturnNearCard(startCard, 1, false)[0], startCard.SelfCard.EndTurnActions.EndTurnNearDamage);
                EffectsManager.Instance.StartParticleEffects(startCard.transform, ReturnNearCard(startCard, 1, false)[0].transform, startCard.SelfCard.EndTurnActions.EndTurnNearDamage);
            }
            if (ReturnNearCard(startCard, 1, true) != null)
            {
                ChangeCardPoints(startCard, ReturnNearCard(startCard, 1, true)[0],  startCard.SelfCard.EndTurnActions.EndTurnNearBoost);
                EffectsManager.Instance.StartParticleEffects(startCard.transform, ReturnNearCard(startCard, 1, true)[0].transform, startCard.SelfCard.EndTurnActions.EndTurnNearBoost);
            }
        }

        if (startCard.SelfCard.EndTurnActions.EndTurnRandomBoost != 0 && !isSelfOrNear)
        {
            ChangeCardPoints(startCard, targetCard,  startCard.SelfCard.EndTurnActions.EndTurnRandomBoost);

            EffectsManager.Instance.StartParticleEffects(startCard.transform, targetCard.transform, startCard.SelfCard.EndTurnActions.EndTurnRandomBoost);
        }

        if (startCard.SelfCard.EndTurnActions.EndTurnRandomDamage != 0 && !isSelfOrNear)
        {
            ChangeCardPoints(startCard, targetCard, -startCard.SelfCard.EndTurnActions.EndTurnRandomDamage);

            EffectsManager.Instance.StartParticleEffects(startCard.transform, targetCard.transform, -startCard.SelfCard.EndTurnActions.EndTurnRandomDamage);
        }

        CheckUICards(targetCard, startCard);
    }

    public void BleedingOrEndurance(CardInfoScript startCard, CardInfoScript targetCard, bool isBleed = false)
    {
        if (isBleed)
            targetCard.SelfCard.StatusEffects.SelfBleeding += startCard.SelfCard.StatusEffects.BleedingOther;
        else
            targetCard.SelfCard.StatusEffects.SelfEndurance += startCard.SelfCard.StatusEffects.SelfEndurance;
    }

    private void CheckUICards(CardInfoScript targetCard, CardInfoScript startCard)
    {
        if (targetCard != null)
        {
            UIManager.Instance.CheckColorPointsCard(targetCard);
            CheckStatusEffects(targetCard);
            IsDestroyCard(targetCard);

        }

        if (startCard != null)
        {
            UIManager.Instance.CheckColorPointsCard(startCard);
            IsDestroyCard(startCard);
        }
    }

    public void ChangeCardPoints(CardInfoScript startCardInfo, CardInfoScript targetCardInfo,  int value)
    {
        ref Card targetCard = ref targetCardInfo.SelfCard;
        ref Card startCard = ref startCardInfo.SelfCard;

        if ((targetCard.StatusEffects.IsIllusion) && (value < 0))
        {
            value += value;
        }

        if ((targetCard.StatusEffects.IsSelfShielded) && (value < 0))
        {
            value = 0;
            targetCard.StatusEffects.IsSelfShielded = false;
        }

        if (targetCard.UniqueMechanics.HealDamageValue != 0 && (value < 0))
        {
            if (targetCard.UniqueMechanics.HealDamageValue == -1)
                value = -value;
            else
            {
                value = -targetCard.UniqueMechanics.HealDamageValue;
            }

        }

        if (targetCard.UniqueMechanics.ReturnDamageValue != 0 && (value < 0) && targetCardInfo != startCardInfo)
        {
            if (targetCard.UniqueMechanics.ReturnDamageValue == -1)
            {
                ChangeCardPoints(targetCardInfo, startCardInfo, value);
            }
            else
            {
                ChangeCardPoints(targetCardInfo, startCardInfo, 1);
            }
        }

        targetCard.BaseCard.Points += value;

        Debug.Log(startCard.BaseCard.Name + " изменила силу " + targetCard.BaseCard.Name + " в размере " + value + "\n" + (targetCard.BaseCard.Points - value) + " => " + targetCard.BaseCard.Points);

        CheckUICards(targetCardInfo, startCardInfo);
        ShowPointsUI(targetCardInfo);
    }

    public void ShowPointsUI(CardInfoScript cardInfo)
    {
        cardInfo.Point.text = cardInfo.SelfCard.BaseCard.Points.ToString();
    }

    public void IsDestroyCard(CardInfoScript card)
    {
        if (card.SelfCard.BaseCard.Points <= 0)
        {
            card.SelfCard.BaseCard.Points = 0;

            DestroyCard(card);
        }
    }

    public void DestroyCard(CardInfoScript card, CardInfoScript startCard = null)
    {
        if (startCard != null)
            Debug.Log(startCard.SelfCard.BaseCard.Name + " уничтожила " + card.SelfCard.BaseCard.Name);

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

    public void SwapPoints(CardInfoScript firstCard, CardInfoScript secondCard)
    {
        int temporaryVariable;
        temporaryVariable = firstCard.SelfCard.BaseCard.Points;
        firstCard.SelfCard.BaseCard.Points = secondCard.SelfCard.BaseCard.Points;
        secondCard.SelfCard.BaseCard.Points = temporaryVariable;

        CheckUICards(firstCard, secondCard);

        ShowPointsUI(firstCard);
        ShowPointsUI(secondCard);
    }

    public IEnumerator EndTurnActions()
    {
        if (GameManager.Instance.IsPlayerTurn)
        {
            foreach (CardInfoScript card in GameManager.Instance.PlayerFieldCards)
            {
                if (card.SelfCard.EndTurnActions.EndTurnActionCount > 0 && !card.SelfCard.StatusEffects.IsSelfStunned)
                {
                    for (int i = 0; i < card.SelfCard.EndTurnActions.EndTurnActionCount; i++)
                    {
                        EndTurn(card, isSelfOrNear:true);

                        if ((card.SelfCard.EndTurnActions.EndTurnRandomDamage != 0) && (GameManager.Instance.EnemyFieldCards.Count > 0))
                        {
                            EndTurn(card, GameManager.Instance.EnemyFieldCards[Random.Range(0, GameManager.Instance.EnemyFieldCards.Count)], false);
                        }

                        if ((card.SelfCard.EndTurnActions.EndTurnRandomBoost != 0) && (GameManager.Instance.PlayerFieldCards.Count > 0))
                        {
                            EndTurn(card,  GameManager.Instance.PlayerFieldCards[Random.Range(0, GameManager.Instance.PlayerFieldCards.Count)], false);
                        }
                    }

                    EndTurnCardEvent.Invoke(card);

                    yield return new WaitForSeconds(0.5f);
                }

                if (card.SelfCard.StatusEffects.IsSelfStunned)
                {
                    card.SelfCard.StatusEffects.IsSelfStunned = false;
                    CheckStatusEffects(card);
                }

                if (card.SelfCard.StatusEffects.SelfBleeding > 0)
                {
                    ChangeCardPoints(card, card, -1);
                    card.SelfCard.StatusEffects.SelfBleeding--;
                    CheckBleeding(card);
                }

                if (card.SelfCard.StatusEffects.SelfEndurance > 0)
                {
                    ChangeCardPoints(card, card, 1);
                    card.SelfCard.StatusEffects.SelfEndurance--;
                    CheckBleeding(card);
                }
            }
        }

        else
        {
            foreach (CardInfoScript card in GameManager.Instance.EnemyFieldCards)
            {
                if (card.SelfCard.EndTurnActions.EndTurnActionCount > 0 && !card.SelfCard.StatusEffects.IsSelfStunned)
                {
                    for (int i = 0; i < card.SelfCard.EndTurnActions.EndTurnActionCount; i++)
                    {
                        EndTurn(card, isSelfOrNear: true);

                        if ((card.SelfCard.EndTurnActions.EndTurnRandomDamage != 0) && (GameManager.Instance.PlayerFieldCards.Count > 0))
                        {
                            EndTurn(card, GameManager.Instance.PlayerFieldCards[Random.Range(0, GameManager.Instance.PlayerFieldCards.Count)], false);
                        }

                        if ((card.SelfCard.EndTurnActions.EndTurnRandomBoost != 0) && (GameManager.Instance.EnemyFieldCards.Count > 0))
                        {
                            EndTurn(card, GameManager.Instance.EnemyFieldCards[Random.Range(0, GameManager.Instance.EnemyFieldCards.Count)], false);
                        }
                    }

                    EndTurnCardEvent.Invoke(card);

                    yield return new WaitForSeconds(0.5f);
                }

                if (card.SelfCard.StatusEffects.IsSelfStunned)
                {
                    card.SelfCard.StatusEffects.IsSelfStunned = false;
                    CheckStatusEffects(card);
                }

                if (card.SelfCard.StatusEffects.SelfBleeding > 0)
                {
                    ChangeCardPoints(card, card, -1);
                    card.SelfCard.StatusEffects.SelfBleeding--;
                    CheckBleeding(card);
                }

                if (card.SelfCard.StatusEffects.SelfEndurance > 0)
                {
                    ChangeCardPoints(card, card, 1);
                    card.SelfCard.StatusEffects.SelfEndurance--;
                    CheckBleeding(card);
                }
            }
        }

        yield break;
    }

    public void SpawnCard(CardInfoScript card, bool player)
    {
        GameObject summonCard;

        if (card.SelfCard.Summons.SummonCardNumber == -1)
        {
            for (int i = 0; i < card.SelfCard.Summons.SummonCardCount; i++)
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
                summonCard.AddComponent<ChoseCard>();
                summonCard.GetComponent<ChoseCard>().enabled = false;

            }
        }

        else
        {
            for (int i = 0; i < card.SelfCard.Summons.SummonCardCount; i++)
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
                summonCardInfo.ShowCardInfo(CardManagerList.SummonCards[card.SelfCard.Summons.SummonCardNumber]);
                summonCard.AddComponent<ChoseCard>();
                summonCard.GetComponent<ChoseCard>().enabled = false;
            }
        }
    }

    public List<CardInfoScript> ReturnNearCard(CardInfoScript card, int range, bool IsRight)
    {
        List<CardInfoScript> RightNearCard = new List<CardInfoScript>();
        List<CardInfoScript> LeftNearCard = new List<CardInfoScript>();

        for (int i = 1; i <= range; i++)
        {
            if ((IsRight) && (card.SiblingIndex + i < card.transform.parent.childCount))
            {
                RightNearCard.Add(card.transform.parent.GetChild(card.SiblingIndex + i).GetComponent<CardInfoScript>());
            }

            else if ((!IsRight) && (card.SiblingIndex - i >= 0))
            {
                LeftNearCard.Add(card.transform.parent.GetChild(card.SiblingIndex - i).GetComponent<CardInfoScript>());
            }
        }

        if ((IsRight) && (RightNearCard.Count != 0))
            return RightNearCard;
        else if ((!IsRight) && (LeftNearCard.Count != 0))
            return LeftNearCard;
        else return null;
    }

    public void CheckStatusEffects(CardInfoScript card)
    {
        if (card.SelfCard.StatusEffects.IsSelfShielded && card.StatusEffectShield == null)
        {
            card.CardStatusEffectImage.material = new Material(EffectsManager.Instance.shieldMaterial);
            card.StatusEffectShield = Instantiate(card.StatusEffectPrefab, card.CardStatusEffectImage.gameObject.transform);
            card.StatusEffectShield.GetComponent<StatusEffect>().Initialize(StatusEffectsType.shield);
        }

        else if (!card.SelfCard.StatusEffects.IsSelfShielded && card.StatusEffectShield != null)
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

        if (card.SelfCard.StatusEffects.IsSelfStunned && card.StatusEffectStunned == null)
        {
            card.StatusEffectStunned = Instantiate(card.StatusEffectPrefab, card.CardStatusEffectImage.gameObject.transform);
            card.StatusEffectStunned.GetComponent<StatusEffect>().Initialize(StatusEffectsType.stun);
        }

        else if (!card.SelfCard.StatusEffects.IsSelfStunned && card.StatusEffectStunned != null)
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

        CheckBleeding(card);
    }

    public void CheckBleeding(CardInfoScript card)
    {
        if (card.SelfCard.StatusEffects.SelfBleeding > 0)
        {
            card.BleedingPanel.SetActive(true);
            card.BleedingPanel.GetComponent<Image>().color = Color.red;
            card.BleedingPanelText.text = card.SelfCard.StatusEffects.SelfBleeding.ToString();
        }

        else if (card.SelfCard.StatusEffects.SelfEndurance > 0)
        {
            card.BleedingPanel.SetActive(true);
            card.BleedingPanel.GetComponent<Image>().color = Color.green;
            card.BleedingPanelText.text = card.SelfCard.StatusEffects.SelfEndurance.ToString();
        }

        else
        {
            card.BleedingPanel.SetActive(false);
        }
    }
}
