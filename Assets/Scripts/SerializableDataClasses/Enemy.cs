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
    public int Health;
    public int Draw;
    public string Name;
    public string BagName;
    public List<string> SpellNames;
    public int BaseXP;
    public int BaseGold;

    public virtual void DrawOrb()
    {

    }
}
