using UnityEngine;
using System;
using System.IO;

public class PlayerManager : Manager
{
    private static readonly string _defaultFilePath = "GameData/Player/Default_Player.json";
    private static readonly string _activePlayerFilePath = Application.persistentDataPath + "Player/ActivePlayer.json"; // TODO

    private Player _player;

    private string _filePath;

    public override void Setup( IContext context )
    {
        base.Setup( context );

        _filePath = _defaultFilePath;
        LoadPlayer();
    }

    public override void Teardown()
    {
        base.Teardown();
        //SavePlayer();
    }

    public void LoadPlayer()
    {
        string json = File.ReadAllText( _filePath );
        _player = JsonUtility.FromJson<Player>( json );
        context.SpellManager.LoadPlayerSpells( _player.Spells );
    }

    public void SavePlayer()
    {

    }
}
