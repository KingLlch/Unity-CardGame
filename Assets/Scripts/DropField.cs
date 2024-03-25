using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class DropField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public TypeField typeField;

    private Transform _emptyTableCard;
    private Transform _emptyHandCard;
    private bool _isChangeEmptyCardPositionInHand;

    private CardMove card;

    private void Awake()
    {
        _emptyHandCard = GameObject.Find("EmptyHandCard").transform;
        _emptyTableCard = GameObject.Find("EmptyTableCard").transform;
    }

    private void ChangeCardPosition()
    {
        if (_isChangeEmptyCardPositionInHand) _emptyHandCard.transform.SetSiblingIndex(card.SiblingIndex);
        _emptyTableCard.transform.SetSiblingIndex(card.SiblingIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        card = eventData.pointerDrag.GetComponent<CardMove>();
        if (!card.IsDraggable) return;

        card.ChangeCardPosition.AddListener(ChangeCardPosition);
        card.HideEmptyCard.AddListener(HideEmptyCard);

        if (typeField == TypeField.SELF_TABLE)
        {
            _emptyTableCard.SetParent(transform);
            _emptyTableCard.SetSiblingIndex(card.SiblingIndex);
            card.FutureCardParentTransform = transform;
        }

        if (typeField == TypeField.SELF_HAND)
        {
            _isChangeEmptyCardPositionInHand = true;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        if (typeField == TypeField.SELF_HAND)
        {
            _isChangeEmptyCardPositionInHand = false;
            _emptyHandCard.SetParent(transform);
            _emptyHandCard.transform.SetSiblingIndex(card.StartSiblingIndex);
        }

        if (typeField == TypeField.SELF_TABLE)
        {
            card.FutureCardParentTransform = card.CurrentCardParentTransform;

            _emptyTableCard.SetParent(null);
            _emptyTableCard.transform.position = new Vector2(2000, 0);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        if (!card.IsDraggable) return;

        if ((typeField == TypeField.SELF_TABLE ) || (typeField == TypeField.SELF_HAND))
        {
            if (typeField == TypeField.SELF_TABLE && card.GameManager.PlayerFieldCards.Count < 9)
            {
                card.GameManager.PlayerHandCards.Remove(card.GetComponent<CardInfoScript>());
                card.GameManager.PlayerHandCards.Add(card.GetComponent<CardInfoScript>());
                card.transform.SetParent(transform);
                card.transform.SetSiblingIndex(card.SiblingIndex);
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
        _emptyHandCard.SetParent(null);
        _emptyTableCard.SetParent(null);
        _emptyHandCard.transform.position = new Vector2(2000, 0);
        _emptyTableCard.transform.position = new Vector2(2000, 0);
    }
}

public enum TypeField
{
    SELF_HAND,
    SELF_TABLE,
    ENEMY_HAND,
    ENEMY_TABLE

}
