using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Camera _mainCamera;
    private Vector3 _offset;
    private GameObject _mainCanvas;

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
        _mainCanvas = GameObject.Find("UI/MainCanvas");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offset = transform.position - _mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, _mainCamera.farClipPlane));

        CurrentCardParentTransform = transform.parent;
        FutureCardParentTransform = transform.parent;

        IsDraggable = ((CurrentCardParentTransform.GetComponent<DropField>().TypeField == TypeField.SELF_HAND) && (GameManager.Instance.IsPlayerTurn) && (!GameManager.Instance.IsChoosing) && (!GameManager.Instance.IsHandCardPlaying));

        if (!IsDraggable) return;

        GameManager.Instance.IsDrag = true;

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

        GameManager.Instance.IsDrag = false;

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

    public void EnemyMoveToField(Transform field)
    {
        if (field.childCount > 0)
            transform.DOMove(field.GetChild(field.childCount - 1).position + new Vector3(50, 0, 0), 0.5f);
        else transform.DOMove(field.position, 0.5f);
    }

    public void PlayerMoveToField(Transform field, Transform emptyHandCard)
    {
        transform.GetComponent<CardInfoScript>().IsAnimationCard = true;
        transform.position = emptyHandCard.transform.position;
        transform.DOMove(field.GetComponent<DropField>().EmptyTableCard.position, 0.5f);
    }

    public void MoveTopHierarchy()
    {
        transform.SetParent(_mainCanvas.transform);
        transform.SetAsLastSibling();
    }

    public void MoveBackHierarchy()
    {
        transform.SetParent(CurrentCardParentTransform);
        transform.SetSiblingIndex(SiblingIndex);
    }
}
