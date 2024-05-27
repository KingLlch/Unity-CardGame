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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        card = eventData.pointerDrag.GetComponent<CardMove>();
        if (!card.IsDraggable) return;

        card.ChangeCardPosition.AddListener(ChangeCardPosition);
        card.HideEmptyCard.AddListener(HideEmptyCard);

        if (TypeField == TypeField.SELF_TABLE)
        {
            EmptyTableCard.SetParent(transform);
            EmptyTableCard.SetSiblingIndex(card.SiblingIndex);
            card.FutureCardParentTransform = transform;
        }

        if (TypeField == TypeField.SELF_HAND)
        {
            EmptyHandCard.SetParent(transform);
            _isChangeEmptyCardPositionInHand = true;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        card = eventData.pointerDrag.GetComponent<CardMove>();

        if ((TypeField == TypeField.SELF_HAND) && (card.IsDraggable))
        {
            _isChangeEmptyCardPositionInHand = false;
            EmptyHandCard.SetParent(transform);
            EmptyHandCard.transform.SetSiblingIndex(card.StartSiblingIndex);
        }

        if (TypeField == TypeField.SELF_TABLE)
        {
            card.FutureCardParentTransform = card.CurrentCardParentTransform;

            EmptyTableCard.SetParent(null);
            EmptyTableCard.transform.position = new Vector2(2000, 0);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

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
        EmptyHandCard.SetParent(null);
        EmptyTableCard.SetParent(null);
        EmptyHandCard.transform.position = new Vector2(2000, 0);
        EmptyTableCard.transform.position = new Vector2(2000, 0);
    }
}

public enum TypeField
{
    SELF_HAND,
    SELF_TABLE,
    ENEMY_HAND,
    ENEMY_TABLE

}
