using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    Context context;

    private void Awake( )
    {
        DontDestroyOnLoad ( this );
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateContext();
    }

    private void CreateContext( )
    {
        context = new Context();
        PlayerManager playerManager = new PlayerManager();
        SpellManager spellManager = new SpellManager();
        BagManager bagManager = new BagManager();
        EnemyManager enemyManager = new EnemyManager();
        context.AddManagers( playerManager , spellManager , bagManager , enemyManager );
        context.SetupManagers();
    }

    public void OnApplicationQuit ( )
    {
        context.TeardownManagers();
        PlayerPrefs.Save();
    }
}
