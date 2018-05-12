using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class CharacterStats : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmp;

    [SyncVar]
    public int maxHealth;

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
        //Istead of having the server calculate damage we should just do this
        //I'm attacking something (I know my damage and I know it's CharacterStat damage
        //Let my target calculate the result on my machine
        //let my target update it's status to the server
        //Then the server will let everyone else know what happened.
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

    [SyncVar]
    private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        protected set { isDead = value; }
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
    public void ServerTakeDamage(int dmg)
    {
        Debug.LogFormat("{0} is being attacked with {1} dmg", name, dmg);
        if (isDead)
            return;
        //dmg -= armor.GetValue();

        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);
        _currentHealth -= dmg;

        Debug.LogFormat("{0} takes {1} damage", transform.name, dmg);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }


    [Command]
    public void CmdAttack(GameObject obj)
    {
        Debug.LogFormat("{0} is attack {1} with {2} dmg", name, obj.name, damage.GetValue());
        obj.GetComponent<CharacterStats>().ServerTakeDamage(damage.GetValue());
    }

    public virtual void Die()
    {
        isDead = true;
        Debug.Log(transform.name + " died.");
    }
}
