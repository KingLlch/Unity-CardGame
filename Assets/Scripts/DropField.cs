using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class DropField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public TypeField typeField;

    private Transform _emptyTableCard;
    private Transform _emptyHandCard;

    private CardMove card;

    private void Awake()
    {
        _emptyHandCard = GameObject.Find("EmptyHandCard").transform;
        _emptyTableCard = GameObject.Find("EmptyTableCard").transform;
    }

    private void ChangeCardPosition()
    {
        _emptyHandCard.transform.SetSiblingIndex(card.SiblingIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        card = eventData.pointerDrag.GetComponent<CardMove>();
        card.ChangeCardPosition.AddListener(ChangeCardPosition);

        if (typeField == TypeField.SELF_TABLE)
        {
            _emptyTableCard.SetParent(transform);
            card.FutureCardParentTransform = transform;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        if (typeField == TypeField.SELF_HAND)
        {
        _emptyHandCard.SetParent(transform);

        card.ChangeCardPosition.RemoveListener(ChangeCardPosition);
        _emptyHandCard.transform.SetSiblingIndex(card.StartSiblingIndex);
        }

        if (typeField == TypeField.SELF_TABLE)
        {
            if (eventData.pointerDrag == null) return;
            card.FutureCardParentTransform = card.CurrentCardParentTransform;
            HideEmptyCard();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        HideEmptyCard();
        card.transform.SetParent(transform);
        card.transform.SetSiblingIndex(card.SiblingIndex);
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
