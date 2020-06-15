using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Enemy
{
    public EnemyType EnemyType;
    public int Health;
    public string Name;
    public List<string> SpellNames;
    public int BaseXP;
    public int BaseGold;
}
