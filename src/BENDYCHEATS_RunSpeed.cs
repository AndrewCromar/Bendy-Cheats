using UnityEngine;
using System;
using System.Collections.Generic;

public class BENDYCHEATS_RunSpeed
{
    public BENDYCHEATS cheats;
    private bool enabled = true;

    public void SetUp(BENDYCHEATS _cheats)
    {
        cheats = _cheats;
        cheats.options.Add(new BENDYCHEATS.CheatOption(GetValue, Execute, Ticker));
    }

    public string GetValue() => "Run Speed: " + (enabled ? "enabled" : "disabled") + ".(" + cheats.playerController.m_RunSpeed.ToString()  + ")";

    public void Execute() => enabled = !enabled;

    public void Ticker()
    {
        if(!enabled) return;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (!(scrollInput != 0f)) return;

        cheats.playerController.m_RunSpeed += scrollInput * 10f;
        cheats.playerController.m_RunSpeed = Mathf.Clamp(cheats.playerController.m_RunSpeed, 10f, 100f);
    }
}
