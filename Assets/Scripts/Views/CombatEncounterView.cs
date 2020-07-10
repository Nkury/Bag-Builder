using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatEncounterView : View
{
    [SerializeField]
    private Image _playerAvatar;

    [SerializeField]
    private Image _enemyAvatar;

    [SerializeField]
    private Slider _playerHealthSlider;

    [SerializeField]
    private TextMeshProUGUI _playerHealthText;

    [SerializeField]
    private Slider _playerManaSlider;

    [SerializeField]
    private TextMeshProUGUI _playerManaText;

    [SerializeField]
    private TextMeshProUGUI _playerSkullLimitText;

    [SerializeField]
    private TextMeshProUGUI _playerBagLimitText;

    [SerializeField]
    private Slider _enemyHealthSlider;

    [SerializeField]
    private TextMeshProUGUI _enemyHealthText;

    [SerializeField]
    private TextMeshProUGUI _enemyNameText;

    [SerializeField]
    private GameObject[] _playerDrawnOrbPositions;

    [SerializeField]
    private GameObject[] _enemyDrawnOrbPositions;

    [SerializeField]
    private GameObject[] _playerSpellPositions;

    [SerializeField]
    private Button _drawOrbButton;

    [SerializeField]
    private SpellView _spellViewPrefab;

    public override void InstantiateView( State state, Context context )
    {
        base.InstantiateView( state, context );
    }

    public void Setup( List<Spell> spells )
    {
        SetupPlayerSpells( spells );
    }

    private void SetupPlayerSpells( List<Spell> spells )
    {
        int numSpells = spells.Count;
        for(int spellIndex = 0; spellIndex < numSpells; spellIndex++ )
        {
            if( !spells[ spellIndex ].IsBasic ) // if it's not a basic spell, then display in spell area
            {
                SpellView spellView = GameObject.Instantiate<SpellView>( _spellViewPrefab , _playerSpellPositions[ spellIndex ].transform );
                spellView.Setup( spells[ spellIndex ] );
            }
        }
    }
}
