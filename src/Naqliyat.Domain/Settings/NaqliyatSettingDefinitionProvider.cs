using Volo.Abp.Settings;

namespace Naqliyat.Settings;

public class NaqliyatSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(NaqliyatSettings.MySetting1));
    }
}
