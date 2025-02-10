using EntityStates;

namespace Scrapper.SkillStates.Utility
{
    public class ChargeSkewer : BaseChargeStab
    {
        protected override EntityState GetNextStateAuthority()
        {
            return new Skewer
            {
                charge = this.charge
            };
        }
    }
}
