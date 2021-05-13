using System;
using System.Collections.Generic;

[Serializable]
public class LevelUp
{
    public int ExperienceThreshold;
    public int Level;
    public string Title;
}

[Serializable]
public class Player
{
    public int Health;
    public int HealthMax;
    public int Mana;
    public int ManaMax;
    public int CurrentSkull;
    public int SkullCap;
    public float OverloadMultiplier;
    public float OverloadLimit;
    public int Experience;
    public int Level;
    public int DamageModifier;
    public int DefenseModifier;
    public int AccessorySlots;
    public List<string> Spells = new List<string>(); // names of the spells the player has
    public string Title;
    public List<LevelUp> LevelUpChart = new List<LevelUp>();

    private IContext _context;

    public void Setup( IContext context )
    {
        _context = context;
    }

    // Returns true if skull limit has been reached.
    public bool ProcessOrb( Orb orb )
    {
        if(orb.type == OrbType.SKULL )
        {
            CurrentSkull += orb.power;
        }

        if( CurrentSkull >= SkullCap )
        {
            CurrentSkull = 0;
            _context.BagManager.ResetPlayerBag();
            return true;
        }

        return false;
    }
}
