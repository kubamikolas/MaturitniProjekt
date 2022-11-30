using UnityEngine;
using Mirror;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    Camera sceneCamera;

    private void Start()
    { 
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        { //Jsme LocalPlayer, ručíme scene cameru
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
    
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
    }

    
    
    
    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
    }

    void DisableComponents()
    {
         for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
    }
    // Když jsme zničeni/zabiti 
    private void OnDisable()
    {   //obnoví scene cameru
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

       GameManager.UnRegisterPlayer(transform.name);
    }
}
