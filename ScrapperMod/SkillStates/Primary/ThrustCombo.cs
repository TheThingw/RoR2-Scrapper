using EntityStates;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace Scrapper.SkillStates.Primary
{
    public class SwingComboFist : BasicScrapperMeleeAttack, SteppedSkillDef.IStepSetter
    {
        public int gauntlet;

        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            gauntlet = i;
        }

        public override void PlayAnimation()
        {
            string animationStateName = gauntlet == 0 ? "SwingFistRight" : "SwingFistLeft";
            float num = Mathf.Max(duration, 0.2f);
            PlayCrossfade("Gesture, Additive", animationStateName, "SwingFist.playbackRate", num, 0.1f);
            PlayCrossfade("Gesture, Override", animationStateName, "SwingFist.playbackRate", num, 0.1f);
        }

        public override void BeginMeleeAttackEffect()
        {
            swingEffectMuzzleString = gauntlet == 0 ? "SwingRight" : "SwingLeft";
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
