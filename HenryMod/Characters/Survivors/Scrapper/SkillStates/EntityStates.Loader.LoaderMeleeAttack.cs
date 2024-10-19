// RoR2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// EntityStates.Loader.LoaderMeleeAttack
using EntityStates;
using EntityStates.Loader;
using RoR2;
using UnityEngine;

public class LoaderMeleeAttack : BasicMeleeAttack
{
	public static GameObject overchargeImpactEffectPrefab;

	public static float barrierPercentagePerHit;

	protected override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
	{
		base.AuthorityModifyOverlapAttack(overlapAttack);
		if (base.HasBuff(JunkContent.Buffs.LoaderOvercharged))
		{
			overlapAttack.damage *= 2f;
			overlapAttack.hitEffectPrefab = LoaderMeleeAttack.overchargeImpactEffectPrefab;
			overlapAttack.damageType |= (DamageTypeCombo)DamageType.Stun1s;
		}
	}

	protected override void OnMeleeHitAuthority()
	{
		base.OnMeleeHitAuthority();
		base.healthComponent.AddBarrierAuthority(LoaderMeleeAttack.barrierPercentagePerHit * base.healthComponent.fullBarrier);
	}
}
