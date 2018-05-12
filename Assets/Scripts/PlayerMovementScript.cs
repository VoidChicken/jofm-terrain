using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
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
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * Time.deltaTime * playerSpeed;

        if (Input.GetButtonDown("Jump"))
            isJumping = true;

        if (Input.GetKeyDown("i"))
            GetComponent<CharacterStats>().CmdAttack(gameObject);
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
