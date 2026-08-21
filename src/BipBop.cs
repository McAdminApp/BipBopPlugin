using BipBopPlugin.Utils;
using McAdminPlugins;
using McAdminPlugins.Yaml;

namespace BipBopPlugin;

public class BipBop(IServerPluginFiles files, IPluginPages pages) : IPlugin
{
    public static IServerPluginFiles? Files { get; private set; }
    
    private const string ConfigFile = "BipBop/config.yml";
    private static YamlDocument _config = null!;
    private static readonly List<PluginField> ConfigValues = [
        // Celebrations
        new("celebrations.chicken", "Chicken")
        {
            Description = "Spawn celebratory chicken on achievements",
            Kind = PluginFieldKind.Toggle,
            Group = "Celebrations"
        },
        new("celebrations.chicken_name", "Chicken Name")
        {
            Description = "Format the name of the chicken. Leave empty to give it no name at all.\n'%' serves as a placeholder for the player name",
            Kind = PluginFieldKind.Text,
            Group = "Celebrations"
        },
        new("celebrations.fireworks", "Fireworks")
        {
            Description = "Spawn fireworks on achievements",
            Kind = PluginFieldKind.Toggle,
            Group = "Celebrations"
        },
        new("celebrations.fireworks_flight", "Fireworks flight time")
        {
            Description = "How long should the fireworks flight time be?",
            Kind = PluginFieldKind.Number,
            Minimum = 0,
            Maximum = 2,
            Group = "Celebrations"
        },
        
        // Portals
        new("portals.wand_required", "Require wand")
        {
            Description = "Is the user required to craft a wand in order to set portals?",
            Kind = PluginFieldKind.Toggle,
            Value = "false",
            Group = "Portals"
        }
    ];
    
    public Task Load()
    {
        Files = files;

        CreatePages();
        return Task.CompletedTask;
    }

    private async Task LoadCurrentSettings(CancellationToken ct)
    {
        _config = await files.ReadYamlAsync(ConfigFile, ct);
        for (var i = 0; i < ConfigValues.Count; i++)
        {
            var setting = ConfigValues[i];
            var value = GetSettingValue(setting);

            ConfigValues[i] = setting with { Value = value };
        }
    }

    private string? GetSettingValue(PluginField setting)
    {
        string? value;
        switch (setting.Kind)
        {
            case PluginFieldKind.Number:
                value = _config.GetInt(setting.Key).ToString();
                break;
            case PluginFieldKind.Toggle:
                value = _config.GetBool(setting.Key).BooleanToString();
                break;
            default:
            case PluginFieldKind.Choice:
            case PluginFieldKind.Password:
            case PluginFieldKind.LongText:
            case PluginFieldKind.Text:
                value = _config.GetString(setting.Key);
                break;
        }
        
        return value;
    }

    private void CreatePages()
    {
        var sections = new List<PluginSection>
        {
            new PluginSettingsSection
            {
                Title = "Settings",
                Description = "Configuration values read from " + ConfigFile,
                LoadAsync = async ct =>
                {
                    await LoadCurrentSettings(ct);
                    return ConfigValues;
                },
                SaveAsync = async (changes, ct) =>
                {
                    try
                    {
                        await files.EditYamlAsync(ConfigFile, config =>
                        {
                            foreach(var (key, value) in changes)
                                config.Set(key, value);
                        }, ct);
                        
                        return PluginResult.Success("Saved " + changes.Count + " changes to " + ConfigFile);
                    }
                    catch (Exception ex)
                    {
                        return PluginResult.Failure("Something went wrong while trying to save values:\n" + ex.Message);
                    }
                }
            }
        };
        
        pages.AddPage(new PluginPage("bipbop-settings", "BipBop")
        {
            AdministratorOnly = false,
            Description = "Settings for BipBop",
            Sections = sections
        });
    }
}