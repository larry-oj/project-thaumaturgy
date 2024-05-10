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
    }

    public static class Controls
    {
        public static class Player
        {
            public static string Up => "player_up";
            public static string Down => "player_down";
            public static string Left => "player_left";
            public static string Right => "player_right";
            public static string Attack => "player_attack";
            public static string Interact => "player_interact";
            public static string SwapActiveWeapon => "player_swap_weapon";
            public static string CraftWeapon => "player_craft_weapon";
            
        }
        
    }

    public static class Sizes
    {
        public static int TilesetSize => 16;
        public static int TilesetHalfsize => 8;
    }

    public static class Balance
    {
        public static int HealthPickupValueDefault => 6;
        public static int ManaPickupValueDefault => 6;
        public static int CurrencyPickupValueDefault => 1;
        public static int ElementUpgradeCost => 10;

        public static class StatusTypes
        {
            public static float BurningTickPeriod => 1.0f;
            public static int BurningTicksAmount => 4;
            public static float BurningDamage => 1.0f;
            public static float FreezingTickPeriod => 4.0f;
            public static int FreezingTicksAmount => 1;
            public static float FreezingDamage => 0f;
            public static float FreezingMultiplier => 0.5f;
            public static float StunnedTickPeriod => 1.0f;
            public static int StunnedTicksAmount => 1;
            public static float StunnedDamage => 0f;
            public static float KnockedBackForce => 50f;
        }
    }

    public static class AnimationNames
    {
        public static string Idle => "idle";
        public static string Run => "running";
        public static string Hurt => "hurting";
        public static string Dead => "dying";
        public static string Reset => "RESET";
    }
}
