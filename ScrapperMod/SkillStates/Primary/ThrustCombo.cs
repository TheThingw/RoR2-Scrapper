using EntityStates;
using RoR2.Skills;
using Scrapper.Content;
using UnityEngine.Networking;

namespace Scrapper.SkillStates.Primary
{
    public class ThrustCombo : BasicScrapperMeleeAttack, SteppedSkillDef.IStepSetter
    {
        public int gauntlet;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            gauntlet = i;
        }

        public override void PlayAnimation()
        {
            AnimatorStates animationState = gauntlet % 2 == 0 ? AnimatorStates.Primary1 : AnimatorStates.Primary2;
            //float num = Mathf.Max(duration, 0.2f);
            PlayAnimation(LAYER_GESTURE, animationState.GetName(), AnimatorParams.Stab.GetName(), duration);
        }

        public override void BeginMeleeAttackEffect()
        {
            //swingEffectMuzzleString = ChildLocatorEntry.Girder.GetName();
            base.BeginMeleeAttackEffect();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write((byte)gauntlet);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            gauntlet = reader.ReadByte();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
