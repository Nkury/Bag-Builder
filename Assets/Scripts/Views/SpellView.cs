using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellView : View
{
    private static readonly int MAX_ORB_SLOTS = 5;

    [SerializeField]
    private GameObject[] _orbSlotPositions;

    [SerializeField]
    private TextMeshProUGUI _spellNameText;

    [SerializeField]
    private TextMeshProUGUI _spellDescriptionText;

    [SerializeField]
    private TextMeshProUGUI _spellManaCostText;

    [SerializeField]
    private OrbSlot _orbSlotPrefab;

    private List<OrbSlot> _orbSlots;

    private Spell _spell;

    public override void InstantiateView( IContext context )
    {
        base.InstantiateView( context );
    }

    public void Setup( Spell spell )
    {
        _spell = spell;
        _orbSlots = new List<OrbSlot>( new OrbSlot[MAX_ORB_SLOTS] );
        SetupOrbSlots();
        SetupSpellInfo();
    }

    private void SetupOrbSlots()
    {
        int numRequirements = _spell.OrbRequirements.Count;
        for(int reqIndex = 0; reqIndex < numRequirements; reqIndex++ )
        {
            Requirement req = _spell.OrbRequirements[ reqIndex ];
            OrbType orbType = req.orbType;
            for(int i = 0; i < req.quantity; i++ )
            {
                OrbSlot orbSlot = GameObject.Instantiate<OrbSlot>( _orbSlotPrefab , _orbSlotPositions[ reqIndex + i ].transform );
                orbSlot.Setup( _context , orbType, this );
                _orbSlots[ reqIndex + i ] = orbSlot;
            }
        }
    }

    private void SetupSpellInfo()
    {
        _spellNameText.text = _spell.GetLocalizedName();
        _spellDescriptionText.text = _spell.GetLocalizedDescription();
        _spellManaCostText.text = _spell.GetLocalizedManaCost();
    }

}
