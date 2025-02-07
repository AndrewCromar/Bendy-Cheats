using UnityEngine;
using System;
using System.Collections.Generic;

public class BENDYCHEATS_Summon
{
    public BENDYCHEATS cheats;
    private bool enabled = true;
    private KeyCode binding;
    private string name;
    private float distance = 10;

    public void SetUp(BENDYCHEATS _cheats, KeyCode _binding, string _name)
    {
        name = _name;
        binding = _binding;
        
        cheats = _cheats;
        cheats.options.Add(new BENDYCHEATS.CheatOption(GetValue, Execute, Ticker));
    }

    public string GetValue() => "Summon: '" + name + "', " + (enabled ? "enabled" : "disabled") + " (" + binding + ").";

    public void Execute() => enabled = !enabled;

    public void Ticker()
    {
        if(!enabled) return;
        if(!Input.GetKeyDown(binding)) return;

        GameObject go = cheats.FindObject(name);

        if(go == null) return;

        Vector3 direction = Camera.main.transform.forward;
        Vector3 newPosition = cheats.playerController.gameObject.transform.position + direction * distance;
        go.transform.position = newPosition;
    }
}
