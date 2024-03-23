using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Camera _mainCamera;
    private Vector3 _offset;

    [HideInInspector] public int StartSiblingIndex;
    [HideInInspector] public int SiblingIndex;

    [HideInInspector] public Transform CurrentCardParentTransform;
    [HideInInspector] public Transform FutureCardParentTransform;

    [HideInInspector] public UnityEvent ChangeCardPosition;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offset = transform.position - _mainCamera.ScreenToWorldPoint(eventData.position);

        CurrentCardParentTransform = transform.parent;
        FutureCardParentTransform = transform.parent;

        StartSiblingIndex = transform.GetSiblingIndex();
        transform.SetParent(CurrentCardParentTransform.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (_mainCamera.ScreenToWorldPoint(eventData.position) + _offset) * Vector2.one;
        CheckPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.SetParent(FutureCardParentTransform);
        transform.SetSiblingIndex(SiblingIndex);
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
