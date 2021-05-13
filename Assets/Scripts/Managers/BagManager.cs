using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class BagManager : Manager
{
    #region DELEGATES
    public delegate void DrawOrb( Orb orb );
    #endregion

    #region EVENTS
    public static event DrawOrb PlayerDrawOrbEvent;
    public static event DrawOrb EnemyDrawOrbEvent;
    #endregion

    private static readonly string DEFAULT_BAG_NAME = "GameData/Bag/Default_Bag.json";
    private static readonly string CURRENT_RUN_BAG_NAME = "GameData/Bag/Bag_Run" + PlayerPrefs.GetInt( PlayerPrefsKey.RUN_COUNT ) + ".json";
    private static readonly string BAG_DIRECTORY = "GameData/Bag/";

    public Bag PlayerBag { get; private set; }
    public Bag CurrentEnemyBag { get; private set; }

    private Dictionary<string , Bag> _enemyBags;

    private string _filePath;

    public override async Task Setup( IContext context )
    {
        base.Setup ( context );
        _enemyBags = new Dictionary<string , Bag>();

        _filePath = ( File.Exists( CURRENT_RUN_BAG_NAME ) ) ? CURRENT_RUN_BAG_NAME : DEFAULT_BAG_NAME;

        LoadPlayerBag();

        string[] fileNames = Directory.GetFiles( BAG_DIRECTORY );
        foreach( string fileName in fileNames )
        {
            if( fileName != DEFAULT_BAG_NAME )
            {
                LoadEnemyBags( fileName );
            }
        }
    }

    protected override void RegisterEvents()
    {
        EnemyManager.PlayEnemySpellEvent += EnemyPlayedSpell;
    }

    #region LOAD_AND_SAVE_BAGS
    private void LoadEnemyBags(string fileName )
    {
        string json = File.ReadAllText( fileName );
        Bag bag = JsonUtility.FromJson<Bag>( json );
        bag.SetupOrbInfo( context );
        bag.SetupWeightTable();
        _enemyBags.Add( bag.BagName, bag );
    }

    private void LoadPlayerBag()
    {
        string json = File.ReadAllText( _filePath );
        PlayerBag = JsonUtility.FromJson<Bag>( json );
        PlayerBag.SetupOrbInfo( context );
        PlayerBag.SetupWeightTable();
    }

    private void SaveBag()
    {
        string json = JsonUtility.ToJson( PlayerBag );
        File.WriteAllText( _filePath , json );
    }
    #endregion  

    public override void Teardown()
    {
        base.Teardown();
        // ADD BACK IN ONCE WE HAVE PERSISTENT DATA
      //  if( !string.IsNullOrEmpty( _filePath ) )
      //  {
      //      SaveBag();
      //  }
    }

    protected override void DeregisterEvents()
    {
        EnemyManager.PlayEnemySpellEvent -= EnemyPlayedSpell;
    }

    #region MEET_REQUIREMENTS
    public bool MeetsRequirementsForPlayer( Spell spell )
    {
        if( spell.MeetsRequirements() )
        {
            return PlayerBag.BagMeetsRequirements( spell.OrbRequirements );
        }

        return false;
    }

    public bool MeetsRequirementsForEnemy( Spell spell )
    {
        if( spell.MeetsRequirements() )
        {
            return CurrentEnemyBag.BagMeetsRequirements( spell.OrbRequirements );
        }

        return false;
    }
    #endregion  

    // TODO: Will listen to events
    public void SetupEnemyBagAtCombatStart()
    {
        string enemyBagName = context.EnemyManager.CurrentEnemy.BagName;
        if( _enemyBags.ContainsKey( enemyBagName ) )
        {
           CurrentEnemyBag = _enemyBags[ enemyBagName ];
        }
        else
        {
            Debug.LogError( "Missing enemy bag for " + enemyBagName );
            return;
        }
    }

    public Orb DrawPlayerBag()
    {
        Orb drawnOrb = PlayerBag.DrawOrb();
        PlayerDrawOrbEvent?.Invoke( drawnOrb );
        return drawnOrb;
    }

    public Orb DrawEnemyBagAndAttack()
    {

        context.EnemyManager.AttackIfAble();

        Orb returnOrb = DrawEnemyBag();
        
        return returnOrb;
    }

    public Orb DrawEnemyBag()
    {
        Orb drawnOrb = null;
        drawnOrb = CurrentEnemyBag.DrawOrb();

        if( drawnOrb != null )
        {
            EnemyDrawOrbEvent?.Invoke( drawnOrb );
        }

        return drawnOrb;
    }

    // will remove all orbs in orb requirement
    public void RemoveOrbToEnemyBag( List<Requirement> orbRequirements )
    {
        for( int reqIndex = 0; reqIndex < orbRequirements.Count; reqIndex++ )
        {
            Requirement req = orbRequirements[ reqIndex ];
            RemoveOrbToEnemyBag( req.orbType , req.quantity );
        }
    }

    // will remove <param: quantity> of orbs matching <param: orbType> as much as it can
    public void RemoveOrbToEnemyBag(OrbType orbType, int quantity )
    {
        CurrentEnemyBag.RemoveDrawnOrbToBag( orbType, quantity );
    }

    public void ResetPlayerBag()
    {
        PlayerBag.ResetBag();   
    }

    public void ResetEnemyBag()
    {
        CurrentEnemyBag.ResetBag();
    }

    private void EnemyPlayedSpell( List<Requirement> orbRequirements )
    {
        for(int reqIndex = 0; reqIndex < orbRequirements.Count; reqIndex++ )
        {
            Requirement req = orbRequirements[ reqIndex ];
            CurrentEnemyBag.RemoveDrawnOrbToBag( req.orbType, req.quantity );
        }
    }

}
