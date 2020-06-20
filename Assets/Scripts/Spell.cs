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
public class Spell
{
    public int ManaCost;
    public string Name;
    public SpellType Type;

    public List<Requirement> OrbRequirements;

    public virtual void Setup()
    {

    }

    public virtual void Effect()
    {

    }

    public virtual bool MeetsRequirements()
    {
        return Context.context.BagManager.MeetsRequirements( OrbRequirements );
    }
}
