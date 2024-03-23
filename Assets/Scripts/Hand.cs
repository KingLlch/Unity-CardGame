using UnityEngine;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private Transform _emptyHandCard;
    private CardMove card;

    private void Awake()
    {
        _emptyHandCard = GameObject.Find("EmptyHandCard").transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        card = eventData.pointerDrag.GetComponent<CardMove>();

        card.ChangeCardPosition.AddListener(ChangeCardPosition);
    }

    private void ChangeCardPosition()
    {
        _emptyHandCard.transform.SetSiblingIndex(card.SiblingIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        _emptyHandCard.SetParent(transform);

        card.ChangeCardPosition.RemoveListener(ChangeCardPosition);
        _emptyHandCard.transform.SetSiblingIndex(card.StartSiblingIndex);
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
        _emptyHandCard.transform.position = new Vector2(1000, 0);
    }
}
