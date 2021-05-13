using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

[Serializable]
public class LocStrings
{
    public List<LocString> Strings;
}

[Serializable]
public class LocString
{
    public string Key;
    public string Value;
}

public class LocalizationManager : Manager
{
    private static readonly string FILE_PATH = "GameData/Localization";
    private static readonly string FILE_NAME_PREFIX = "loc_";

    private static readonly string FILE_NAME_SUFFIX = "english"; // TODO: add functionality for multiple languages

    private Dictionary<string , string> _stringDictionary;
    public override async Task Setup( IContext context )
    {
        base.Setup( context );
        _stringDictionary = new Dictionary<string , string>();

        string[] files = Directory.GetFiles( FILE_PATH );

        for( int i = 0; i < files.Length; i++ )
        {
            if(files[ i ] == FILE_PATH + "\\" + FILE_NAME_PREFIX + FILE_NAME_SUFFIX + ".json" )
            {

                string json = File.ReadAllText( files[ i ] );
                LocStrings locStrings = JsonUtility.FromJson<LocStrings>( json );
                SetupStringDictionary( locStrings );
            }
        }
    }

    private void SetupStringDictionary( LocStrings locStrings )
    {
        for(int i = 0; i < locStrings.Strings.Count; i++ )
        {
            _stringDictionary.Add( locStrings.Strings[ i ].Key , locStrings.Strings[ i ].Value );
        }
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    public string GetLocalizedString( string key, params string[] args )
    {
        if( _stringDictionary.ContainsKey( key ) )
        {
            int numArgs = args.Length;
            string returnStr = _stringDictionary[ key ];
            for( int argIndex = 0; argIndex < numArgs; argIndex++ )
            {
                if(_stringDictionary[key].Contains("{" + argIndex + "}" ) )
                {
                    returnStr = returnStr.Replace( "{" + argIndex + "}" , args[ argIndex ] );
                }
                else
                {
                    Debug.LogError( string.Format("Localized string does not contain {0} argIndex." , argIndex) );
                    break;
                }
            }

            return returnStr;
        }
        else
        {
            Debug.LogError( "String is missing from localized string dictionary" );
            return null;
        }
    }
}
