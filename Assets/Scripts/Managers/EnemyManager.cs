using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class EnemyManager : Manager
{
    private static readonly string _enemyFilePath = "Enemies";

    public static System.Action<int> EnemyDealtDamage;

    private Enemy _currentEnemy;

    private List<Enemy> _dungeonEnemies;

    public override void Setup()
    {
        base.Setup();
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    public void DealDamage(int amount )
    {
        _currentEnemy.Health -= amount;
        EnemyDealtDamage( amount );
    }
}
