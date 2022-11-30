using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";
    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("No camera");
            this.enabled = false;
        }
    }
    // Pokud je nějaký input, vytřelíme, je to pouze na LocalPlayera, který střílí, vytřelíme a pokud to trení neboli hitne hráče, pokud ano, podle id nám bude řečeno, kdo byl střelen.
    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    private void Shoot() // Pokud něco trefíme, co jsme trefili, poškození/damage
    {
        RaycastHit _hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            if(_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, weapon.damage);
            }
        }
    }

    [Command]
    private void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " Killed ");

        Player _player = GameManager.GetPlayer(_playerID); //sleduje to/trackuje hráče
        _player.TakeDamage(_damage); //damage metoda, kterou voláme na daném hráči
    }

}