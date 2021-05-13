using System;
using UnityEditor;
public enum OrbType
{
    NONE,
    FIRE,
    ICE,
    LIGHTNING,
    SKULL,
    FEAR,
    REST
}

[Serializable]
public class Orb : IComparable
{
    public string orb_name;
    public string asset_name;
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

    public int CompareTo( object obj )
    {
        if( obj != null && obj is Orb )
        {
            Orb orb = obj as Orb;
            return orb_name.CompareTo( orb.orb_name );
        }
        else
        {
            return 1;
        }
    }

    public static bool operator ==(Orb lOrb, Orb rOrb )
    {
        if( lOrb.orb_name == rOrb.orb_name && 
            lOrb.type == rOrb.type &&
            lOrb.power == rOrb.power )
        {
            return true;
        }

        return false;
    }

    public static bool operator !=( Orb lOrb , Orb rOrb ) => !( lOrb == rOrb );
}