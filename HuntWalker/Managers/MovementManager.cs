using Dalamud.Plugin.Ipc;
using Dalamud.Plugin.Ipc.Exceptions;
using System;
using System.Collections.Generic;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System.Numerics;
using FFXIVClientStructs.FFXIV.Client.Game;
using Dalamud.Game.ClientState.Conditions;
using HuntWalker.Models;
using ECommons.Automation.NeoTaskManager;
using Dalamud.Game.ClientState.Objects.Types;

namespace HuntWalker.Managers;

public class MovementManager : IDisposable {
    public List<Vector3> KholusiaWaypoints = new(){
        new Vector3((float) -219.9,(float) 476.7,(float) -734.2), // "17.1, 6.8"
        new Vector3((float) -95.7,(float) 428.2,(float) -547.3), // "19.5, 10.5"
        new Vector3((float) 14.9,(float) 366.9,(float) -379.7), // "21.8, 13.9"
        new Vector3((float) 164.8,(float) 342.5,(float) -504.4), // "24.8, 11.4"
        new Vector3((float) 494.5,(float) 302.2,(float) -100.0), // "31.4, 19.5"
        new Vector3((float) 256.1,(float) 326.7,(float) -115.2), // "26.7, 19.2"
        new Vector3((float) 77.4,(float) 351.1,(float) -201.3), // "23.0, 17.5"
        new Vector3((float) -1.4,(float) 332.0,(float) 40.8), // "21.2, 22.4"
        new Vector3((float) -335.5,(float) 379.0,(float) -295.9), // "14.8, 15.6"
        new Vector3((float) -494.4,(float) 367.6,(float) -144.9), // "11.6, 18.6"
        new Vector3((float) -627.0,(float) 79.7,(float) 200.5), // "8.9, 25.5"
        new Vector3((float) -326.9,(float) 65.6, (float) 133.9), // "14.9, 24.2"
        new Vector3((float) -31.7,(float) 32.2,(float) 484.4), // "20.9, 31.2"
        new Vector3((float) 159.8,(float) 34.8,(float) 419.2), // "24.7, 29.8"
        new Vector3((float) 266.7,(float) 47.9,(float) 143.6), // "26.8, 24.3"
        new Vector3((float) 646.5,(float) 58.4,(float) 131.7), // "34.4, 24.1"
        new Vector3((float) 423.4,(float) 26.8,(float) 420.3) // "30.0, 30.0"
    };

    public List<Vector3> NuckelaveeWaypoints = new()
    {

    };

    public List<Vector3> LakelandWaypoints = new() {
        new Vector3((float) -489.6,(float) 88.7,(float) -444.5), // "11.7, 12.6"
        new Vector3((float) -94.4,(float) 108.1,(float) -594.9), // "19.7, 9.7"
        new Vector3((float) 87.6,(float) 143.0,(float) -455.1), // "23.2, 12.3"
        new Vector3((float) 303.9,(float) 143.1,(float) -305.7), // "27.7, 15.3"
        new Vector3((float) 751.1,(float) 105.3,(float) -471.8), // "36.6, 12.2"
        new Vector3((float) 679.4,(float) 44.5,(float) -274.9), // "35.1, 16.0"
        new Vector3((float) 399.9,(float) 139.4,(float) -106.6), // "29.5, 19.3"
        new Vector3((float) 459.4,(float) 20.5,(float) 59.4), // "30.7, 22.6"
        new Vector3((float) 709.2,(float) 27.7,(float) 274.7), // "35.7, 27.0"
        new Vector3((float) 684.3,(float) 20.4,(float) 534.5), // "35.2, 32.2"
        new Vector3((float) 258.0,(float) 34.9,(float) 788.6), // "26.7 37.3"
        new Vector3((float) 322.0,(float) 20.8,(float) 463.8), // "27.9 30.8"
        new Vector3((float) 74.9,(float) -1.6,(float) 407.8), // "23.1, 29.7"
        new Vector3((float) 199.8,(float) 19.3,(float) 120.0), // "25.5, 23.9"
        new Vector3((float)-159.7,(float) 15.6,(float) 74.4), // "18.3, 23.0"
        new Vector3((float)-394.3,(float) 24.9,(float) 146.7), // "13.6, 24.4"
        new Vector3((float)-682.7,(float) 86.6,(float) 59.1), // "7.8, 22.6"
        new Vector3((float)-512.9,(float) 40.8,(float) -215.0) // "11.3, 17.2"
    };

    public List<Vector3> AhmAhrengWaypoints = new() {
        new Vector3((float) -499.5,(float) 45.0,(float) -105.0), // "11.5, 19.4"
        new Vector3((float) -567.4,(float) 16.9,(float) -475.1), // "10.2, 11.9"
        new Vector3((float) -239.9,(float) 61.8,(float) -564.4), // "16.7, 10.2"
        new Vector3((float) 54.8,(float) 71.1,(float) -549.5), // "22.6, 10.5"
        new Vector3((float) -108.5,(float) 47.7,(float) -269.1), // "19.3, 16.1"
        new Vector3((float) 352.9,(float) -8.3,(float) -458.0), // "28.6, 12.3"
        new Vector3((float) 470.4,(float) -18.2,(float) -391.9), // "30.9, 13.7"
        new Vector3((float) 364.7,(float) -2.9,(float) -60.1), // "28.8, 20.3"
        new Vector3((float) 599.8,(float) -12.3,(float) 2.3), // "33.5, 21.5"
        new Vector3((float) 348.0,(float) -5.7,(float) 227.0), // "28.5, 26.0"
        new Vector3((float) 574.5,(float) -87.7,(float) 620.5), // "33.0, 33.9"
        new Vector3((float) 444.5,(float) -63.8,(float) 688.2), // "30.4, 35.3"
        new Vector3((float) -83.1,(float) -37.4,(float) 743.4), // "19.9, 36.4"
        new Vector3((float) -214.6,(float) -36.3,(float) 504.2), // "17.2, 31.6"
        new Vector3((float) 82.1,(float) -83.6,(float) 410.5), // "23.1, 29.7"
        new Vector3((float) -82.2,(float) -27.1,(float) 364.5), // "19.8, 28.8"
        new Vector3((float) -114.8,(float) -14.5,(float) 170.0), // "19.2, 24.9"
        new Vector3((float) -257.3,(float) 32.7,(float) 124.2) // "16.3, 24.0"
    };
    public List<Vector3> IlMhegWaypoints = new() {
        new Vector3((float) -119.9,(float) 20.9,(float) 279.8), // "19.1, 27.1"
        new Vector3((float) 64.8,(float) 18.7,(float) 364.6), // "22.8, 28.8"
        new Vector3((float) 154.8,(float) 13.7,(float) 559.3), // "24.6, 32.7"
        new Vector3((float) 159.8,(float) 15.5,(float) 789.1), // "24.7, 37.7"
        new Vector3((float) -89.7,(float) 47.1,(float) 669.4), // "19.7, 34.9"
        new Vector3((float) -394.4,(float) 87.7,(float) 629.4), // "13.6, 34.1"
        new Vector3((float) -589.4,(float) 79.1,(float) 459.6), // "9.7 30.7"
        new Vector3((float) -683.7,(float) 56.4,(float) 271.5), // "7.8, 27.0"
        new Vector3((float) -699.3,(float) 50.8,(float) 75.1), // "7.5, 23.0"
        new Vector3((float) -534.6,(float) 29.4,(float) -59.9), // "10.8, 20.3"
        new Vector3((float) -515.0,(float) 55.6,(float) -284.4), // "11.2, 16.0"
        new Vector3((float) -67.4,(float) 73.9,(float) -649.3), // "20.2, 8.5"
        new Vector3((float) 199.6,(float) 99.3,(float) -719.2), // "25.5, 7.1"
        new Vector3((float) 389.5,(float) 110.0,(float) -804.2), // "29.3, 5.4"
        new Vector3((float) 614.3,(float) 123.8,(float) -709.3), // "33.8, 7.3"
        new Vector3((float) 504.6,(float) 110.5,(float) -394.7), // "31.6, 13.6"
        new Vector3((float) 286.8,(float) 59.5,(float) -126.6) // "27.2, 19.0"
    };

    public List<Vector3> RakTikaWaypoints = new()
    {
        new Vector3((float) -238.6,(float) 36.6,(float) 128.9), // "16.7, 24.0"
        new Vector3((float) -334.6,(float) 32.5,(float) 40.0), // "14.8, 22.3"
        new Vector3((float) -319.7,(float) 33.0,(float) 434.5), // "15.1, 30.2"
        new Vector3((float) -193.0,(float) 27.2,(float) 687.1), // "17.8, 35.1"
        new Vector3((float) -475.5,(float) 21.2,(float) 719.1), // "12.0, 35.9"
        new Vector3((float) -648.8,(float) 26.2,(float) 654.9), // "8.5, 34.6"
        new Vector3((float) -579.5,(float) 30.6,(float) 125.1), // "9.9, 24.0"
        new Vector3((float) -599.8,(float) 44.3,(float) -143.7), // "9.5, 18.6"
        new Vector3((float) 24.8,(float) 22.7,(float) -389.6), // "22.0, 13.7"
        new Vector3((float) 50.1,(float) 0.3,(float) -543.9), // "22.5, 10.7"
        new Vector3((float) 246.7,(float) 21.2,(float) -330.9), // "26.3, 14.9"
        new Vector3((float) 594.3,(float) 41.6,(float) 69.9), // "33.4, 22.9"
        new Vector3((float) 416.7,(float) 49.9,(float) 224.6), // "29.7, 26.0"
        new Vector3((float) 239.1,(float) 40.2,(float) 139.9), // "26.3, 24.3"
        new Vector3((float) 196.7,(float) 8.5,(float) 437.2) // "25.3, 30.4"
    };

    public List<Vector3> TempestWaypoints = new()
    {
        new Vector3((float) 460.1,(float) 399.0,(float) -527.6), // "30.7, 10.9"
        new Vector3((float) 755.6,(float) 436.8,(float) -494.0), // "36.7, 11.6"
        new Vector3((float) 811.8,(float) 422.3,(float) -244.1), // "37.7, 16.7"
        new Vector3((float) 729.3,(float) 417.9,(float) -75.1), // "36.1, 20.0"
        new Vector3((float) 609.5,(float) 402.8,(float) -0.0), // "33.7, 21.6"
        new Vector3((float) 384.1,(float) 361.2,(float) 64.9), // "29.1, 22.8"
        new Vector3((float) 159.4,(float) 301.6,(float) 163.5), // "24.7, 24.8"
        new Vector3((float) -310.2,(float) 297.4,(float) -54.8), // "15.4 20.4"
        new Vector3((float) -399.6,(float) 161.9,(float) -204.7), // "13.5, 17.4"
        new Vector3((float) -164.9,(float) 386.9,(float) -404.6), // "18.2, 13.4"
        new Vector3((float) -284.6,(float) 411.2,(float) -565.8), // "15.7, 10.3"
        new Vector3((float) -20.1,(float) 443.5,(float) -689.3), // "21.1, 7.7"
        new Vector3((float) 185.8,(float) 382.7,(float) -427.9), // "25.3, 13.0"
        new Vector3((float) 344.6,(float) 400.7,(float) -654.2), // "28.4, 8.4"
        new Vector3((float) 484.5,(float) 457.8,(float) -874.0), // "31.2, 4.0"
        new Vector3((float) -503.9,(float) 402.8,(float) -811.9), // "11.4, 5.2"
        new Vector3((float) -654.3,(float) 408.3,(float) -654.3) // "8.4, 8.4"
    };

    public List<Vector3> LabyrinthosWaypoints = new()
    {
        new Vector3((float) 439.2,(float) 201.0,(float) -659.5), // "30.2, 8.4"
        new Vector3((float) 639.9,(float) 200.0,(float) -399.7), // "34.3, 13.5"
        new Vector3((float) 532.8,(float) 104.6,(float) 221.6), // "32.1, 26.0"
        new Vector3((float) 176.3,(float) -4.0,(float) 194.8), // "25.2, 25.5"
        new Vector3((float) -89.5,(float) 10.1,(float) 840.0), // "19.6, 38.4"
        new Vector3((float) -469.2,(float) 5.6,(float) 690.9), // "12.0, 35.4"
        new Vector3((float) -790.1,(float) 10.0,(float) 616.8), // "5.8, 33.9"
        new Vector3((float) -541.3,(float) 6.6,(float) -119.9), // "10.6, 19.2"
        new Vector3((float) -246.9,(float) 13.9,(float) -256.2), // "16.5, 16.3"
        new Vector3((float) -224.0,(float) 110.9,(float) -602.0) // "17.1, 9.5"
    };

    public List<Vector3> ThavnairWaypoints = new()
    {
        new Vector3((float) -64.8,(float) 44.1,(float) 474.3), // "20.2 31.0"
        new Vector3((float) -153.4,(float) 51.7,(float) 85.9), // "18.3, 23.2"
        new Vector3((float) -164.8,(float) 102.1,(float) -255.9), // "18.2, 16.4"
        new Vector3((float) -349.5,(float) 92.5,(float) -464.4), // "14.5, 12.2"
        new Vector3((float) -141.9,(float) 104.6,(float) -496.7), // "18.6, 11.5"
        new Vector3((float) 400.2,(float) 27.2,(float) -394.4), // "29.5, 13.6"
        new Vector3((float) 564.1,(float) 34.4,(float) -59.9), // "32.8, 20.3"
        new Vector3((float) 254.0,(float) 46.2,(float) -28.6), // "26.5, 20.9"
        new Vector3((float) 308.5,(float) 30.5,(float) 214.7) // "27.7, 25.8"
    };

    public List<Vector3> GarlemaldWaypoints = new()
    {
        new Vector3((float) -407.7,(float) 45.8,(float) 479.6), // Do not stuck on houses around aetheryte
        new Vector3((float) 294.6,(float) 9.0,(float) 624.4), // "27.4, 34.0"
        new Vector3((float) 549.3,(float) 5.8,(float) 544.5), // "32.5, 32.4"
        new Vector3((float) 574.3,(float) 35.0,(float) 20.1), // "33.0, 21.9"
        new Vector3((float) 374.8,(float) 35.2,(float) -34.9), // "29.0, 20.8"
        new Vector3((float) 85.0,(float) 20.9,(float) 204.7), // "23.2, 25.6"
        new Vector3((float) -290.6,(float) 41.8,(float) -89.7), // "15.7, 19.7"
        new Vector3((float) -479.4,(float) 45.1,(float) -204.8), // "11.9, 17.4"
        new Vector3((float) -467.6,(float) 42.8,(float) -435.4), // "12.1, 12.7"
        new Vector3((float) -589.3,(float) 43.3,(float) -494.5) // "9.7, 11.6"
    };

    public List<Vector3> MareLamentorumWaypoints = new()
    {
        new Vector3((float) -254.2,(float) 73.1,(float) 364.4), // "19.7, 23.6"
        new Vector3((float) -559.4,(float) 159.5,(float) 129.9), // "10.3, 24.1"
        new Vector3((float) -214.4,(float) 102.5,(float) 162.7), // "17.2, 24.7"
        new Vector3((float) -154.9,(float) 94.6,(float) 0.2), // "18.4, 21.5"
        new Vector3((float) 138.5,(float) 77.7,(float) 83.7), // "24.3, 23.4"
        new Vector3((float) 344.0,(float) 83.7,(float) 254.7), // "28.3, 26.7"
        new Vector3((float) 750.5,(float) 160.9,(float) 289.8), // "36.4, 27.2"
        new Vector3((float) 413.0,(float) 122.4,(float) 412.0), // "29.8, 29.8"
        new Vector3((float) 135.0,(float) 89.3,(float) 599.7), // "24.2, 33.5"
        new Vector3((float) -19.9,(float) 94.5,(float) 659.3) // "21.1, 34.7"
    };

    public List<Vector3> UltimaThuleWaypoints = new()
    {
        new Vector3((float) -312.2,(float) 92.2,(float) 723.3), // "15.2, 36.0"
        new Vector3((float) -5.2,(float) 82.4,(float) 624.4), // "21.3, 34.0"
        new Vector3((float) -194.0,(float) 96.7,(float) 442.6), // "17.6, 30.4"
        new Vector3((float) -252.2,(float) 102.4,(float) 219.7), // "16.4, 25.9"
        new Vector3((float) -487.1,(float) 86.8,(float) 7.6), // "11.7, 21.7"
        new Vector3((float) -666.6,(float) 99.5,(float) -42.8), // "8.2, 20.6"
        new Vector3((float) -394.9,(float) 290.2,(float) -538.0), // "13.3, 10.5"
        new Vector3((float) -102.3,(float) 310.7,(float) -577.6), // "19.4, 10.0"
        new Vector3((float) 330.0,(float) 317.9,(float) -444.7) // "28.1, 12.6"
    };

    public List<Vector3> ElpisWaypoints = new()
    {
        new Vector3((float) -724.2,(float) -18.8,(float) 374.8), // "7.0, 29.0"
        new Vector3((float) -436.0,(float) -23.0,(float) 524.7), // "12.8, 32.1"
        new Vector3((float) -178.4,(float) -14.6,(float) 430.7), // "17.8, 30.1"
        new Vector3((float) -149.1,(float) 10.5,(float) 155.2), // "18.5, 24.5"
        new Vector3((float) 394.5,(float) 14.1,(float) 309.1), // "29.4, 27.7"
        new Vector3((float) 548.3,(float) 164.8,(float) -147.4), // "32.6, 18.6"
        new Vector3((float) 639.9,(float) 165.5,(float) -359.5), // "34.4, 14.3"
        new Vector3((float) 627.6,(float) 211.8,(float) -531.5), // "34.0, 10.8"
        new Vector3((float) -9.2,(float) 161.2,(float) -403.5), // "21.3, 13.5"
        new Vector3((float) 10.2,(float) 163.4,(float) -774.2), // "21.7, 6.0"
        new Vector3((float) -430.3,(float) 320.4,(float) -587.1) // "12.8, 9.8"
    };

    public List<uint> _ARankbNPCIds = new()
    {
        // Shadowbringers
        8906, // Nuckelavee
        8907, // Nariphon
        8911, // Li'l Murderer
        8912, // Huracan
        8901, // Maliktender
        8902, // Sugaar
        8654, // the mudman
        8655, // O poorest pauldia
        8891, // Supay
        8892, // Grassman
        8896, // Rusalka
        8897, // Baal
        // Endwalker
        10623, // Storsie
        10624, // Hulder
        10625, // Yilan
        10626, // Sugriva
        10627, // Minerva
        10628, // Aegeiros
        10629, // Lunatender queen
        10630, // Mousse princess
        10631, // Gurangatch
        10632, // Petalodus
        10633, // Fan ail
        10634 // Arch-eta
    };

    private readonly IChatGui chat;
    private readonly IPluginLog log;
    private readonly IObjectTable objectTable;
    private ushort targetTerritory = 0;
    private List<uint> marksFoundInArea = new();

    private TimeSpan _lastUpdate = new(0);
    private TimeSpan _execDelay = new(0, 0, 1);

    public event EventHandler OnMovementDone;

    private TaskManager movementTasks;

    public bool Available { get; private set; } = false;

    public bool CanAct
    {
        get
        {
            if (Dalamud.ClientState.LocalPlayer == null)
                return false;
            if (Dalamud.Conditions[ConditionFlag.BetweenAreas] ||
                Dalamud.Conditions[ConditionFlag.BetweenAreas51] ||
                Dalamud.Conditions[ConditionFlag.BeingMoved] ||
                Dalamud.Conditions[ConditionFlag.Casting] ||
                Dalamud.Conditions[ConditionFlag.Casting87] ||
                Dalamud.Conditions[ConditionFlag.Jumping] ||
                Dalamud.Conditions[ConditionFlag.Jumping61] ||
                Dalamud.Conditions[ConditionFlag.LoggingOut] ||
                Dalamud.Conditions[ConditionFlag.Occupied] ||
                Dalamud.Conditions[ConditionFlag.Unconscious] ||
                Dalamud.ClientState.LocalPlayer.CurrentHp < 1)
                return false;
            return true;
        }
    }


    public bool IsBusy => Lifestream_IPCSubscriber.IsBusy();
    public bool IsRunning => VNavmesh_IPCSubscriber.Path_IsRunning();
    public bool IsPathfinding => VNavmesh_IPCSubscriber.Nav_PathfindInProgress();
    public bool NavReady => VNavmesh_IPCSubscriber.Nav_IsReady();

    private readonly ICallGateSubscriber<TrainMob, bool> hhMarkSeen;


    // 813 -> Lakeland

    public MovementManager(
		IDalamudPluginInterface pluginInterface,
        IChatGui chat,
        IPluginLog log,
        IObjectTable objectTable
    ) {
        this.chat = chat;
		this.log = log;
        this.objectTable = objectTable;
		Available = true;
        movementTasks = new TaskManager(new TaskManagerConfiguration(600000, false, true, false, false, true, true));
        movementTasks.StepMode = true;

        log.Debug("------ Wow we are instanced!");
        Dalamud.Framework.Update += Tick;
    }
    public void OnMarkSeen(IBattleNpc mark)
    {
        log.Debug("We are seeing " + mark.Name + "("+ mark.NameId+ ")");
        if(movementTasks.NumQueuedTasks > 0 && _ARankbNPCIds.Contains(mark.NameId))
        {
            if(!marksFoundInArea.Contains(mark.NameId))
            {
                chat.Print("Adding "+mark.Name + "("+mark.NameId+") to the seen list.");
                marksFoundInArea.Add(mark.NameId);
                chat.Print(marksFoundInArea.Count + "/2 marks found in area.");
            } else
            {
                log.Debug("Mark already in list....");
            }
        }
    }
    private void Tick(IFramework framework)
    {
        _lastUpdate += framework.UpdateDelta;
        if (_lastUpdate > _execDelay)
        {
            DoUpdate(framework);
            _lastUpdate = new(0);
        }
    }

    private void CheckObjectTable()
    {
        if (Dalamud.ClientState.TerritoryType != targetTerritory)
            return;
        foreach (var obj in objectTable)
        {
            if (obj is not IBattleNpc mob) continue;
            var battlenpc = mob as IBattleNpc;
            if(_ARankbNPCIds.Contains(battlenpc.NameId))
            {
                OnMarkSeen(battlenpc);
            }
        }
    }
 
    private unsafe void DoUpdate(IFramework framework)
    {
        log.Debug(movementTasks.NumQueuedTasks+" Tasks left.");
        CheckObjectTable();
        if (movementTasks.NumQueuedTasks == 0)
        {
            log.Debug("DoUpdate! rdy " + NavReady + " !run " + !IsRunning + " !pathfind " + !IsPathfinding + " !busy " + !IsBusy + " canact " + CanAct);
            if (NavReady && !IsRunning && !IsPathfinding && !IsBusy && CanAct)
                OnMovementDone.Invoke(this, EventArgs.Empty);
            return;
        }

        log.Debug("DoUpdate! !rdy " + !NavReady + " run " + IsRunning + " pathfind " + IsPathfinding + " busy " + IsBusy + " !canact " + !CanAct);
        if (!NavReady || IsRunning || IsPathfinding || IsBusy || !CanAct)
            return;

        if (marksFoundInArea.Count > 1)
        {
            chat.Print("Found all marks in area. Stopping.");
            Stop();
            return;
        }
        
        log.Debug("Update run");
        log.Debug("We are in " + Dalamud.ClientState.TerritoryType + "tgt " + targetTerritory);
        if (Dalamud.ClientState.TerritoryType != targetTerritory)
        {
            log.Debug("We are not in the target territory, not pathing...");
            return;
        }
        movementTasks.Step();
        
    }

    public void Stop()
    {
        marksFoundInArea = new();
        movementTasks.Abort();
        VNavmesh_IPCSubscriber.Path_Stop();
    }

    private unsafe void TryMount()
    {
        var am = ActionManager.Instance();
        log.Debug("Are we mounted? " + Dalamud.Conditions[ConditionFlag.Mounted]);
        if (!Dalamud.Conditions[ConditionFlag.Mounted])
        {
            log.Debug("We are not mounted, mounting...");
            am->UseAction(ActionType.GeneralAction, 24);
            return;
        }
    }

    public void DEBUG()
    {
        Lifestream_IPCSubscriber.Teleport(136, 0);
    }

    public void ScoutLakeland()
    {
        targetTerritory = 813;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 813)
        {
            // The Ostall Imperative
            Lifestream_IPCSubscriber.Teleport(136, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in LakelandWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void ScoutKholusia()
    {
        targetTerritory = 814;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 814)
        {
            // Tomra
            Lifestream_IPCSubscriber.Teleport(139, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in KholusiaWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }
    public void ScoutAhmAhreng()
    {
        targetTerritory = 815;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 815)
        {
            // Twine
            Lifestream_IPCSubscriber.Teleport(141, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in AhmAhrengWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void ScoutIlMheg()
    {
        targetTerritory = 816;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 816)
        {
            // Lydha Lran
            Lifestream_IPCSubscriber.Teleport(144, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in IlMhegWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void ScoutRakTika()
    {
        targetTerritory = 817;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 817)
        {
            // Slitherbough
            Lifestream_IPCSubscriber.Teleport(142, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in RakTikaWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void ScoutTempest()
    {
        targetTerritory = 818;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 818)
        {
            // The Ondo Cups
            Lifestream_IPCSubscriber.Teleport(147, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in TempestWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void ScoutLabyrinthos()
    {
        targetTerritory = 956;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 956)
        {
            // The Archeion
            Lifestream_IPCSubscriber.Teleport(166, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in LabyrinthosWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void ScoutThavnair()
    {
        targetTerritory = 957;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 957)
        {
            // Yedlihmad
            Lifestream_IPCSubscriber.Teleport(169, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in ThavnairWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void ScoutGarlemald()
    {
        targetTerritory = 958;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 958)
        {
            // Camp Broken Glass
            Lifestream_IPCSubscriber.Teleport(172, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in GarlemaldWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void ScoutMareLamentorum()
    {
        targetTerritory = 959;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 959)
        {
            // Sinus Lacrimarum
            Lifestream_IPCSubscriber.Teleport(174, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in MareLamentorumWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void ScoutUltimaThule()
    {
        targetTerritory = 960;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 960)
        {
            // Reah Tahra
            Lifestream_IPCSubscriber.Teleport(179, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in UltimaThuleWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void ScoutElpis()
    {
        targetTerritory = 961;
        Stop();
        if (Dalamud.ClientState.TerritoryType != 961)
        {
            // The Twelve Wonders
            Lifestream_IPCSubscriber.Teleport(177, 0);
        }

        movementTasks.Enqueue(() => {
            TryMount();
        });

        foreach (var p in ElpisWaypoints)
        {
            movementTasks.Enqueue(() => {
                var res = VNavmesh_IPCSubscriber.SimpleMove_PathfindAndMoveTo(p, true);
                log.Debug("Pathfind was: " + res);
                if (!res)
                    chat.Print("WARNING: Pathfind was: " + res);
            });
        }
    }

    public void Dispose() {
        log.Debug("------ Wow we are disposed!");
        Dalamud.Framework.Update -= Tick;
    }

	private void OnDisable() {
		log.Info("VNavMesh IPC has been disabled. Disabling support.");
		Available = false;
	}
}
