using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Manager
{
    private readonly string CANVAS_NAME = "DefaultCanvas";

    private Canvas _canvas;

    private ViewGameObjectsScriptableObject _viewPrefabs;

    public UIManager( ViewGameObjectsScriptableObject viewPrefabs )
    {
        _viewPrefabs = viewPrefabs;
    }

    public override void Setup( IContext context )
    {
        base.Setup( context );
        _canvas = GameObject.Find( CANVAS_NAME ).GetComponent<Canvas>();
    }

    public Canvas GetActiveCanvas()
    {
        return _canvas;
    }

    public GameObject InstantiateViewOnDefaultCanvas( string viewName )
    {
        GameObject view = _viewPrefabs.GetPrefabFromName( viewName );

        if( view == null )
        {
            Debug.LogError( string.Format( "{0} not found in view prefabs scriptable object" , viewName ) );
            return null;
        }

        GameObject obj = GameObject.Instantiate<GameObject>( view , _canvas.transform );

        return obj;
    }
}
