using System;

public enum OrbType
{
    FIRE,
    ICE,
    LIGHTNING,
    SKULL,
    STAB,
    REST
}

[Serializable]
public class Orb
{
    public int power;
    public int mana;
    public int weight; // for determining the weight table
    public OrbType type;

    public Orb( int _power , int _mana , OrbType _type , int _weight )
    {
        power = _power;
        mana = _mana;
        type = _type;
        weight = _weight;
    }
}