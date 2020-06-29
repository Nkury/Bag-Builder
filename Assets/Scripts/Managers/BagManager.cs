using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BagManager : Manager
{
    private static readonly string DEFAULT_BAG_NAME = "GameData/Bag/Default_Bag.json";
    private static readonly string CURRENT_RUN_BAG_NAME = "GameData/Bag/Bag_Run" + PlayerPrefs.GetInt( PlayerPrefsKey.RUN_COUNT ) + ".json";
    private static readonly string BAG_DIRECTORY = "GameData/Bag/";

    private Bag _bag;

    private Dictionary<string , Bag> _enemyBags;

    private string _filePath;

    public override void Setup()
    {
        base.Setup ( );
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

    private void LoadEnemyBags(string fileName )
    {
        string json = File.ReadAllText( fileName );
        Bag bag = JsonUtility.FromJson<Bag>( json );
        bag.SetupWeightTable();
        _enemyBags.Add( bag.BagName, bag );
    }

    private void LoadPlayerBag()
    {
        string json = File.ReadAllText( _filePath );
        _bag = JsonUtility.FromJson<Bag>( json );
        _bag.SetupWeightTable();
    }

    private void SaveBag()
    {
        string json = JsonUtility.ToJson( _bag );
        File.WriteAllText( _filePath , json );
    }

    public override void Teardown()
    {
        // ADD BACK IN ONCE WE HAVE PERSISTENT DATA
      //  if( !string.IsNullOrEmpty( _filePath ) )
      //  {
      //      SaveBag();
      //  }
    }

    public bool MeetsRequirements(List<Requirement> orbRequirements )
    {
        int numRequirements = orbRequirements.Count;

        for(int reqIndex = 0; reqIndex < numRequirements; reqIndex++ )
        {
            Requirement req = orbRequirements[ reqIndex ];
            if( _bag.DrawnQuantityTable[(int)req.orbType] < req.quantity )
            {
                return false;
            }
        }

        return true;
    }
}
