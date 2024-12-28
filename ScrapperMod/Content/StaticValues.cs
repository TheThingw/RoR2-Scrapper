using System;
using UnityEngine;

namespace Scrapper.Content
{
    public static class StaticValues
    {
        public static string GetName(ChildNames name) => Enum.GetName(typeof(ChildNames), name);

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
        public const string LAYER_BODY = "Body";
        public const string LAYER_IMPACT = "Impact";
        public const string LAYER_GESTURE = "Gesture, Override";
        public const string LAYER_FULLBODY = "FullBody, Override";
        public const string LAYER_AIM_PITCH = "AimPitch";
        public const string LAYER_AIM_YAW = "AimYaw";
        public const string LAYER_FLINCH = "Flinch";
        public const string LAYER_IDLE = "Idle, Additive";
        #endregion

        #region Parameters
        public const string PARAM_FORWARD_SPEED = "forwardSpeed";
        public const string PARAM_RIGHT_SPEED = "rightSpeed";
        public const string PARAM_UP_SPEED = "upSpeed";
        public const string PARAM_WALK_SPEED = "walkSpeed";
        public const string PARAM_IS_MOVING = "isMoving";
        public const string PARAM_IS_SPRINTING = "isSprinting";
        public const string PARAM_IS_GROUNDED = "isGrounded";
        public const string PARAM_IN_COMBAT = "inCombat";
        public const string PARAM_AIM_YAW = "aimYawCycle";
        public const string PARAM_AIM_PITCH = "aimPitchCycle";
        public const string PARAM_EMOTE_RATE = "Emote.playbackRate";
        public const string PARAM_SLASH_RATE = "Slash.playbackRate";
        public const string PARAM_THROW_RATE = "ThrowBomb.playbackRate";
        public const string PARAM_FLINCH_INDEX = "flinchIndex";
        #endregion

        public const string NAME = "Scrapper";
        #region Skill Values
        public const float OPPORTUNIST_DMG_MULT = 0.25f;
        #endregion

        #region Anim Control
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
    }
}