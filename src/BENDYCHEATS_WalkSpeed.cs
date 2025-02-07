using UnityEngine;
using System;
using System.Collections.Generic;

public class BENDYCHEATS_WalkSpeed
{
    public BENDYCHEATS cheats;
    private bool enabled;

    public void SetUp(BENDYCHEATS _cheats)
    {
        cheats = _cheats;
        cheats.options.Add(new BENDYCHEATS.CheatOption(GetValue, Execute, Ticker));
    }

    public string GetValue() => "Walk Speed: " + (enabled ? "enabled" : "disabled") + ".(" + cheats.playerController.m_MoveSpeed.ToString()  + ")";

    public void Execute() => enabled = !enabled;

    public void Ticker()
    {
        if(!enabled) return;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (!(scrollInput != 0f)) return;

        cheats.playerController.m_MoveSpeed += scrollInput * 10f;
        cheats.playerController.m_MoveSpeed = Mathf.Clamp(cheats.playerController.m_MoveSpeed, 10f, 100f);
    }
}
