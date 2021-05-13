using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class SpellManager : Manager
{
    private static readonly string FILE_PATH = "GameData/Spells";
    private Dictionary<string , Spell> allSpells;
    private Dictionary<string , Spell> playerSpells;
    private Dictionary<string , Spell> enemySpells;

    public override async Task Setup( IContext context )
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
            case SpellType.FIRE_BASIC_SPELL:
                return JsonUtility.FromJson<FireBasicSpell>( json );
            case SpellType.ICE_BASIC_SPELL:
                return JsonUtility.FromJson<IceBasicSpell>( json );
            case SpellType.LIGHTNING_BASIC_SPELL:
                return JsonUtility.FromJson<LightningBasicSpell>( json );
            case SpellType.JAB_SPELL:
                return JsonUtility.FromJson<JabSpell>( json );
            case SpellType.MASS_FEAR_SPELL:
                return JsonUtility.FromJson<MassFearSpell>( json );
            case SpellType.SLUMBER_SPELL:
                return JsonUtility.FromJson<SlumberSpell>( json );
        }

        return null;
    }

    public void LoadPlayerSpells( List<string> spells )
    {
        playerSpells.Clear();
        int spellCount = spells.Count;
        for(int spellIndex = 0; spellIndex < spellCount; spellIndex++ )
        {
            string spellName = spells[ spellIndex ];
            Spell spellToAdd = allSpells[ spellName ];
            spellToAdd.SetPlayerSpell();
            playerSpells.Add( spellName, spellToAdd );
        }
    }

    public void LoadEnemySpells( List<string> spells )
    {
        enemySpells.Clear();
        int spellCount = spells.Count;
        for( int spellIndex = 0; spellIndex < spellCount; spellIndex++ )
        {
            string spellName = spells[ spellIndex ];
            Spell spellToAdd = allSpells[ spellName ];
            spellToAdd.SetEnemySpell();
            enemySpells[ spellName ] = spellToAdd;
        }
    }

    public Spell GetPlayerSpell( string spellName )
    {
        if( playerSpells.ContainsKey( spellName ) )
        {
            return playerSpells[ spellName ];
        }
        else
        {
            Debug.LogError( string.Format( "SpellManager.GetPlayerSpell: Missing spell name {0} for player" , spellName ) );
            return null;
        }
    }

    public Spell GetEnemySpell( string spellName )
    {
        if( enemySpells.ContainsKey( spellName ) )
        {
            return enemySpells[ spellName ];
        }
        else
        {
            Debug.LogError( string.Format( "SpellManager.GetEnemySpell: Missing spell name {0} for enemy" , spellName ) );
            return null;
        }
    }

    public List<Spell> GetPlayerSpells()
    {
        return playerSpells.Values.ToList();
    }

    public List<Spell> GetCurrentEnemySpells()
    {
        return enemySpells.Values.ToList();
    }
}
