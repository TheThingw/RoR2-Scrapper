using System;

namespace Scrapper.Content
{
    public enum ChildLocatorEntry
    {
        MainHurtbox,
        StabHitbox,
        StabHitboxGroup,
        Body,
        Girder,
        Sword
    }

    public enum AnimatorStates
    {
        BufferEmpty,

        Throw,

        Primary1,
        Primary2,

        StabStart,
        StabHold,

        // fullbody
        StabEnd,
        Sidestep,
        Lunge,
        Impale
    }

    public enum AnimatorParams
    {
        Emote,
        Stab,
        Dash,
        Throw
    }

    internal static class StaticValues
    {
        public static string GetName<T>(this T val) where T : Enum
        {
            var name = Enum.GetName(typeof(T), val);
            
            if (typeof(T) == typeof(AnimatorParams))
                return name + ".playbackRate";

            return name;
        }

        #region Skill Values
        public const float OPPORTUNIST_DMG_MULT = 0.25f;
        #endregion

        // these animation names are all pre-defined
        // enums should be for scrapper specific stuff since those can change at any time

        #region Layers
        public const string LAYER_BODY = "Body";
        public const string LAYER_FULLBODY = "FullBody, Override";
        public const string LAYER_GESTURE = "Gesture, Override";

        public const string LAYER_AIM_PITCH = "AimPitch";
        public const string LAYER_AIM_YAW = "AimYaw";

        public const string LAYER_IMPACT = "Impact";
        public const string LAYER_FLINCH = "Flinch";
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
        public const string PARAM_FLINCH_INDEX = "flinchIndex";

        public const string PARAM_AIM_YAW = "aimYawCycle";
        public const string PARAM_AIM_PITCH = "aimPitchCycle";
        #endregion
    }
}