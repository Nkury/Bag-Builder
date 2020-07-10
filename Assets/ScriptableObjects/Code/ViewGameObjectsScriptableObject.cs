using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ViewPrefabToName
{
    public string name;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ViewObjects", order = 1)]
public class ViewGameObjectsScriptableObject : ScriptableObject
{
    public ViewPrefabToName[] ViewPrefabs;

    private Dictionary<string , GameObject> _viewPrefabDictionary;

    public void OnEnable()
    {
        _viewPrefabDictionary = new Dictionary<string , GameObject>();
        for( int i = 0; i < ViewPrefabs.Length; i++ )
        {
            _viewPrefabDictionary.Add( ViewPrefabs[ i ].name , ViewPrefabs[ i ].prefab );
        }
    }

    public GameObject GetPrefabFromName( string name )
    {
        if( _viewPrefabDictionary.ContainsKey( name ) )
        {
            return _viewPrefabDictionary[ name ];
        }

        return null;
    }
}
