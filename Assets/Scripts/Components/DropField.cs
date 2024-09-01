using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public TypeField TypeField;
    public Transform EmptyTableCard;
    public Transform EmptyHandCard;
    public Transform EmptyEnemyTableCard;

    private bool _isChangeEmptyCardPositionInHand;

    [HideInInspector] public UnityEvent<CardInfoScript> DropCard;

    private CardMove card;
    private CardInfoScript cardInfo;

    private void Awake()
    {
        EmptyHandCard = GameObject.Find("EmptyHandCard").transform;
        EmptyTableCard = GameObject.Find("EmptyTableCard").transform;
    }

    private void ChangeCardPosition()
    {
        if (_isChangeEmptyCardPositionInHand)
            EmptyHandCard.transform.SetSiblingIndex(card.SiblingIndex);
        EmptyTableCard.transform.SetSiblingIndex(card.SiblingIndex);

        if (cardInfo.SelfCard.StatusEffects.IsInvisibility)
        {
            EmptyEnemyTableCard.transform.SetSiblingIndex(card.SiblingIndex);
        }
    }

    public void OnPointerEnter(PointerEventData pointer)
    {
        if (pointer.pointerDrag == null) return;

        card = pointer.pointerDrag.GetComponent<CardMove>();
        cardInfo = card.GetComponent<CardInfoScript>();
        if (!card.IsDraggable) return;

        ChildRayCast(false);

        card.ChangeCardPosition.AddListener(ChangeCardPosition);
        card.HideEmptyCard.AddListener(HideEmptyCard);

        if (TypeField == TypeField.SELF_TABLE && !cardInfo.SelfCard.StatusEffects.IsInvisibility)
        {
            EmptyTableCard.SetParent(transform);
            EmptyTableCard.SetSiblingIndex(card.SiblingIndex);
            EmptyTableCard.transform.position = new Vector3(EmptyTableCard.transform.position.x, EmptyTableCard.transform.position.y, 9);
            card.FutureCardParentTransform = transform;
        }

        if (TypeField == TypeField.SELF_HAND)
        {
            EmptyHandCard.SetParent(transform);
            EmptyHandCard.transform.position = new Vector3(EmptyHandCard.transform.position.x, EmptyHandCard.transform.position.y, 0);
            _isChangeEmptyCardPositionInHand = true;
        }

        if (TypeField == TypeField.ENEMY_TABLE && cardInfo.SelfCard.StatusEffects.IsInvisibility)
        {
            EmptyEnemyTableCard.SetParent(transform);
            EmptyEnemyTableCard.SetSiblingIndex(card.SiblingIndex);
            EmptyEnemyTableCard.transform.position = new Vector3(EmptyEnemyTableCard.transform.position.x, EmptyEnemyTableCard.transform.position.y, 9);
            card.FutureCardParentTransform = transform;
        }

    }

    public void OnPointerExit(PointerEventData pointer)
    {
        if (pointer.pointerDrag == null) return;
        card = pointer.pointerDrag.GetComponent<CardMove>();
        ChildRayCast(true);

        if ((TypeField == TypeField.SELF_HAND) && (card.IsDraggable))
        {
            _isChangeEmptyCardPositionInHand = false;
            EmptyHandCard.SetParent(transform);
            EmptyHandCard.transform.SetSiblingIndex(card.StartSiblingIndex);
        }

        if (TypeField == TypeField.SELF_TABLE && !cardInfo.SelfCard.StatusEffects.IsInvisibility)
        {
            card.FutureCardParentTransform = card.CurrentCardParentTransform;

            EmptyTableCard.SetParent(transform.parent);
        }

        if (TypeField == TypeField.ENEMY_TABLE && cardInfo.SelfCard.StatusEffects.IsInvisibility)
        {
            card.FutureCardParentTransform = card.CurrentCardParentTransform;

            EmptyEnemyTableCard.SetParent(transform.parent);
        }
    }

    public void OnDrop(PointerEventData pointer)
    {
        if (pointer.pointerDrag == null) return;

        if (!card.IsDraggable) return;

        ChildRayCast(true);

        if ((TypeField == TypeField.SELF_TABLE && !cardInfo.SelfCard.StatusEffects.IsInvisibility) ||
            (TypeField == TypeField.SELF_HAND) ||
            (TypeField == TypeField.ENEMY_TABLE && cardInfo.SelfCard.StatusEffects.IsInvisibility))
        {
            if (TypeField == TypeField.SELF_TABLE && !cardInfo.SelfCard.StatusEffects.IsInvisibility)
            {
                if (GameManager.Instance.PlayerFieldCards.Count >= GameManager.Instance.MaxNumberCardInField)
                {
                    GameManager.Instance.ThrowCard(cardInfo, true);
                    GameManager.Instance.IsHandCardPlaying = true;

                    HideEmptyCard();
                    return;
                }

                card.CurrentCardParentTransform = transform;

                card.transform.SetParent(transform);
                card.transform.SetSiblingIndex(card.SiblingIndex);

                HideEmptyCard();

                DropCard.Invoke(cardInfo);
            }

            else if (TypeField == TypeField.ENEMY_TABLE && cardInfo.SelfCard.StatusEffects.IsInvisibility)
            {
                if (GameManager.Instance.EnemyFieldCards.Count >= GameManager.Instance.MaxNumberCardInField)
                {
                    GameManager.Instance.ThrowCard(cardInfo, true);
                    GameManager.Instance.IsHandCardPlaying = true;

                    HideEmptyCard();
                    return;
                }

                card.CurrentCardParentTransform = transform;

                card.transform.SetParent(transform);
                card.transform.SetSiblingIndex(card.SiblingIndex);

                HideEmptyCard();

                DropCard.Invoke(cardInfo);
            }

            else
            {
                card.transform.SetParent(card.CurrentCardParentTransform);
                card.transform.SetSiblingIndex(card.SiblingIndex);
            }
        }

        else
        {
            card.transform.SetParent(card.CurrentCardParentTransform);
            card.transform.SetSiblingIndex(card.StartSiblingIndex);
        }
    }

    public void HideEmptyCard()
    {
        EmptyHandCard.SetParent(transform.parent);
        EmptyTableCard.SetParent(transform.parent);
        EmptyEnemyTableCard.SetParent(transform.parent);
    }

    private void ChildRayCast(bool isOn)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Image>())
                transform.GetChild(i).GetComponent<Image>().raycastTarget = isOn;
        }
    }

}

public enum TypeField
{
    SELF_HAND,
    SELF_TABLE,
    ENEMY_HAND,
    ENEMY_TABLE
}
