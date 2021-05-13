public interface IContext
{
    OrbManager OrbManager { get; set; }
    BagManager BagManager { get; set; }
    PlayerManager PlayerManager { get; set; }
    SpellManager SpellManager { get; set; }
    EnemyManager EnemyManager { get; set; }
    UIManager UIManager { get; set; }
    StateMachine StateMachine { get; set; }
    LocalizationManager LocalizationManager { get; set; }
    AssetManager AssetManager { get; set; }
}
