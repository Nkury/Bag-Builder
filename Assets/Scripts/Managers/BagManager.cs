using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BagManager : Manager
{
    private static readonly string DEFAULT_BAG_NAME = "GameData/Bag/Default_Bag.json";
    private static readonly string CURRENT_RUN_BAG_NAME = "GameData/Bag/Bag_Run" + PlayerPrefs.GetInt( PlayerPrefsKey.RUN_COUNT ) + ".json";

    private Bag _bag;

    private string _filePath;

    public override void Setup()
    {
        base.Setup ( );

        _filePath = ( File.Exists( CURRENT_RUN_BAG_NAME ) ) ? CURRENT_RUN_BAG_NAME : DEFAULT_BAG_NAME;
        SaveBag();
        LoadBag();
        _bag.DrawOrb();
    }

    private void LoadBag()
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
        if( !string.IsNullOrEmpty( _filePath ) )
        {
            SaveBag();
        }
    }

    public bool MeetsRequirements(OrbRequirements orbRequirements )
    {
        int numRequirements = orbRequirements.orbRequirements.Count;

        for(int reqIndex = 0; reqIndex < numRequirements; reqIndex++ )
        {
            Requirement req = orbRequirements.orbRequirements[ reqIndex ];
            if( _bag.DrawnQuantityTable[(int)req.orbType] < req.quantity )
            {
                return false;
            }
        }

        return true;
    }
}
