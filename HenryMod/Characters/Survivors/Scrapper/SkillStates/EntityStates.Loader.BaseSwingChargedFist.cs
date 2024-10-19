// RoR2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// EntityStates.Loader.BaseSwingChargedFist
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EntityStates;
using EntityStates.Loader;
using RoR2;
using UnityEngine;

public class BaseSwingChargedFist : LoaderMeleeAttack
{
	public float charge;

	[SerializeField]
	public float minLungeSpeed;

	[SerializeField]
	public float maxLungeSpeed;

	[SerializeField]
	public float minPunchForce;

	[SerializeField]
	public float maxPunchForce;

	[SerializeField]
	public float minDuration;

	[SerializeField]
	public float maxDuration;

	public static bool disableAirControlUntilCollision;

	public static float speedCoefficientOnExit;

	public static float velocityDamageCoefficient;

	protected Vector3 punchVelocity;

	private float bonusDamage;

	private static int ChargePunchStateHash = Animator.StringToHash("ChargePunch");

	private static int ChargePunchParamHash = Animator.StringToHash("ChargePunch.playbackRate");

	[CompilerGenerated]
	private static Action<BaseSwingChargedFist> m_onHitAuthorityGlobal;

	public float punchSpeed { get; private set; }

	public static event Action<BaseSwingChargedFist> onHitAuthorityGlobal
	{
		[CompilerGenerated]
		add
		{
			Action<BaseSwingChargedFist> action = BaseSwingChargedFist.m_onHitAuthorityGlobal;
			Action<BaseSwingChargedFist> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref BaseSwingChargedFist.m_onHitAuthorityGlobal, (Action<BaseSwingChargedFist>)Delegate.Combine(action2, value), action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<BaseSwingChargedFist> action = BaseSwingChargedFist.m_onHitAuthorityGlobal;
			Action<BaseSwingChargedFist> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref BaseSwingChargedFist.m_onHitAuthorityGlobal, (Action<BaseSwingChargedFist>)Delegate.Remove(action2, value), action2);
			}
			while ((object)action != action2);
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		if (base.isAuthority)
		{
			base.characterMotor.Motor.ForceUnground();
			base.characterMotor.disableAirControlUntilCollision |= BaseSwingChargedFist.disableAirControlUntilCollision;
			this.punchVelocity = BaseSwingChargedFist.CalculateLungeVelocity(base.characterMotor.velocity, base.GetAimRay().direction, this.charge, this.minLungeSpeed, this.maxLungeSpeed);
			base.characterMotor.velocity = this.punchVelocity;
			base.characterDirection.forward = base.characterMotor.velocity.normalized;
			this.punchSpeed = base.characterMotor.velocity.magnitude;
			this.bonusDamage = this.punchSpeed * (BaseSwingChargedFist.velocityDamageCoefficient * base.damageStat);
		}
	}

	protected override float CalcDuration()
	{
		return Mathf.Lerp(this.minDuration, this.maxDuration, this.charge);
	}

	protected override void PlayAnimation()
	{
		base.PlayAnimation();
		base.PlayAnimation("FullBody, Override", BaseSwingChargedFist.ChargePunchStateHash, BaseSwingChargedFist.ChargePunchParamHash, base.duration);
	}

	protected override void AuthorityFixedUpdate()
	{
		base.AuthorityFixedUpdate();
		if (!base.authorityInHitPause)
		{
			base.characterMotor.velocity = this.punchVelocity;
			base.characterDirection.forward = this.punchVelocity;
			base.characterBody.isSprinting = true;
		}
	}

	protected override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
	{
		base.AuthorityModifyOverlapAttack(overlapAttack);
		overlapAttack.damage = base.damageCoefficient * base.damageStat + this.bonusDamage;
		overlapAttack.forceVector = base.characterMotor.velocity + base.GetAimRay().direction * Mathf.Lerp(this.minPunchForce, this.maxPunchForce, this.charge);
		if (base.fixedAge + base.GetDeltaTime() >= base.duration)
		{
			HitBoxGroup hitBoxGroup = base.FindHitBoxGroup("PunchLollypop");
			if ((bool)hitBoxGroup)
			{
				base.hitBoxGroup = hitBoxGroup;
				overlapAttack.hitBoxGroup = hitBoxGroup;
			}
		}
	}

	protected override void OnMeleeHitAuthority()
	{
		base.OnMeleeHitAuthority();
		BaseSwingChargedFist.onHitAuthorityGlobal?.Invoke(this);
	}

	public override void OnExit()
	{
		base.OnExit();
		base.characterMotor.velocity *= BaseSwingChargedFist.speedCoefficientOnExit;
	}

	public static Vector3 CalculateLungeVelocity(Vector3 currentVelocity, Vector3 aimDirection, float charge, float minLungeSpeed, float maxLungeSpeed)
	{
		currentVelocity = ((Vector3.Dot(currentVelocity, aimDirection) < 0f) ? Vector3.zero : Vector3.Project(currentVelocity, aimDirection));
		return currentVelocity + aimDirection * Mathf.Lerp(minLungeSpeed, maxLungeSpeed, charge);
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		return InterruptPriority.PrioritySkill;
	}
}
