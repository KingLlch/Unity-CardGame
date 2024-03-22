using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Camera _mainCamera;
    private Vector3 offset;

    [HideInInspector] public UnityEvent ChangeCardPosition;
    [HideInInspector] public Transform CurrentCardParentTransform;
    [HideInInspector] public Transform FutureCardParentTransform;
    [HideInInspector] public int StartSiblingIndex;
    [HideInInspector] public int SiblingIndex;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - _mainCamera.ScreenToWorldPoint(eventData.position);

        CurrentCardParentTransform = transform.parent;
        FutureCardParentTransform = transform.parent;

        StartSiblingIndex = transform.GetSiblingIndex();
        transform.SetParent(CurrentCardParentTransform.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (_mainCamera.ScreenToWorldPoint(eventData.position) + offset) * Vector2.one;
        CheckPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.SetParent(FutureCardParentTransform);
    }

    private void CheckPosition()
    {
        int newIndex = FutureCardParentTransform.childCount;

        for (int i = 0; i < FutureCardParentTransform.childCount; i++)
        {
            if (transform.position.x < FutureCardParentTransform.GetChild(i).position.x)
            {
                newIndex = i;

                if (FutureCardParentTransform.transform.GetSiblingIndex() < newIndex) newIndex--;
                break;
            }
        }

        ChangeCardPosition.Invoke();
        SiblingIndex = newIndex;
    }
}
