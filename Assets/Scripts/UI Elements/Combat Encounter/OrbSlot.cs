using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrbSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private RectTransform _rectTransform;

    private OrbType _orbType;

    private IContext _context;

    private bool _hasOrb;

    private SpellView _spellView;

    public bool HasOrb
    {
        get
        {
            return _hasOrb;
        }
    }

    public void OnDrop( PointerEventData eventData )
    {
        if( eventData.pointerDrag != null )
        {
            OrbDraggable orbDraggable = eventData.pointerDrag.GetComponent<OrbDraggable>();
            if( orbDraggable.OrbType == _orbType )
            {
                orbDraggable.SetOrbSlot( this );
                Vector2 pos = _rectTransform.position;
                //pos.y -= _rectTransform.rect.width / 4;
                //pos.x += _rectTransform.rect.height / 4;
                orbDraggable.RectTransform.position = pos;
                _hasOrb = true;
            }
        }
    }

    public void Setup( IContext context, OrbType orbType, SpellView spellView )
    {
        _orbType = orbType;
        _context = context;
        _spellView = spellView;
    }

    public void RemovedOrb()
    {
        _hasOrb = false;
    }
}
