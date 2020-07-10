using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrbDraggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private RectTransform _rectTransform;

    public RectTransform RectTransform
    {
        get
        {
            return _rectTransform;
        }
    }

    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private float _alphaFadeValue;

    private OrbType _orbType;

    public OrbType OrbType
    {
        get
        {
            return _orbType;
        }
    }

    private Vector2 _originalAnchoredPosition;

    private Context _context;

    private OrbSlot _inOrbSlot; // for determining if orb is in a spell slot
    
    private void Awake()
    {
        _originalAnchoredPosition = _rectTransform.anchoredPosition;
        //_canvas = _context.UIManager.GetActiveCanvas();
    }

    public void Setup( Context context, OrbType orbType )
    {
        _context = context;
        _orbType = orbType;
    }

    public void OnBeginDrag( PointerEventData eventData )
    {
        Debug.Log( "OnBeginDrag" );

        if( _inOrbSlot )
        {
            _inOrbSlot.RemovedOrb();
            _inOrbSlot = null;
        }

        _canvasGroup.alpha = _alphaFadeValue;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag( PointerEventData eventData )
    {
        Debug.Log( "OnDrag" );
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        Debug.Log( "OnEndDrag" );

        if( _inOrbSlot == null )
        {
            _rectTransform.anchoredPosition = _originalAnchoredPosition;
        }

        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        Debug.Log( "OnPointerDown" );
    }

    public void OnDrop( PointerEventData eventData ) 
    {
        throw new System.NotImplementedException();
    }

    public void SetOrbSlot(OrbSlot orbSlot)
    {
        _inOrbSlot = orbSlot;
    }
}
