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
            Modules.Content.AddEntityState(typeof(ThrustCombo));

            Modules.Content.AddEntityState(typeof(QuickStep));
            Modules.Content.AddEntityState(typeof(QuickLunge));

            Modules.Content.AddEntityState(typeof(ThunderStep));
            Modules.Content.AddEntityState(typeof(ThunderLunge));

            Modules.Content.AddEntityState(typeof(ChargeSkewer));
            Modules.Content.AddEntityState(typeof(ChargeRiposte));

            Modules.Content.AddEntityState(typeof(ThrowPylon));

            Modules.Content.AddEntityState(typeof(Impale));
            Modules.Content.AddEntityState(typeof(MainState));
        }
    }
}
