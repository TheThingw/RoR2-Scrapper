using EntityStates;

namespace Scrapper.SkillStates.Utility
{
    public class ChargeRiposte : BaseChargeStab
    {
        protected override EntityState GetNextStateAuthority()
        {
            return new Reposte
            {
                charge = this.charge
            };
        }
    }
}
