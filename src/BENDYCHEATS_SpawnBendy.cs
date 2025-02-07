using UnityEngine;
using System;
using System.Collections.Generic;

public class BENDYCHEATS_SpawnBendy
{
    public BENDYCHEATS cheats;
    private bool enabled = true;
    private KeyCode binding = KeyCode.B;

    public void SetUp(BENDYCHEATS _cheats)
    {
        cheats = _cheats;
        cheats.options.Add(new BENDYCHEATS.CheatOption(GetValue, Execute, Ticker));
    }

    public string GetValue() => "Spawn Bendy: " + (enabled ? "enabled" : "disabled") + "(B).";

    public void Execute() => enabled = !enabled;

    public void Ticker()
    {
        if(!enabled) return;
        if(!Input.GetKeyDown(binding)) return;

        cheats.bendyController.ForceSpawn();
    }
}
