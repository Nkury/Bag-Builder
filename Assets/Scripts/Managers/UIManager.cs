using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;


public class UIManager : Manager
{
    private readonly string CANVAS_NAME = "DefaultCanvas";

    private Canvas _canvas;
    
    public override async Task Setup( IContext context )
    {
        base.Setup( context );
        _canvas = GameObject.Find( CANVAS_NAME ).GetComponent<Canvas>();
    }

    public Canvas GetActiveCanvas()
    {
        return _canvas;
    }

    public void InstantiateViewOnDefaultCanvas( string assetName, Action<GameObject> callback )
    {
        context.AssetManager.LoadPrefabAsset( assetName, gameObj =>
        {
            if(gameObj == null )
            {
                Debug.LogError( string.Format( "{0} not found in Addressables" , assetName ) );                
            }

            GameObject returnObj = GameObject.Instantiate( gameObj , _canvas.transform );
            callback( returnObj );
        }
        );       
    }
}
