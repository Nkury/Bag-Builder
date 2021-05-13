using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Threading.Tasks;

public class AssetManager : Manager
{
    private List<string> _addressableLabels = new List<string> // add new labels to this list so they can be loaded in the dictionaries
    {
        "sprite",
        "prefab"
    };

    private Dictionary<string , GameObject> _cachedPrefabDictionary;
    private Dictionary<string , Sprite> _cachedSpriteDictionary;
    private Dictionary<string , IResourceLocation> _cachedAddressableLocations;

    public override async Task Setup( IContext context )
    {
        base.Setup( context );
        _cachedSpriteDictionary = new Dictionary<string , Sprite>();
        _cachedPrefabDictionary = new Dictionary<string , GameObject>();
        _cachedAddressableLocations = new Dictionary<string , IResourceLocation>();

        Addressables.InitializeAsync();        
        await GetAllLocations();        
    }

    public override void Teardown()
    {
        base.Teardown();
    }

    private async Task GetAllLocations()
    {
        foreach( string label in _addressableLabels )
        {
            var unloadedLocations = await Addressables.LoadResourceLocationsAsync( label ).Task;

            foreach( var location in unloadedLocations )
            {
                Debug.Log( string.Format( "Adding {0} to locationdictionary" , location ) );
                if( !_cachedAddressableLocations.ContainsKey( location.PrimaryKey ) )
                {
                    _cachedAddressableLocations.Add( location.PrimaryKey , location );
                }
            }
        }
    }

    private void CacheSpriteInDictionary( string assetName, Action<Sprite> callback )
    {
        IResourceLocation location;
        if( _cachedAddressableLocations.TryGetValue( assetName , out location ) )
        {
            AsyncOperationHandle<UnityEngine.Object> asyncSprite = Addressables.LoadAssetAsync<UnityEngine.Object>( location );
            asyncSprite.Completed += handle =>
            {
                Texture2D text = handle.Result as Texture2D;
                Sprite sprite = Sprite.Create( text , new Rect(0, 0, text.width, text.height), new Vector2(0.5f, 0.5f));
                if( sprite == null )
                {
                    Debug.LogError( string.Format( "Unable to load {0} from {1} in AssetManager" , assetName , location ) );
                    return;
                }
                Debug.Log( "Loaded " + text.name );
                _cachedSpriteDictionary.Add( text.name , sprite );
                callback( sprite );
                Addressables.Release( asyncSprite );
            };          
        }
        else
        {
            Debug.LogError( string.Format( "Cannot find {0} in cachedAddressableLocations" , assetName ) );
        }         
    }

    private void CacheObjectInDictionary( string assetName, Action<GameObject> callback )
    {
        IResourceLocation location;
        if( _cachedAddressableLocations.TryGetValue( assetName , out location ) )
        {
            AsyncOperationHandle<GameObject> asyncObj = Addressables.LoadAssetAsync<GameObject>( location );
            asyncObj.Completed += handle =>
            {
                GameObject obj = handle.Result;
                if(obj == null )
                {
                    Debug.LogError( string.Format( "Unable to load {0} from {1} in AssetManager" , assetName , location ) );
                    return;
                }
                Debug.Log( "Loaded " + obj.name );
                _cachedPrefabDictionary.Add( obj.name , obj );
                callback( obj );
                Addressables.Release( asyncObj );
            };
        }
        else
        {
            Debug.LogError( string.Format( "Cannot find {0} in cachedAddressableLocations" , assetName ) );            
        }
    }

    public void LoadSpriteAsset( string assetName, Action<Sprite> callback )
    {
        if( _cachedSpriteDictionary.ContainsKey( assetName ) )
        {
            Sprite returnSprite = _cachedSpriteDictionary[ assetName ];
            callback( returnSprite );
        }
        else
        {
            Debug.Log( string.Format( "Missing sprite {0} from Addressables so caching it" , assetName ) );
            CacheSpriteInDictionary( assetName, callback );
        }
    }

    public void LoadPrefabAsset( string assetName, Action<GameObject> callback )
    { 
        if( _cachedPrefabDictionary.ContainsKey( assetName ) )
        {
            GameObject returnObj = _cachedPrefabDictionary[ assetName ];
            callback( returnObj );
        }
        else
        {
            Debug.Log( string.Format( "Missing prefab {0} from Addressables so caching it" , assetName ) );
            CacheObjectInDictionary( assetName, callback );
        }
    }
}
