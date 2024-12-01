using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ImGuiNET;
using HuntWalker.Managers;
using Serilog;
using ECommons.Automation.NeoTaskManager;
using System.Threading;

namespace HuntWalker.Windows;

public class MainWindow : Window, IDisposable
{
    private Configuration config;
    private MovementManager movementManager;
    private IChatGui chat;
    private readonly IPluginLog log;
    private TaskManager userTasks;

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(
        Configuration config,
        MovementManager movementManager,
        IChatGui chat,
        IPluginLog log)
        : base("My Amazing Window##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.chat = chat;
        this.log = log;
        this.movementManager = movementManager;
        this.movementManager.OnMovementDone += HandleMovementDone;
        this.config = config;
        userTasks = new TaskManager(new TaskManagerConfiguration(600000, false, true, false, false, true, true));
        userTasks.StepMode = true;
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 430),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
    }

    private void HandleMovementDone(object? sender, EventArgs e)
    {
        log.Debug("MainWindow: Movement is done, queue next inputs.");
        userTasks.Step();
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text($"The random config bool is {config.SomePropertyToBeSavedAndWithADefault}");

        /*
        if (ImGui.Button("Show Settings"))
        {
            Plugin.ToggleConfigUI();
        }
        */

        if (ImGui.Button("Get Territory"))
        {
            chat.Print("I am in " + Dalamud.ClientState.TerritoryType);
        }

        if (ImGui.Button("Get Position"))
        {
            chat.Print("I am at " + Dalamud.ClientState.LocalPlayer.Position);
        }

        if (ImGui.Button("STOP ALL"))
        {
            chat.Print("Stopping...");
            movementManager.Stop();
            userTasks.Abort();
        }

        if (ImGui.Button("STOP THIS"))
        {
            chat.Print("Stopping...");
            movementManager.Stop();
        }

        if (ImGui.Button("Scout ShB"))
        {
            chat.Print("Queueing to scout all of ShB");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Lakeland!");
                movementManager.ScoutLakeland();
            });
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Kholusia!");
                movementManager.ScoutKholusia();
            });
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Ahm Ahreng!");
                movementManager.ScoutAhmAhreng();
            });
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Il Mheg!");
                movementManager.ScoutIlMheg();
            });
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting RakTika!");
                movementManager.ScoutRakTika();
            });
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Tempest!");
                movementManager.ScoutTempest();
            });
        }

        if (ImGui.Button("Scout Lakeland"))
        {
            chat.Print("Queueing to scout Lakeland");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Lakeland!");
                movementManager.ScoutLakeland(); 
            });
        }

        if (ImGui.Button("Scout Kholusia"))
        {
            chat.Print("Queueing to scout Kholusia");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Kholusia!");
                movementManager.ScoutKholusia();
            });
        }

        if (ImGui.Button("Scout Ahm Ahreng"))
        {
            chat.Print("Queueing to scout Ahm Ahreng");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Ahm Ahreng!");
                movementManager.ScoutAhmAhreng();
            });
            
        }

        if (ImGui.Button("Scout Il Mheg"))
        {
            chat.Print("Queueing to scout Il Mheg");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Il Mheg!");
                movementManager.ScoutIlMheg();
            });
        }

        if (ImGui.Button("Scout Rak'tika Greatwood"))
        {
            chat.Print("Queueing to scout The Rak'tika Greatwood");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting RakTika!");
                movementManager.ScoutRakTika();
            });
        }

        if (ImGui.Button("Scout Tempest"))
        {
            chat.Print("Queueing to scout Tempest");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Tempest!");
                movementManager.ScoutTempest();
            });
        }

        if (ImGui.Button("Scout Labyrinthos"))
        {
            chat.Print("Queueing to scout Laybrinthos");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Labyrinthos!");
                movementManager.ScoutLabyrinthos();
            });
            
        }

        if (ImGui.Button("Scout Thavnair"))
        {
            chat.Print("Queueing to scout Thavnair");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Thavnair!");
                movementManager.ScoutThavnair();
            });
        }

        if (ImGui.Button("Scout Garlemald"))
        {
            chat.Print("Queueing to scout Garlemald");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Garlemald!");
                movementManager.ScoutGarlemald();
            });
        }

        if (ImGui.Button("Scout Mare Lamentorum"))
        {
            chat.Print("Queueing to scout Mare Lamentorum");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Mare Lamentorum!");
                movementManager.ScoutMareLamentorum();
            });
        }

        if (ImGui.Button("Scout Ultima Thule"))
        {
            chat.Print("Queueing to scout Ultima Thule");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Ultima Thule!");
                movementManager.ScoutUltimaThule();
            });
        }

        if (ImGui.Button("Scout Elpis"))
        {
            chat.Print("Queueing to scout Elpis");
            userTasks.Enqueue(() => {
                chat.Print("TaskMgr: Starting Elpis!");
                movementManager.ScoutElpis();
            });
        }

        ImGui.Spacing();

        ImGui.Text("Have a goat:");
        /*
        var goatImage = Plugin.TextureProvider.GetFromFile(GoatImagePath).GetWrapOrDefault();
        if (goatImage != null)
        {
            ImGuiHelpers.ScaledIndent(55f);
            ImGui.Image(goatImage.ImGuiHandle, new Vector2(goatImage.Width, goatImage.Height));
            ImGuiHelpers.ScaledIndent(-55f);
        }
        else
        {
            ImGui.Text("Image not found.");
        }
        */
    }
}
