using UnityEngine;
using Mirror;
public class Player : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    [SyncVar] //aplikuje se damage a unity to sync všem hráčům
    private int currentHealth;

    void Awake ()
    {
        SetDefaults ();

    }

    public void TakeDamage (int _amount)
    {
        currentHealth-= _amount;

        Debug.Log(transform.name + " now has" + currentHealth + " health."); // Jen hostitel to muže videt v konzoli
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }
}
