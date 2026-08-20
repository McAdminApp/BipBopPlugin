using McAdminPlugins;

namespace BipBopPlugin;

public class BipBop(IServerPluginFiles files, IPluginPages pages) : IPlugin
{
    public static IServerPluginFiles? Files { get; private set; }
    
    public Task Load()
    {
        Files = files;
        
        pages.AddPage(new PluginPage("bipbop-settings", "BipBop")
        {
            AdministratorOnly = false,
            Description = "Settings for BipBop",
            Sections = [
                new PluginSettingsSection
                {
                    Title = "Settings",
                    Description = "Settings for the plugin",
                    LoadAsync = async ct => [ // TODO during load set current value from file
                        // Celebrations
                        new PluginField("celebration.chicken", "Chicken")
                        {
                            Description = "Spawn celebratory chicken on achievements",
                            Kind = PluginFieldKind.Toggle,
                            Value = "true",
                            Group = "Celebrations"
                        },
                        new PluginField("celebration.chicken_name", "Chicken Name")
                        {
                            Description = "Format the name of the chicken. Leave empty to give it no name at all.\n'%' serves as a placeholder for the player name",
                            Kind = PluginFieldKind.Text,
                            Value = "[PLAYER]'s partykyckling",
                            Group = "Celebrations"
                        },
                        new PluginField("celebration.fireworks", "Fireworks")
                        {
                            Description = "Spawn fireworks on achievements",
                            Kind = PluginFieldKind.Toggle,
                            Value = "true",
                            Group = "Celebrations"
                        },
                        new PluginField("celebration.fireworks_flight", "Fireworks flight time")
                        {
                            Description = "How long should the fireworks flight time be?",
                            Kind = PluginFieldKind.Number,
                            Value = "0",
                            Minimum = 0,
                            Maximum = 2,
                            Group = "Celebrations"
                        },
                        
                        // Portals
                        new PluginField("portals.wand_required", "Require wand")
                        {
                            Description = "Is the user required to craft a wand in order to set portals?",
                            Kind = PluginFieldKind.Toggle,
                            Value = "false",
                            Group = "Portals"
                        }
                    ],
                    SaveAsync = async (changes, ct) =>
                    {
                        // TODO write to correct files
                        return PluginResult.Success("Successfully saved!");
                    }
                }
            ]
        });
        
        return Task.CompletedTask;
    }
}