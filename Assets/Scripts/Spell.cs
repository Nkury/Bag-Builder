using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SpellType
{
    FIREBASICSPELL,
    ICEBASICSPELL,
    LIGHTNINGBASICSPELL
}

[Serializable]
public class Requirement
{
    public OrbType orbType;
    public int quantity;
}

[Serializable]
public class OrbRequirements
{
    public List<Requirement> orbRequirements;
}

[Serializable]
public class Spell
{
    private int _manaCost;
    private string _name;
    private SpellType _type;

    private OrbRequirements _orbRequirements;

    public virtual void Setup()
    {

    }

    public virtual void Effect()
    {

    }

    public virtual bool MeetsRequirements()
    {
        return Context.context.BagManager.MeetsRequirements( _orbRequirements );
    }

    public SpellType GetSpellType()
    {
        return _type;
    }
}
