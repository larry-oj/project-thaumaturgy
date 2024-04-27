namespace projectthaumaturgy.Scripts;

public static class Options
{
    public static class PathOptions
    {
        public static string CharacterStateToCharacter => "../..";
        public static string CharacterComponentToPivot => "../Pivot";
        public static string WeaponToCharacter => "../..";
        public static string WeaponStateToWeapon => "../..";
        public static string WeaponStateToCharacter => "../../../..";
        public static string Level => "/root/Game/Level";
        public static string GameManager => "/root/GameManager";
    }

    public static class Controls
    {
        public static string PlayerUp => "player_up";
        public static string PlayerDown => "player_down";
        public static string PlayerLeft => "player_left";
        public static string PlayerRight => "player_right";
        public static string PlayerAttack => "player_attack";
        public static string PlayerCraftWeapon => "player_craft_weapon";
    }

    public static class Sizes
    {
        public static int TilesetSize => 16;
        public static int TilesetHalfsize => 8;
    }
}
