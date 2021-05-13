using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnemyType
{
    IMP,
    GOBLIN,
    BAT,
    WORM_TERROR,
    WISPS,
    SLIME,
    HANDS,
    PIN_CUSHION,
    ABOMINATION
}

[Serializable]
public class Enemy
{
    public EnemyType EnemyType;
    public int MaxHealth;
    public int Health;
    public float DefaultDrawSpeed;
    public string Name;
    public string BagName;
    public List<string> SpellNames;
    public int BaseXP;
    public int BaseGold;

    [NonSerialized]
    protected IContext _context;

    [NonSerialized]
    protected string _intent; // intent is the name of the spell they're thinking about casting

    [NonSerialized]
    public bool IgnoreDrawUpdate;

    public string Intent
    {
        get
        {
            return _intent;
        }
    }

    public virtual void Setup( IContext context )
    {
        _context = context;
    }

    public virtual bool CanDrawOrb()
    {
        return false;
    }

    public virtual void UpdateDrawnOrbInfo( Orb orb )
    {
        SetIntent( orb );
    }
    
    public virtual void ProcessOrb( Orb orb )
    {

    }

    protected virtual void SetIntent( Orb orb )
    {

    }

    public virtual float GetDrawSpeed()
    {
        return DefaultDrawSpeed;
    }
}
