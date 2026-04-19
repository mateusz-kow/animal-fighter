using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 7f;
    public GameObject projectilePrefab;
    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.up;
    private Rigidbody2D rb;
    private Animator animator;
    private float mapLimit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void SetBounds(float size) => mapLimit = size - 0.5f;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            lastDirection = moveInput.normalized;
            if (animator)
            {
                animator.SetBool("isMoving", true);
                if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                    animator.SetInteger("direction", moveInput.x > 0 ? 3 : 1);
                else
                    animator.SetInteger("direction", moveInput.y > 0 ? 2 : 0);
            }
        }
        else if (animator) animator.SetBool("isMoving", false);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;

        if (context.performed && projectilePrefab)
        {
            GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            p.GetComponent<Rigidbody2D>().linearVelocity = lastDirection * 12f;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
        
        float cX = Mathf.Clamp(rb.position.x, -0.5f, mapLimit);
        float cY = Mathf.Clamp(rb.position.y, -0.5f, mapLimit);
        rb.position = new Vector2(cX, cY);
    }

    public void PlayFootstep()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (moveInput != Vector2.zero && !audio.isPlaying)
        {
            audio.Play();
        }
    }
}