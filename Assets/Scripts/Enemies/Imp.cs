using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
    public double ChanceForMassFear;
    public double DeltaToGetLessMassFears;
    public double DeltaToGetMoreMassFears;
    public float MinDrawSpeed;
    public float MaxDrawSpeed;

    private int drawCount = 0;
    private float _currentDrawSpeed;

    public override void Setup( IContext context )
    {
        base.Setup( context );
        _currentDrawSpeed = DefaultDrawSpeed;
    }

    public override bool CanDrawOrb()
    {
        //if( drawCount < Draw )
        //{
        //    return true;
        //}
        //else
        //{
        //    drawCount = 0;
        //    return false;
        //}
        return true;
    }

    public override float GetDrawSpeed()
    {     
        return _currentDrawSpeed;
    }
    
    public override void UpdateDrawnOrbInfo( Orb orb )
    {
        if( !IgnoreDrawUpdate )
        {
            drawCount++;
        }
        ModifyDrawSpeed( orb );
        base.UpdateDrawnOrbInfo( orb );
    }

    private void ModifyDrawSpeed( Orb orb )
    {
        if( orb.type == OrbType.FEAR )
        {
            _currentDrawSpeed++;
        } else if( orb.type == OrbType.REST )
        {
            _currentDrawSpeed--;
        }

        _currentDrawSpeed = Mathf.Clamp( _currentDrawSpeed , MinDrawSpeed , MaxDrawSpeed );
    }

    protected override void SetIntent( Orb orb )
    {
        if( Health < Mathf.FloorToInt( Health / 2 ) )
        {
            _intent = LocKey.IMP_SLUMBER_SPELL_NAME;
        }
        else
        {
            float chance = Random.value;
            if( chance < ChanceForMassFear )
            {
                _intent = LocKey.IMP_MASS_FEAR_SPELL_NAME;
                ChanceForMassFear -= DeltaToGetLessMassFears;
            }
            else
            {
                _intent = LocKey.IMP_JAB_SPELL_NAME;
                ChanceForMassFear += DeltaToGetMoreMassFears;
            }
        }
    }


}
