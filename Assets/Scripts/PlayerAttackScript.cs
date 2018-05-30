using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour {

    [SerializeField]
    private GameObject hitBox;
		
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(CycleHitbox());
        }
	}

    private IEnumerator CycleHitbox()
    {
        hitBox.SetActive(true);
        yield return new WaitForSeconds(.1f);
        hitBox.SetActive(false);
        yield return null;
    }
}
