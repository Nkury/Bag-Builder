using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private Context context;

    [SerializeField]
    private ViewGameObjectsScriptableObject _viewPrefabs;

    private void Awake( )
    {
        DontDestroyOnLoad ( this );
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateContext();
        context.StateMachine.PushState( new CombatState() );
    }

    private void CreateContext( )
    {
        context = new Context();
        PlayerManager playerManager = new PlayerManager();
        SpellManager spellManager = new SpellManager();
        BagManager bagManager = new BagManager();
        EnemyManager enemyManager = new EnemyManager();
        UIManager uiManager = new UIManager( _viewPrefabs );
        StateMachine stateMachine = new StateMachine();
        LocalizationManager localizationManager = new LocalizationManager();
        context.AddManagers( spellManager, bagManager, playerManager, enemyManager, uiManager, stateMachine, localizationManager ); // order matters
        context.SetupManagers();
    }

    public void OnApplicationQuit ( )
    {
        context.TeardownManagers();
        PlayerPrefs.Save();
    }
}
