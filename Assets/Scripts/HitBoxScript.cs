using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxScript : MonoBehaviour {
    private Attackable parent;

    private void Start()
    {
        //May want to change the "root" assumption at some point
        parent = transform.root.GetComponent<Attackable>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Attackable target = collision.gameObject.GetComponent<Attackable>();
        if (target)
            if (target.IsAnEnemy(parent.GetMyEnemies()))
                GetComponentInParent<CharacterStats>().AttackObject(collision.gameObject);

    }
}
