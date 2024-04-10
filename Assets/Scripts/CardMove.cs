using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameManager GameManager;
    private Camera _mainCamera;
    private Vector3 _offset;

    public bool IsDraggable;

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
        _offset = transform.position - _mainCamera.ScreenToWorldPoint(new Vector3 (eventData.position.x, eventData.position.y, _mainCamera.farClipPlane));

        CurrentCardParentTransform = transform.parent;
        FutureCardParentTransform = transform.parent;

        IsDraggable = ((CurrentCardParentTransform.GetComponent<DropField>().TypeField == TypeField.SELF_HAND) && (GameManager.IsPlayerTurn) && (!GameManager.IsChoosing) && (!GameManager.IsSingleCardPlaying) );

        if (!IsDraggable) return;

        GameManager.IsDrag = true;

        StartSiblingIndex = transform.GetSiblingIndex();
        transform.SetParent(CurrentCardParentTransform.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDraggable) return;

        transform.position = (_mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, _mainCamera.farClipPlane)) + _offset);

        ChangePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsDraggable) return;

        GameManager.IsDrag = false;

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

    private void ChangePosition()
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

    public void MoveToField(Transform field)
    {
        transform.DOMove(field.position, 0.5f);
    }

    public void PlayerMoveToField(Transform field, Transform emptyHandCard)
    {
        transform.position = emptyHandCard.transform.position;
       // transform.SetParent(FutureCardParentTransform.parent);
        //transform.SetAsLastSibling();
        transform.DOMove(field.GetComponent<DropField>().EmptyTableCard.position, 0.5f);
      //  transform.SetParent(FutureCardParentTransform);
       // transform.SetSiblingIndex(SiblingIndex);
    }
}
