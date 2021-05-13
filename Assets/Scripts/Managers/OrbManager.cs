using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class OrbManager : Manager
{
    private Dictionary<string , Orb> _orbDictionary;
    private Dictionary<OrbType , List<string>> _orbTypeDictionary;

    private static readonly string ORB_DIRECTORY = "GameData/Orbs/";

    public override async Task Setup( IContext context )
    {
        _orbDictionary = new Dictionary<string , Orb>();
        _orbTypeDictionary = new Dictionary<OrbType , List<string>>();

        string[] fileNames = Directory.GetFiles( ORB_DIRECTORY );
        foreach( string fileName in fileNames )
        {
            LoadOrbs( fileName );
        }
    }

    private void LoadOrbs( string fileName )
    {
        string json = File.ReadAllText( fileName );
        Orb orb = JsonUtility.FromJson<Orb>( json );
        _orbDictionary.Add( orb.orb_name , orb );

        if( _orbTypeDictionary.ContainsKey( orb.type ) )
        {
            _orbTypeDictionary[ orb.type ].Add( orb.asset_name );
        }
        else
        {
            _orbTypeDictionary.Add( orb.type , new List<string>() { orb.asset_name } );
        }
    }

    public Orb GetOrbFromDictionary( string orbName )
    {
        Orb orb;
        if( _orbDictionary.TryGetValue( orbName, out orb ) )
        {
            return orb;
        }
        return null;
    }

    // return a list of orb_names matching the type starting with biggest power to lowest power
    public List<string> GetOrbNamesMatchingType( OrbType orbType )
    {
        List<string> orb_names = new List<string>();

        orb_names = _orbTypeDictionary[ orbType ];

        orb_names.Sort( delegate ( string orb1 , string orb2 )
        {
            Orb orbA = _orbDictionary[ orb1 ];
            Orb orbB = _orbDictionary[ orb2 ];
            return orbA.power.CompareTo( orbB.power );
        } );

        return orb_names;
    }
}
