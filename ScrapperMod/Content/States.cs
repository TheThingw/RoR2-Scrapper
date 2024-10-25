using Scrapper.Modules.BaseContent.BaseStates.oLD;

namespace Scrapper.Content
{
    public static class States
    {
        public static void Init()
        {
            Scrapper.Modules.Content.AddEntityState(typeof(SlashCombo));

            Scrapper.Modules.Content.AddEntityState(typeof(Shoot));

            Scrapper.Modules.Content.AddEntityState(typeof(Roll));

            Scrapper.Modules.Content.AddEntityState(typeof(ThrowBomb));
        }
    }
}
