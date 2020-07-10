using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SpellManager : Manager
{
    private static readonly string FILE_PATH = "GameData/Spells";
    private Dictionary<string , Spell> allSpells;
    private Dictionary<string , Spell> playerSpells;
    private Dictionary<string , Spell> enemySpells;

    public override void Setup ( IContext context )
    {
        base.Setup( context );
        allSpells = new Dictionary<string , Spell>();
        playerSpells = new Dictionary<string , Spell>();
        enemySpells = new Dictionary<string , Spell>();

        string[] files = Directory.GetFiles( FILE_PATH );

        for(int i = 0; i < files.Length; i++ )
        {
            string json = File.ReadAllText( files[ i ] );
            Spell spell = JsonUtility.FromJson<Spell>( json );
            spell = CreateSpell( spell.Type , json );
            spell.Setup( context );
            allSpells.Add( spell.Name , spell );
        }
    }

    private Spell CreateSpell(SpellType spellType, string json)
    {
        switch( spellType )
        {
            case SpellType.FIREBASICSPELL:
                return JsonUtility.FromJson<FireBasicSpell>( json );
            case SpellType.ICEBASICSPELL:
                return JsonUtility.FromJson<IceBasicSpell>( json );
            case SpellType.LIGHTNINGBASICSPELL:
                return JsonUtility.FromJson<LightningBasicSpell>( json );
        }

        return null;
    }

    public void LoadPlayerSpells( List<string> spells )
    {
        int spellCount = spells.Count;
        for(int spellIndex = 0; spellIndex < spellCount; spellIndex++ )
        {
            string spellName = spells[ spellIndex ];
            playerSpells.Add( spellName, allSpells[ spellName ] );
        }
    }

    public void LoadEnemySpells( List<string> spells )
    {
        int spellCount = spells.Count;
        for( int spellIndex = 0; spellIndex < spellCount; spellIndex++ )
        {
            string spellName = spells[ spellIndex ];
            enemySpells[ spellName ] = allSpells[ spellName ];
        }
    }

    public List<Spell> GetPlayerSpells()
    {
        return playerSpells.Values.ToList();
    }
}
