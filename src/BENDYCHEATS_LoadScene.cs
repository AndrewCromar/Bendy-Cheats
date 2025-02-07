using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BENDYCHEATS_LoadScene
{
    public BENDYCHEATS cheats;
    private bool enabled;
    private string name;

    public void SetUp(BENDYCHEATS _cheats, string _name)
    {
        cheats = _cheats;
        name = _name;
        cheats.options.Add(new BENDYCHEATS.CheatOption(GetValue, Execute, cheats.Dummy_Ticker));
    }

    public string GetValue() => "Load Scene " + name + ".";

    public void Execute() => SceneManager.LoadScene(name);
}
