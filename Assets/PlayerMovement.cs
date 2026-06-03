using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed = 5f;
    Rigidbody2D rb;
    float jumpForce = 7f;
    int maxJumps = 2;
    int jumpsRemaining;
    SpriteRenderer spriteRenderer;

    int gem = 0;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI gameOverText;

    public TextMeshProUGUI collectGemText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpsRemaining = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput = -1f;
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1f;
        }
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;
        }

        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        } 
        else if (moveInput > 0) 
        {
            spriteRenderer.flipX = false;
        }

        gemText.text = "Gems: " + Mathf.FloorToInt(gem).ToString();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsRemaining = maxJumps;
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Spikes"))
        {
            gem = 0; 
            RestartLevel();
        }
        if (other.CompareTag("Gem"))
        {
            Destroy(other.gameObject); 
            gem += 1;
        }
        if (other.CompareTag("Goal"))
        {
            if (gem == 3)
            {
                collectGemText.gameObject.SetActive(false);
                gameOverText.gameObject.SetActive(true);
                gameOverText.text = "YOU WIN!!!";
                Debug.Log("YOU WIN!");
                Time.timeScale = 0f;
            }
            else
            {
                collectGemText.gameObject.SetActive(true);
                collectGemText.text = "Collect the all the GEMS to win";
                Debug.Log("Collect the all the GEMS to win"); 
            }
        }        
        if (other.CompareTag("Enemy"))
        {
            gem = 0;
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }




}