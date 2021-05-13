using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class OrbDraggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{ 

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
    private Image _orbImage;

    [SerializeField]
    private float _alphaFadeValue;
    private Canvas _canvas;

    public OrbType OrbType { get; private set; } = OrbType.NONE;
    public string OrbName { get; private set; }

    private Vector2 _originalAnchoredPosition;

    private IContext _context;

    private OrbSlot _inOrbSlot; // for determining if orb is in a spell slot
    
    private void Awake()
    {
        _originalAnchoredPosition = _rectTransform.anchoredPosition;
        //_canvas = _context.UIManager.GetActiveCanvas();
    }

    public void Setup( IContext context )
    {
        _orbImage.gameObject.SetActive( false ); // set image to false until we load the sprite
        _context = context;
        _canvas = context.UIManager.GetActiveCanvas();
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

    private void LoadOrbImage( Orb orb )
    {
        _context.AssetManager.LoadSpriteAsset( orb.asset_name, sprite =>
        {
            if( sprite != null )
            {
                _orbImage.gameObject.SetActive( true );
                _orbImage.sprite = sprite;
            }
        } );        
    }

    public void SetOrb( Orb orb )
    {
        LoadOrbImage( orb );
        OrbType = orb.type;
        OrbName = orb.asset_name;
    }

    public void ClearOrbDraggable()
    {
        _orbImage.gameObject.SetActive( false );
        OrbType = OrbType.NONE;
        OrbName = string.Empty;
    }
}
