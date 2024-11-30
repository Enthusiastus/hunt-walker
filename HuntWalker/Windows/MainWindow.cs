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

namespace HuntWalker.Windows;

public class MainWindow : Window, IDisposable
{
    private Configuration config;
    private MovementManager movementManager;
    private IChatGui chat;

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(
        Configuration config,
        MovementManager movementManager,
        IChatGui chat)
        : base("My Amazing Window##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.chat = chat;
        this.movementManager = movementManager;
        this.config = config;
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 430),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        chat.Print("can act? "+movementManager.CanAct);
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

        if (ImGui.Button("Scout Lakeland"))
        {
            chat.Print("Beginning to scout Lakeland");
            movementManager.ScoutLakeland();
        }

        if (ImGui.Button("Scout Kholusia"))
        {
            chat.Print("Beginning to scout Kholusia");
            movementManager.ScoutKholusia();
        }

        if (ImGui.Button("Scout Ahm Ahreng"))
        {
            chat.Print("Beginning to scout Ahm Ahreng");
            movementManager.ScoutAhmAhreng();
        }

        if (ImGui.Button("Scout Il Mheg"))
        {
            chat.Print("Beginning to scout Il Mheg");
            movementManager.ScoutIlMheg();
        }

        if (ImGui.Button("Scout Rak'tika Greatwood"))
        {
            chat.Print("Beginning to scout The Rak'tika Greatwood");
            movementManager.ScoutRakTika();
        }

        if (ImGui.Button("Scout Tempest"))
        {
            chat.Print("Beginning to scout Tempest");
            movementManager.ScoutTempest();
        }


        if (ImGui.Button("Get Territory"))
        {
            //log.Debug("I am in " + Dalamud.ClientState.TerritoryType);
        }

        if (ImGui.Button("Get Position"))
        {
            //log.Debug("I am at " + Dalamud.ClientState.LocalPlayer.Position);
        }

        if (ImGui.Button("STOP"))
        {
            chat.Print("Stopping...");
            movementManager.Stop();
        }

        if (ImGui.Button("Scout Labyrinthos"))
        {
            chat.Print("Beginning to scout Laybrinthos");
            movementManager.ScoutLabyrinthos();
        }

        if (ImGui.Button("Scout Thavnair"))
        {
            chat.Print("Beginning to scout Thavnair");
            movementManager.ScoutThavnair();
        }

        if (ImGui.Button("Scout Garlemald"))
        {
            chat.Print("Beginning to scout Garlemald");
            movementManager.ScoutGarlemald();
        }

        if (ImGui.Button("Scout Mare Lamentorum"))
        {
            chat.Print("Beginning to scout Mare Lamentorum");
            movementManager.ScoutMareLamentorum();
        }

        if (ImGui.Button("Scout Ultima Thule"))
        {
            chat.Print("Beginning to scout Ultima Thule");
            movementManager.ScoutUltimaThule();
        }

        if (ImGui.Button("Scout Elpis"))
        {
            chat.Print("Beginning to scout Elpis");
            movementManager.ScoutElpis();
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
