using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using HuntWalker.Windows;
using HuntWalker.Managers;
using Microsoft.Extensions.DependencyInjection;
using ECommons;
using ECommons.Automation.NeoTaskManager;

namespace HuntWalker;

public sealed class Plugin : IDalamudPlugin
{
    //[PluginService] internal static IDalamudPluginInterface pluginInterface { get; private set; } = null!;
    public IDalamudPluginInterface pluginInterface;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    //[PluginService] internal static IChatGui chatGui { get; private set; } = null!;
    //[PluginService] internal static IPluginLog pluginLog { get; private set; } = null!;

    private const string CommandName = "/hw";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("SamplePlugin");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }

    public Plugin(IDalamudPluginInterface pluginInterface,
        IChatGui chatGui,
        IPluginLog pluginLog
        )
    {
        this.pluginInterface = pluginInterface;
        Dalamud.Initialize(pluginInterface);
        ECommonsMain.Init(pluginInterface, this);
        
        Configuration = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        // you might normally want to embed resources and load them from the manifest stream
        var goatImagePath = Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
        var serviceProvider = new ServiceCollection()
            .AddSingleton(pluginInterface)
            .AddSingleton(chatGui)
            .AddSingleton(pluginLog)
            .AddSingleton<Configuration>()
            .AddSingleton<MovementManager>()
            .AddSingleton<MainWindow>()
            .BuildServiceProvider();

        ConfigWindow = new ConfigWindow(this);

        MainWindow = serviceProvider.GetRequiredService<MainWindow>();

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });

        pluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        pluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        pluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just toggle the display status of our main ui
        ToggleMainUI();
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();
}

