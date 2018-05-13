using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour {
    [TagSelector]
    public List<string> enemyTags = new List<string>();

    public bool IsAnEnemy(List<string> tags)
    {
        for(int i = 0; i < tags.Count; i++)
        {
            if (enemyTags.Contains(tags[i]))
                return true;
        }
        return false;
    }

    public List<string> GetMyEnemies()
    {
        return enemyTags;
    }
}
