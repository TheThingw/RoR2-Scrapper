using EntityStates;

namespace Scrapper.SkillStates.Secondary
{
    public class ThunderStep : BaseSideStep
    {
        protected override EntityState GetNextStateAuthority() => new ThunderLunge();
    }
}
