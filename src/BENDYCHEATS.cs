using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BENDYCHEATS : MonoBehaviour
{
    public PlayerController playerController;
    public CH3BendyController bendyController;

    public bool renderGUI = true;

    public List<CheatOption> options = new List<CheatOption>();
    public int index;

    public string output;

    public void Start()
    {
        bendyController = FindObjectOfType<CH3BendyController>();
        output = "Bendy Controller: " + bendyController + output;

        new BENDYCHEATS_Teleport().SetUp(this);
        new BENDYCHEATS_RunSpeed().SetUp(this);
        new BENDYCHEATS_WalkSpeed().SetUp(this);
        new BENDYCHEATS_DeleteObject().SetUp(this);
        if(bendyController != null) new BENDYCHEATS_SpawnBendy().SetUp(this);
        new BENDYCHEATS_Summon().SetUp(this, KeyCode.I, "Ai_Bendy(Clone)");
        new BENDYCHEATS_Summon().SetUp(this, KeyCode.O, "Ai_Projectionist");
        new BENDYCHEATS_Summon().SetUp(this, KeyCode.P, "Ai_Boris");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals)) index--;
        if (Input.GetKeyDown(KeyCode.Minus)) index++;
        index = Mathf.Clamp(index, 0, options.Count - 1);

        if (Input.GetKeyDown(KeyCode.Backspace)) options[index].Execute();

        if(Input.GetKeyDown(KeyCode.F1)) renderGUI = !renderGUI;

        foreach (CheatOption option in options) option.Ticker();
    }

    public void OnGUI()
    {
        if (!renderGUI) return;

        string text = "BENDY CHEATS\n-------------------------";

        foreach (CheatOption option in options) text += "\n" + (options.IndexOf(option) == index ? "> " : "-- ") + option.GetValue();

        text += "\n-------------------------\nDebugging:";
        text += "\nCurrent Scene: '" + SceneManager.GetActiveScene().name + "'.";
        text += "\nLooking At: '" + LookingAt().name + "'.";

        // text += "\n-------------------------\nRoot GameObjects:\n";
        // foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects()) text += go.name + "\n";

        text += "\n-------------------------\nOutput:\n" + output;

        GUI.Label(new Rect(10, 10, 500, 500), text);
    }

    public GameObject FindObject(string name)
    {
        foreach (GameObject go in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            if (go.name == name) {
                string clipboard = go.transform.gameObject.name + "\n";
                Transform parent = go.transform.parent;
                while(parent != null)
                {
                    clipboard += parent.gameObject.name + "\n";
                    parent = parent.parent;
                }
                GUIUtility.systemCopyBuffer = clipboard;

                return go;
            }
        }

        return null;
    }

    public GameObject LookingAt()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100)) return hit.transform.gameObject;

        return null;
    }

    public class CheatOption
    {
        public Func<string> getValue;
        public Action execute;
        public Action ticker;

        public CheatOption(Func<string> getValue, Action execute, Action ticker)
        {
            this.getValue = getValue;
            this.execute = execute;
            this.ticker = ticker;
        }

        public string GetValue() => getValue.Invoke();
        public void Execute() => execute.Invoke();
        public void Ticker() => ticker.Invoke();
    }

    public string Dummy_GetValue() => "Dummy Option";
    public void Dummy_Execute() { }
    public void Dummy_Ticker() { }
}