using UnityEngine;
using System;
using System.Collections.Generic;

public class BENDYCHEATS_DeleteObject
{
    public BENDYCHEATS cheats;
    private bool enabled = true;
    private KeyCode binding = KeyCode.R;

    public void SetUp(BENDYCHEATS _cheats)
    {
        cheats = _cheats;
        cheats.options.Add(new BENDYCHEATS.CheatOption(GetValue, Execute, Ticker));
    }

    public string GetValue() => "Delete Object: " + (enabled ? "enabled" : "disabled") + "(R).";

    public void Execute() => enabled = !enabled;

    public void Ticker()
    {
        if(!enabled) return;
        if(!Input.GetKeyDown(binding)) return;

        GameObject go = cheats.LookingAt();

        if(go == null) return;

        GameObject.Destroy(go);
    }
}
