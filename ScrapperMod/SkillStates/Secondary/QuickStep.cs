using EntityStates;

namespace Scrapper.SkillStates.Secondary
{
    public class QuickStep : BaseSideStep 
    {
        protected override EntityState GetNextStateAuthority() => new QuickLunge();
    }
}
