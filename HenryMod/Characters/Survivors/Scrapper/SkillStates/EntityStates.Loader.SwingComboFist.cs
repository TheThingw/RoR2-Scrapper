// RoR2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// EntityStates.Loader.SwingComboFist
using EntityStates;
using EntityStates.Loader;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

public class SwingComboFist : LoaderMeleeAttack, SteppedSkillDef.IStepSetter
{
	public int gauntlet;

	void SteppedSkillDef.IStepSetter.SetStep(int i)
	{
		this.gauntlet = i;
	}

	protected override void PlayAnimation()
	{
		string animationStateName = ((this.gauntlet == 0) ? "SwingFistRight" : "SwingFistLeft");
		float num = Mathf.Max(base.duration, 0.2f);
		base.PlayCrossfade("Gesture, Additive", animationStateName, "SwingFist.playbackRate", num, 0.1f);
		base.PlayCrossfade("Gesture, Override", animationStateName, "SwingFist.playbackRate", num, 0.1f);
	}

	protected override void BeginMeleeAttackEffect()
	{
		base.swingEffectMuzzleString = ((this.gauntlet == 0) ? "SwingRight" : "SwingLeft");
		base.BeginMeleeAttackEffect();
	}

	public override void OnSerialize(NetworkWriter writer)
	{
		base.OnSerialize(writer);
		writer.Write((byte)this.gauntlet);
	}

	public override void OnDeserialize(NetworkReader reader)
	{
		base.OnDeserialize(reader);
		this.gauntlet = reader.ReadByte();
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		return InterruptPriority.Skill;
	}
}
