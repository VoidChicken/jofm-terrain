using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {
    [SerializeField]
    public int baseValue;

    private List<int> modifiers = new List<int>();
    public int GetValue()
    {
        int finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(int mod)
    {
        if (mod != 0)
        {
            modifiers.Add(mod);
        }
    }

    public void RemoveModifier(int mod)
    {
        if(mod != 0)
        {
            modifiers.Remove(mod);
        }
    }
}
