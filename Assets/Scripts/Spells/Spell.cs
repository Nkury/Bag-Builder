using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SpellType
{
    FIRE_BASIC_SPELL,
    ICE_BASIC_SPELL,
    LIGHTNING_BASIC_SPELL,
    JAB_SPELL,
    MASS_FEAR_SPELL,
    SLUMBER_SPELL
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
    public string Description;
    public bool IsBasic;

    public List<Requirement> OrbRequirements;

    protected IContext context;
    protected bool isEnemy;

    public virtual void Setup( IContext context )
    {
        this.context = context;
    }

    public virtual void Effect()
    {

    }

    // any additional requirements needed to satisfy casting a spell
    public virtual bool MeetsRequirements()
    {
        return true;
    }

    public virtual string GetLocalizedDescription()
    {
        return context.LocalizationManager.GetLocalizedString( Description );
    }

    public virtual string GetLocalizedName()
    {
        return context.LocalizationManager.GetLocalizedString( Name );
    }

    public virtual string GetLocalizedManaCost()
    {
        return string.Format(string.Format("{0} {1}", ManaCost.ToString(), context.LocalizationManager.GetLocalizedString( LocKey.MANA_RESOURCE_NAME )));
    }

    public virtual void SetEnemySpell()
    {
        isEnemy = true;
    }

    public virtual void SetPlayerSpell()
    {
        isEnemy = false;
    }
}
