using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    [TagSelector]
    public List<string> enemyTags = new List<string>();

    public bool IsAnEnemy(string tag)
    {
        return enemyTags.Contains(tag);
    }

    public List<string> GetMyEnemies()
    {
        return enemyTags;
    }
}
