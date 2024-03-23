using UnityEngine;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private Transform _emptyHandCard;
    private Card card;
    private Table table;

    private void Awake()
    {
        _emptyHandCard = GameObject.Find("EmptyHandCard").transform;
        table = GameObject.FindAnyObjectByType<Table>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        card = eventData.pointerDrag.GetComponent<Card>();

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
           
        table.HideEmptyCard();

        if (card.FutureCardParentTransform == transform)
        {
            HideEmptyCard();
            card.transform.SetParent(transform);
            card.transform.SetSiblingIndex(card.SiblingIndex);
        }
    }

    public void HideEmptyCard()
    {
        _emptyHandCard.SetParent(null);
        _emptyHandCard.transform.position = new Vector2(1000, 0);
    }
}
