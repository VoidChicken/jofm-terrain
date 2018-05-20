using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxScript : MonoBehaviour {
    private Attackable parent;

    private void Start()
    {
        //TODO: May want to change the "root" assumption at some point
        parent = transform.root.GetComponent<Attackable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(name + " triggered OnTriggerEnter with " + collision.name);
        Attackable target = collision.gameObject.GetComponent<Attackable>();
        if (target)
            if (parent.IsAnEnemy(target.tag))
            {
                Debug.LogFormat("{0} is attacking {1} with {2} dmg", parent.name, target.name, parent.GetComponent<CharacterStats>().damage.GetValue());
                GetComponentInParent<CharacterStats>().AttackObject(collision.gameObject);
            }
    }
}
