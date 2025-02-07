using UnityEngine;
using System;
using System.Collections.Generic;

public class BENDYCHEATS_Teleport
{
    public BENDYCHEATS cheats;
    private bool enabled = true;
    private KeyCode binding = KeyCode.G;
    private float distance = 10;

    public void SetUp(BENDYCHEATS _cheats)
    {
        cheats = _cheats;
        cheats.options.Add(new BENDYCHEATS.CheatOption(GetValue, Execute, Ticker));
    }

    public string GetValue() => "Teleport: " + (enabled ? "enabled" : "disabled") + "(G).";

    public void Execute() => enabled = !enabled;

    public void Ticker()
    {
        if(!enabled) return;
        if(!Input.GetKeyDown(binding)) return;

        Vector3 direction = Camera.main.transform.forward;
        Vector3 newPosition = cheats.playerController.gameObject.transform.position + direction * distance;
        cheats.playerController.gameObject.transform.position = newPosition;
    }
}
