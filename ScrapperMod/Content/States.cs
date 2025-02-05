using Scrapper.Modules;
using Scrapper.SkillStates;
using Scrapper.SkillStates.Primary;
using Scrapper.SkillStates.Secondary;
using Scrapper.SkillStates.Special;
using Scrapper.SkillStates.Utility;

namespace Scrapper.Content
{
    public static class States
    {
        public static void Init()
        {
            ContentManagement.AddEntityState(typeof(ThrustCombo));

            ContentManagement.AddEntityState(typeof(QuickStep));
            ContentManagement.AddEntityState(typeof(QuickLunge));

            ContentManagement.AddEntityState(typeof(ThunderStep));
            ContentManagement.AddEntityState(typeof(ThunderLunge));

            ContentManagement.AddEntityState(typeof(ChargeSkewer));
            ContentManagement.AddEntityState(typeof(ChargeRiposte));

            ContentManagement.AddEntityState(typeof(ThrowPylon));

            ContentManagement.AddEntityState(typeof(Impale));
            ContentManagement.AddEntityState(typeof(MainState));
        }
    }
}
