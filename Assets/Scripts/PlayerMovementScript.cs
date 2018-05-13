using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovementScript : NetworkBehaviour
{

    [SerializeField]
    [Range(1f, 30f)]
    private float jumpMultiplier = 12f;
    [SerializeField]
    private float fallMultiplier = 2.5f;
    [SerializeField]
    private float lowJumpMultiplier = 2f;
    [SerializeField]
    private float playerSpeed = 3;

    float horizontal;
    bool isJumping = false;

    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;

    [SyncVar(hook = "ServerFlipSprite")]
    private float flipSprite = 0;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * Time.deltaTime * playerSpeed;
        FlipSprite();

        if (Input.GetButtonDown("Jump"))
            isJumping = true;

        if (Input.GetKeyDown("i"))
            GetComponent<CharacterStats>().AttackObject(gameObject);
    }

    [Client]
    private void ServerFlipSprite(float flip)
    {
        playerSprite.transform.rotation = Quaternion.Euler(0, flip, 0);
    }

    [Client]
    private void FlipSprite()
    {
        if (horizontal < 0)
            playerSprite.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (horizontal > 0)
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);

        //Make a Server-side Flip
        CmdFlipSprite(playerSprite.transform.rotation.eulerAngles.y);
    }

    [Server]
    [Command]
    private void CmdFlipSprite(float flip)
    {
        flipSprite = flip;
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            rb.AddForce(Vector2.up * jumpMultiplier, ForceMode2D.Impulse);
            isJumping = false;
        }
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        rb.AddForce(new Vector2(horizontal, 0), ForceMode2D.Impulse);

    }
}
