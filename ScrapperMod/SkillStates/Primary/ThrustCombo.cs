
using EntityStates;
using RoR2.Skills;
using Scrapper.Content;
using UnityEngine;
using UnityEngine.Networking;

namespace Scrapper.SkillStates.Primary
{
    public class ThrustCombo : BasicScrapperMeleeAttack, SteppedSkillDef.IStepSetter
    {
        public int gauntlet;

        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            gauntlet = i;
        }

        public override void PlayAnimation()
        {
            string animationStateName = gauntlet == 0 ? StaticValues.PRIMARY_1 : StaticValues.PRIMARY_2;
            float num = Mathf.Max(duration, 0.2f);

            PlayCrossfade(StaticValues.LAYER_GESTURE, animationStateName, StaticValues.PARAM_SLASH_RATE, num, 0.1f);
            PlayCrossfade(StaticValues.LAYER_GESTURE, animationStateName, StaticValues.PARAM_SLASH_RATE, num, 0.1f);
        }

        public override void BeginMeleeAttackEffect()
        {
            swingEffectMuzzleString = StaticValues.GetName(StaticValues.ChildNames.Girder);
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
            return InterruptPriority.Skill;
        }
    }
}
