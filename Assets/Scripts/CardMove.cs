using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameManager GameManager;
    private Camera _mainCamera;
    private Vector3 _offset;

    public bool IsDraggable;
    public bool IsDrag;

    [HideInInspector] public int StartSiblingIndex;
    [HideInInspector] public int SiblingIndex;

    [HideInInspector] public Transform CurrentCardParentTransform;
    [HideInInspector] public Transform FutureCardParentTransform;

    [HideInInspector] public UnityEvent ChangeCardPosition;
    [HideInInspector] public UnityEvent HideEmptyCard;

    private void Awake()
    {
        _mainCamera = Camera.main;
        GameManager = FindObjectOfType<GameManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offset = transform.position - _mainCamera.ScreenToWorldPoint(eventData.position);

        CurrentCardParentTransform = transform.parent;
        FutureCardParentTransform = transform.parent;

        IsDraggable = ((CurrentCardParentTransform.GetComponent<DropField>().typeField == TypeField.SELF_HAND) && (GameManager.IsPlayerTurn));

        if (!IsDraggable) return;

        StartSiblingIndex = transform.GetSiblingIndex();
        transform.SetParent(CurrentCardParentTransform.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDraggable) return;
        IsDrag = true;

        transform.position = (_mainCamera.ScreenToWorldPoint(eventData.position) + _offset) * Vector2.one;
        CheckPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsDraggable) return;
        IsDrag = false;

        HideEmptyCard.Invoke();

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (!eventData.pointerCurrentRaycast.gameObject.GetComponent<DropField>())
        {
            transform.SetParent(CurrentCardParentTransform);
            transform.SetSiblingIndex(StartSiblingIndex);  
        }

        ChangeCardPosition.RemoveAllListeners();
        HideEmptyCard.RemoveAllListeners();
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

        SiblingIndex = newIndex;
        ChangeCardPosition.Invoke();
    }
}
