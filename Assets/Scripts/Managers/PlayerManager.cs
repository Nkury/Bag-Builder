using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

public class PlayerManager : Manager
{
    #region DELEGATES
    public delegate void OrbProcessed( bool underSkullLimit);
    public delegate void PlayerDamage( int damageDealt );
    public delegate void PlayerDead();
    #endregion

    #region EVENTS
    public static event OrbProcessed OrbProcessedEvent;
    public static event PlayerDamage PlayerDealtDamageEvent;
    public static event PlayerDamage PlayerHealDamageEvent;
    public static event PlayerDead PlayerDeadEvent;
    #endregion

    private static readonly string _defaultFilePath = "GameData/Player/Default_Player.json";
    private static readonly string _activePlayerFilePath = Application.persistentDataPath + "Player/Active_Player.json"; // TODO

    private Player _player;

    private string _filePath;

    public override async Task Setup( IContext context )
    {
        base.Setup( context );

        _filePath = _defaultFilePath;
        LoadPlayer();
    }

    protected override void RegisterEvents()
    {
        BagManager.PlayerDrawOrbEvent += ProcessOrb;
    }

    public override void Teardown()
    {
        base.Teardown();
        //SavePlayer();
    }

    protected override void DeregisterEvents()
    {
        BagManager.PlayerDrawOrbEvent -= ProcessOrb;
    }

    public void LoadPlayer()
    {
        string json = File.ReadAllText( _filePath );
        _player = JsonUtility.FromJson<Player>( json );
        _player.Setup( context );
        context.SpellManager.LoadPlayerSpells( _player.Spells );
    }

    public void SavePlayer()
    {

    }

    public void ProcessOrb( Orb orb )
    {
        bool skullLimitPassed = _player.ProcessOrb( orb );
        OrbProcessedEvent?.Invoke( skullLimitPassed );
    }

    public void DealDamage( int damageDealt )
    {
        int clampedHealth = Mathf.Clamp( _player.Health - damageDealt , 0 , _player.HealthMax );
        _player.Health = clampedHealth;
        PlayerDealtDamageEvent?.Invoke( damageDealt );
        if( _player.Health <= 0 )
        {
            PlayerDeadEvent?.Invoke();
        }
    }

    public void HealDamage( int damageHealed )
    {
        int clampedHealth = Mathf.Clamp( _player.Health + damageHealed , 0 , _player.HealthMax );
        _player.Health = clampedHealth;
        PlayerHealDamageEvent?.Invoke( damageHealed );
    }
}
