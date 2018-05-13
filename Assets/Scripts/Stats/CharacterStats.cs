using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class CharacterStats : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmp;

    [SyncVar]
    public int maxHealth;
    [SyncVar]
    private bool isDead = false;
    [SerializeField]
    [SyncVar(hook = "OnChangedHealth")]
    private int _currentHealth;

    [Header("Optional: ")]
    [SyncVar]
    public Stat damage;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        //If a Player already connected we need to update OUR instance of TMP to show their current health
        //According to Unity Docs _currentHealth should already be SyncVar'd by now...
        if (isClient)
        {
            UpdateHealthStatusText(_currentHealth);
        }
    }

    public override void OnStartLocalPlayer()
    {
        Setup();
        UpdateHealthStatusText(_currentHealth);
        CmdUpdateVars(_currentHealth,damage);
    }

    [Command]
    void CmdUpdateVars(int health, Stat dmg)
    {
        _currentHealth = health;
        damage = dmg;
    }

    [Server]
    [ClientRpc]
    void RpcUpdateClientHealthStatusText()
    {
        UpdateHealthStatusText(_currentHealth);
    }

    [Client]
    void UpdateHealthStatusText(int health)
    {
        if (tmp)
            tmp.SetText("{0}/{1}", health, maxHealth);
    }

    [Client]
    void OnChangedHealth(int health)
    {
        _currentHealth = health;
        UpdateHealthStatusText(_currentHealth);
    }

    [Client]
    public virtual void Setup()
    {
        SetDefaults();
    }

    [Client]
    void SetDefaults()
    {
        _currentHealth = maxHealth;
        damage.baseValue = 10;
    }

    [Server]
    public void CheckDeath(int dmg)
    {
        
    }

    [Client]
    private void TakeDamage(int dmg)
    {
        if (isDead)
        {
            Debug.LogErrorFormat("{0} should be dead but isn't", name);
            return;
        }            

        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);
        _currentHealth -= dmg;
        UpdateHealthStatusText(_currentHealth);
        CmdUpdateVars(_currentHealth, damage);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    [Client]
    public void AttackObject(GameObject obj)
    {
        obj.GetComponent<CharacterStats>().TakeDamage(damage.GetValue());
    }

    [Client]
    public virtual void Die()
    {
        isDead = true;
        CmdIAmDead(isDead);
        Destroy(gameObject);
    }

    [Server]
    [Command]
    private void CmdIAmDead(bool dead)
    {
        if (!dead)
            Debug.LogErrorFormat("{0} is calling server-side dead but isDead = {1}", name, dead);
        isDead = dead;
        NetworkServer.Destroy(gameObject);
    }
}
