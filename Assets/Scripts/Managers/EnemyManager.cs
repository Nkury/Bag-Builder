using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemyManager : Manager
{
    private static readonly string ENEMY_FILE_PATH = "GameData/Enemies/";

    public static System.Action<int> EnemyDealtDamage;

    private Enemy _currentEnemy;

    private Dictionary<string, Enemy> _dungeonEnemies;

    public override void Setup( IContext context ) 
    {
        base.Setup( context );
        _dungeonEnemies = new Dictionary<string , Enemy>();

        string[] fileNames = Directory.GetFiles( ENEMY_FILE_PATH );

        foreach(string fileName in fileNames )
        {
            string json = File.ReadAllText( fileName );
            Enemy enemy = JsonUtility.FromJson<Enemy>( json );
            _dungeonEnemies.Add( enemy.Name , CreateEnemy( enemy.EnemyType , json ));
        }
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    public Enemy CreateEnemy(EnemyType enemyType, string json)
    {
        switch( enemyType )
        {
            case EnemyType.IMP:
                return JsonUtility.FromJson<Imp>( json );
        }

        return null;
    }

    public void DealDamage(int amount )
    {
        _currentEnemy.Health -= amount;
        EnemyDealtDamage( amount );
    }
}
