using UnityEngine;
using static UnityEngine.UI.Selectable;

namespace Scrapper.Content
{
    public static class StaticValues
    {
        public enum ChildNames
        {
            MainHurtbox,
            BodySling,
            Belt,
            Harness,
            ArmWireR,
            ArmWireL,
            ArmLight,
            SuitShoulderR,
            ConnectorR,
            SuitHand,
            WristWire,
            SuitShoulderL,
            Girder,
            Handle,
            Body,
            Scarf,
            Shoulders,
            Head,
            ExoSuit,
            Sling,
            Chestplate,
            Scarf1,
            SuitArmR,
            Kneepads,
            Scarf2,
            HandWire1,
            HandWire2
        }

        #region Layers
        public static string LAYER_BODY = "Body";
        public static string LAYER_IMPACT = "Impact";
        public static string LAYER_GESTURE = "Gesture, Override";
        public static string LAYER_FULLBODY = "FullBody, Override";
        public static string LAYER_AIM_PITCH = "AimPitch";
        public static string LAYER_AIM_YAW = "AimYaw";
        public static string LAYER_FLINCH = "Flinch";
        public static string LAYER_IDLE = "Idle, Additive";
        #endregion

        #region Parameters
        public static string PARAM_FORWARD_SPEED = "";
        public static string PARAM_RIGHT_SPEED = "";
        public static string PARAM_UP_SPEED = "";
        public static string PARAM_WALK_SPEED = "";
        public static string PARAM_IS_MOVING = "";
        public static string PARAM_IS_SPRINTING = "";
        public static string PARAM_IS_GROUNDED = "";
        public static string PARAM_IS_COMBAT = "";
        public static string PARAM_IS_BAZOOKA = "";
        public static string PARAM_AIM_YAW = "";
        public static string PARAM_AIM_PITCH = "";
        public static string PARAM_EMOTE_RATE = "";
        public static string PARAM_SLASH_RATE = "";
        public static string PARAM_SHOOT_RATE = "";
        public static string PARAM_THROW_RATE = "";
        public static string PARAM_FLINCH_INDEX = "";
        #endregion

        public const string NAME = "Scrapper";
        #region Skill Values
        public const float OPPORTUNIST_DMG_MULT = 0.25f;
        #endregion

        #region Anim Control
        public const string GESTURE_ADD = "Gesture, Additive";
        public const string GESTURE_OVER = "Gesture, Override";
        public const string FULLBODY_ADD = "Fullbody, Additive";
        public const string BODY_OVER = "Fullbody, Override";

        public const string EMPTY = "BufferEmpty";
        #endregion

        #region Movement
        // Jump
        public const string JUMP = NAME + "Jump";
        public const string ASCEND = NAME + "Ascend";
        public const string DESCEND = NAME + "Descend";
        public const string DESCEND_FAST = NAME + "DescendFast";

        // Idle
        public const string IDLE = NAME + "Idle";
        public const string IDLE_IN = NAME + "IdleIn";  // Transition between Running and idle
        public const string IDLE_LONG = NAME + "IdleLong";
        public const string IDLE_LONG_START = NAME + "IdleLongStart";  // Transition between Idle and Long Idle


        // Walk
        public const string WALK_F = NAME + "WalkF";
        public const string WALK_B = NAME + "WalkB";
        public const string WALK_L = NAME + "WalkL";
        public const string WALK_R = NAME + "WalkR";

        // Run
        public const string RUN = NAME + "RunF";
        public const string RUN_L = NAME + "RunL";
        public const string RUN_R = NAME + "RunR";

        // Lobby
        public const string SELECT = NAME + "Select"; // Lobby Intro
        public const string SELECT_IDLE = NAME + "SelectIdle"; // Lobby Idle

        // Spawn
        public const string SPAWN = NAME + "Spawn";     // Plays when you press any button to start playing, just like Acrid Wake up anim
        public const string SPAWN_IDLE = NAME + "spawnIdle"; // Similar to Acrid Sleeping Idle.
        #endregion

        #region Skill Anims
        //Primary
        public const string PRIMARY_1 = NAME + "Primary1";
        public const string PRIMARY_2 = NAME + "Primary2";

        // Default Secondary
        public const string DASH = NAME + "Dash"; // 1st half of secondary
        public const string DASH_F = NAME + "DashF"; // 1st half of secondary
        public const string DASH_B = NAME + "DashB";
        public const string DASH_L = NAME + "DashL";
        public const string DASH_R = NAME + "DashR";

        // On Kill Impale
        public const string IMPALE = NAME + "Impale"; // on kill impale, and for second half of secondary

        // Alt Secondary - Not Made Yet
        public const string BLAST = NAME + "Blast"; // Alt special Jump

        // Default Utility
        public const string STAB_START = NAME + "StabStart"; // Start charging
        public const string STAB_HOLD = NAME + "StabHold"; // Hold charge
        public const string STAB_END = NAME + "StabEnd"; // Release key

        // Alt Utility - Not Made Yet
        public const string BLOCK_START = NAME + "BlockStart"; // Start block
        public const string BLOCK_HOLD = NAME + "BlockHold"; // Hold block
        public const string BLOCK_END = NAME + "BlockEnd"; // On key release, or when held to long

        // Default Special
        public const string THROW = NAME + "Throw"; // Default special

        // Alt Special - Not Made Yet
        public const string AIM = NAME + "SpecAim"; // Aim for alt special
        public const string THROW2 = NAME + "SpecStab"; // Attack / primary replacement for Alt special
        #endregion

        #region Anim Params
        public const string DASH_RATE = "";
        public const string PRIMARY_RATE = "";
        public const string SECONDARY_RATE = "";
        public const string UTILITY_RATE = "";
        public const string SPECIAL_RATE = "";
        #endregion

        #region ChildLocator
        public const string PRIMARY1_MUZZLE = "";
        public const string PRIMARY2_MUZZLE = "";

        public const string MUZZLE = "";
        #endregion
    }
}