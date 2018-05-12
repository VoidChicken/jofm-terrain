using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(NetworkIdentity))]
public class PlayerSetup : NetworkBehaviour
{
    CharacterStats stats;
    [SerializeField]
    Behaviour[] componentsToDisable;
    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
        }

        if (isClient && isLocalPlayer)
        {
            GameObject.Find("PlayerCam").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = gameObject.transform;
        }
        //May need to do localPlayer check
        //if (isLocalPlayer)
        //GameObject.FindObjectOfType<CameraScript>().player = transform.gameObject;
    }

    private void DisableComponents()
    {
        foreach (Behaviour component in componentsToDisable)
        {
            component.enabled = false;
        }
    }
}
