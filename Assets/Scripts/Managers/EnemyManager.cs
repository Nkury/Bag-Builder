using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class EnemyManager : Manager
{
    #region DELEGATES
    public delegate void EnemyDamage( int damageDealt );
    public delegate void EnemyDead();
    public delegate void PlaySpell( List<Requirement> orbRequirements );
    #endregion

    #region EVENTS
    public static event EnemyDamage EnemyDealtDamageEvent;
    public static event EnemyDamage EnemyHealDamageEvent;
    public static event EnemyDead EnemyDeadEvent;
    public static event PlaySpell PlayEnemySpellEvent;
    #endregion

    private static readonly string ENEMY_FILE_PATH = "GameData/Enemies/";

    public Enemy CurrentEnemy
    {
        get { return _currentEnemy; }
    }
    private Enemy _currentEnemy;

    private Dictionary<string, Enemy> _dungeonEnemies;

    public override async Task Setup( IContext context ) 
    {
        base.Setup( context );
        _dungeonEnemies = new Dictionary<string , Enemy>();

        string[] fileNames = Directory.GetFiles( ENEMY_FILE_PATH );

        foreach(string fileName in fileNames )
        {
            string json = File.ReadAllText( fileName );
            Enemy enemy = JsonUtility.FromJson<Enemy>( json );
            enemy = CreateEnemy( enemy.EnemyType , json );
            enemy.Setup( context );
            _dungeonEnemies.Add( enemy.Name , enemy );
        }

        // TODO: FOR TESTING PURPOSES REMOVE LATER
        SetCurrentEnemy( LocKey.IMP_ENEMY_NAME );
        context.SpellManager.LoadEnemySpells( _currentEnemy.SpellNames );
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    private Enemy CreateEnemy(EnemyType enemyType, string json)
    {
        switch( enemyType )
        {
            case EnemyType.IMP:
                return JsonUtility.FromJson<Imp>( json );
        }

        return null;
    }

    public void DealDamage( int amount )
    {
        int clampedHealth = Mathf.Clamp( _currentEnemy.Health - amount , 0 , _currentEnemy.MaxHealth );
        _currentEnemy.Health = clampedHealth;
        EnemyDealtDamageEvent?.Invoke( amount );
        if( _currentEnemy.Health <= 0 )
        {
            EnemyDeadEvent?.Invoke();
        }
    }

    public void HealDamage( int amount )
    {
        int clampedHealth = Mathf.Clamp( _currentEnemy.Health + amount , 0 , _currentEnemy.MaxHealth );
        _currentEnemy.Health = clampedHealth;
        EnemyHealDamageEvent?.Invoke( amount );
    }

    public void OverrideDrawUpdate( bool noDrawUpdate )
    {
        _currentEnemy.IgnoreDrawUpdate = noDrawUpdate;
    }

    public void ResetDraw()
    {

    }

    // TODO: WILL LISTEN TO EVENTS
    public void SetCurrentEnemy( string enemyName )
    {
        Enemy enemy;
        if( _dungeonEnemies.TryGetValue( enemyName, out enemy ) )
        {
            _currentEnemy = enemy;
            context.BagManager.SetupEnemyBagAtCombatStart();
        }
        else
        {
            Debug.LogError( string.Format( "Missing {0} from current dungeon enemy dictionary" , enemyName ) );
        }
    }

    public void UpdateEnemyDrawInfo( Orb orb )
    {
        _currentEnemy.UpdateDrawnOrbInfo( orb );
    }

    // returns a spell if it can cast one
    // otherwise, returns null
    public Spell AttackIfAble()
    {
        string intent = _currentEnemy.Intent;
        Spell intendedSpell = null;
        if( !string.IsNullOrEmpty( intent ) )
        {
            intendedSpell = context.SpellManager.GetEnemySpell( intent );
            if( context.BagManager.MeetsRequirementsForEnemy( intendedSpell ) )
            {
                intendedSpell.Effect();
                context.BagManager.RemoveOrbToEnemyBag( intendedSpell.OrbRequirements );
                PlayEnemySpellEvent?.Invoke( intendedSpell.OrbRequirements );
                return intendedSpell;
            }
        }

        return intendedSpell;
    }
}
