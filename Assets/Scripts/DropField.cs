using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DropField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public TypeField TypeField;
    public Transform EmptyTableCard;
    public Transform EmptyHandCard;

    private bool _isChangeEmptyCardPositionInHand;

    [HideInInspector] public UnityEvent<CardInfoScript> DropCard;

    private CardMove card;

    private void Awake()
    {
        EmptyHandCard = GameObject.Find("EmptyHandCard").transform;
        EmptyTableCard = GameObject.Find("EmptyTableCard").transform;
    }

    private void ChangeCardPosition()
    {
        if (_isChangeEmptyCardPositionInHand) EmptyHandCard.transform.SetSiblingIndex(card.SiblingIndex);
        EmptyTableCard.transform.SetSiblingIndex(card.SiblingIndex);
    }

    public void OnPointerEnter(PointerEventData pointer)
    {
        if (pointer.pointerDrag == null) return;

        card = pointer.pointerDrag.GetComponent<CardMove>();
        if (!card.IsDraggable) return;

        card.ChangeCardPosition.AddListener(ChangeCardPosition);
        card.HideEmptyCard.AddListener(HideEmptyCard);

        if (TypeField == TypeField.SELF_TABLE)
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

    }

    public void OnPointerExit(PointerEventData pointer)
    {
        if (pointer.pointerDrag == null) return;
        card = pointer.pointerDrag.GetComponent<CardMove>();

        if ((TypeField == TypeField.SELF_HAND) && (card.IsDraggable))
        {
            _isChangeEmptyCardPositionInHand = false;
            EmptyHandCard.SetParent(transform);
            EmptyHandCard.transform.SetSiblingIndex(card.StartSiblingIndex);
        }

        if (TypeField == TypeField.SELF_TABLE)
        {
            card.FutureCardParentTransform = card.CurrentCardParentTransform;

            EmptyTableCard.SetParent(transform.parent);
        }
    }

    public void OnDrop(PointerEventData pointer)
    {
        if (pointer.pointerDrag == null) return;

        if (!card.IsDraggable) return;

        if ((TypeField == TypeField.SELF_TABLE) || (TypeField == TypeField.SELF_HAND))
        {
            if (TypeField == TypeField.SELF_TABLE)
            {
                if (GameManager.Instance.PlayerFieldCards.Count >= GameManager.Instance.MaxNumberCardInField)
                {
                    GameManager.Instance.ThrowCard(card.GetComponent<CardInfoScript>(), true);
                    GameManager.Instance.IsHandCardPlaying = true;

                    HideEmptyCard();
                    return;
                }

                card.CurrentCardParentTransform = transform;

                card.transform.SetParent(transform);
                card.transform.SetSiblingIndex(card.SiblingIndex);

                HideEmptyCard();

                DropCard.Invoke(card.GetComponent<CardInfoScript>());
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
    }
}

public enum TypeField
{
    SELF_HAND,
    SELF_TABLE,
    ENEMY_HAND,
    ENEMY_TABLE
}
