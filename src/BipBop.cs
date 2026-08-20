using McAdminPlugins;

namespace BipBopPlugin;

public class BipBop(IServerPluginFiles files, IPluginNavigation nav) : IPlugin
{
    public static IServerPluginFiles? Files { get; private set; }
    
    public Task Load()
    {
        Files = files;
        
        nav.AddPage(
            text: "BipBop",
            href: "bipbop",
            administratorOnly: false
        );
        
        return Task.CompletedTask;
    }
}