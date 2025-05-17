using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public float moveSpeed = 7f;
    public Boolean alive = true;
    public Boolean paused = false;
    private bool isCollidingWithOrb = false;
    private Animator animator;
    private Rigidbody2D rigidBody;
    public MovementType startGamemode;
    internal PlayerSettings settings;
    [SerializeField] private Sprite cubeSprite;
    [SerializeField] private Sprite shipSprite;
    [SerializeField] private Sprite ballSprite;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private AudioClip deathClip;

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ApplySettings();
    }

    private void ApplySettings()
    {
        SwitchMovement((MovementType)settings.Gamemode);
        playerMovement.SetGravity((GravityDirection)settings.Gravity);
    }

    public void SwitchMovement(MovementType movementType)
    {
        switch (movementType)
        {
            case MovementType.Ball:
                playerMovement = new BallMovement(rigidBody, transform);
                animator.enabled = true;
                break;
            case MovementType.Ship:
                playerMovement = new ShipMovement(rigidBody, transform);
                animator.enabled = false;
                spriteRenderer.sprite = shipSprite;
                break;
            case MovementType.Cube:
            default:
                playerMovement = new CubeMovement(rigidBody, transform);
                animator.enabled = true;
                break;
        }
    }

    public void Die()
    {
        Debug.Log("DED");
        rigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
        animator.SetTrigger("Die");
        alive = false;

        AudioManager.Instance.PlayRandomDeathSFX(0.1f);
        AudioManager.Instance.PauseMusic();

        StartCoroutine(Retry());
    }

    IEnumerator Retry()
    {
        yield return new WaitForSeconds(0.75f);

        transform.position = new Vector3(0, 1, 0);

        transform.eulerAngles = new Vector3(
            0,
            0,
            0
        );

        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.SetTrigger("Alive");
        alive = true;

        ApplySettings();

        List<GameObject> terrains = GameLevelSerializer.GetAllGameObjectsInScene();
        foreach (GameObject obj in terrains)
        {
            if (!obj.CompareTag(((int)EnumTerrain.Block).ToString()))
            {
                continue;
            }

            Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), GetComponent<BoxCollider2D>(), false);
        }

        AudioManager.Instance.RestartMusic();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(((int)EnumTerrain.Block).ToString()))
        {
            Vector3 blockPosition = collision.gameObject.transform.position;
            Vector3 playerPosition = transform.position;

            // Adjust the tolerance for vertical alignment
            float verticalTolerance = collision.gameObject.transform.localScale.y;

            // Calculate the relative position of the player to the block
            bool isComingFromLeft = playerPosition.x < blockPosition.x && Mathf.Abs(playerPosition.y - blockPosition.y) < verticalTolerance;
            bool isComingFromRight = playerPosition.x > blockPosition.x && Mathf.Abs(playerPosition.y - blockPosition.y) < verticalTolerance;
            bool isComingFromTop = playerPosition.y > blockPosition.y && Mathf.Abs(playerPosition.x - blockPosition.x) < collision.gameObject.transform.localScale.x / 2;
            bool isComingFromBottom = playerPosition.y < blockPosition.y && Mathf.Abs(playerPosition.x - blockPosition.x) < collision.gameObject.transform.localScale.x / 2;

            if (isComingFromLeft || isComingFromRight || (playerMovement.GetGravityDirection() < 0 ? isComingFromTop : isComingFromBottom))
            {
                // Ignore collision if coming from the left or right
                Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
            }
            else if (playerMovement.GetGravityDirection() > 0 ? isComingFromTop : isComingFromBottom)
            {
                // Treat as a normal platform
                Debug.Log("GROUND");
                playerMovement.SetGrounded(true);
            }
        }
        else if (collision.gameObject.CompareTag(((int)EnumTerrain.Spike).ToString()))
        {
            Die();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(((int)EnumTerrain.JumpOrb).ToString()))
        {
            isCollidingWithOrb = true;
        }
        else if (collision.CompareTag(((int)EnumTerrain.CubePortal).ToString()))
        {
            SwitchMovement(MovementType.Cube);
        }
        else if (collision.CompareTag(((int)EnumTerrain.ShipPortal).ToString()))
        {
            SwitchMovement(MovementType.Ship);
        }
        else if (collision.CompareTag(((int)EnumTerrain.BallPortal).ToString()))
        {
            SwitchMovement(MovementType.Ball);
        }
        else if (collision.CompareTag("EndPortal"))
        {
            Debug.Log("finish");
            animator.SetTrigger("Die");
            EndMenuManager.Instance.OpenModal();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(((int)EnumTerrain.Block).ToString()))
        {
            playerMovement.SetGrounded(false);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(((int)EnumTerrain.JumpOrb).ToString()))
        {
            isCollidingWithOrb = false;
        }
    }

    private void CheckGroundedStatus()
    {
        // Vérifie si le joueur est en contact avec un objet ayant le tag "Block"
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0);
        bool grounded = false;

        foreach (var hit in hits)
        {
            if (hit.CompareTag(((int)EnumTerrain.Block).ToString()))
            {
                grounded = true;
                break;
            }
        }

        playerMovement.SetGrounded(grounded);
    }

    public void Update()
    {
        CheckGroundedStatus();

        playerMovement.CallMove();

        if (alive && !paused)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }

        //Debug.Log(playerMovement.CanMove());

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerMovement.SwitchGravity();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchMovement(MovementType.Ball);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SwitchMovement(MovementType.Ship);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchMovement(MovementType.Cube);
        }

        HandleOrbJump(); // Check for orb jump
    }

    private void HandleOrbJump()
    {
        // Check if the player is colliding with an orb and the mouse button is pressed
        if (isCollidingWithOrb && Input.GetMouseButtonDown(0))
        {
            rigidBody.linearVelocity = Vector2.up * playerMovement.GetGravityDirection() * 10;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        paused = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        paused = false;
    }
}