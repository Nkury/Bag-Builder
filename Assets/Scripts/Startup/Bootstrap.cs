using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private Context context;
    
    private void Awake( )
    {
        DontDestroyOnLoad ( this );
    }

    // Start is called before the first frame update
    void Start()
    {
        Context.SetupCompleteEvent += OnSetupComplete;
        CreateContext();
    }

    private void CreateContext( )
    {
        context = new Context();
        PlayerManager playerManager = new PlayerManager();
        SpellManager spellManager = new SpellManager();
        OrbManager orbManager = new OrbManager();
        BagManager bagManager = new BagManager();
        EnemyManager enemyManager = new EnemyManager();
        UIManager uiManager = new UIManager();
        AssetManager assetManager = new AssetManager();
        StateMachine stateMachine = new StateMachine();
        LocalizationManager localizationManager = new LocalizationManager();
        context.AddManagers( assetManager, spellManager, orbManager, bagManager, playerManager, enemyManager, uiManager, stateMachine, localizationManager ); // order matters
        context.SetupManagers();
    }

    private void OnSetupComplete()
    {
        Debug.Log( "Manager setup complete" );
        Context.SetupCompleteEvent -= OnSetupComplete;
        
        context.StateMachine.PushState( new CombatState() );
    }

    public void OnApplicationQuit ( )
    {
        context.TeardownManagers();
        PlayerPrefs.Save();
    }
}
