using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển
    public float jumpForce = 10f; // Lực nhảy
    public float dashForce = 10f; // Lực dash
    private Rigidbody2D rb;
    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool facingRight = true; // Biến để theo dõi hướng mà nhân vật đang đối diện
    private Animator anim; // Tham chiếu tới Animator
    private bool isDashing = false;
    public float dashTime = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Kiểm tra xem Animator có tồn tại không
        if (anim == null)
        {
            Debug.LogError("Animator not found on the Player object.");
        }
    }

    void Update()
    {
        // Lấy input từ người chơi
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (!isDashing)
        {
            // Di chuyển đối tượng theo input
            Vector2 movement = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
            rb.velocity = movement;

            // Kiểm tra và quay đầu nhân vật
            if (moveHorizontal > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveHorizontal < 0 && facingRight)
            {
                Flip();
            }
        }

        // Kiểm tra xem player có đang đứng trên mặt đất không
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Nếu player đang đứng trên mặt đất và người chơi nhấn phím Space
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Áp dụng lực nhảy
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // Áp dụng animation nhảy
            anim.SetTrigger("Jump");
        }

        // Nếu player không đứng trên mặt đất và không nhấn phím nhảy, chuyển sang animation Fall
        if (!isGrounded && rb.velocity.y < 0)
        {
            anim.SetBool("Fall", true);
        }
        else
        {
            anim.SetBool("Fall", false);
        }

        // Cập nhật trạng thái của Animator
        anim.SetFloat("Speed", Mathf.Abs(moveHorizontal));
        anim.SetBool("Grounded", isGrounded);

        // Dash khi nhấn phím E
        if (Input.GetKeyDown(KeyCode.E) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        anim.SetFloat("Move", Mathf.Abs(moveHorizontal));
    }

    // Phương thức để quay đầu nhân vật
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Coroutine để xử lý Dash
    IEnumerator Dash()
    {
        isDashing = true;
        float dashDirection = facingRight ? 1f : -1f;
        rb.velocity = new Vector2(dashForce * dashDirection, rb.velocity.y);
        yield return new WaitForSeconds(dashTime); // Đảm bảo hàm WaitForSeconds được viết đúng
        rb.velocity = new Vector2(0f, rb.velocity.y);
        isDashing = false;
    }

    // Vẽ vòng tròn kiểm tra mặt đất trong Scene view để kiểm tra
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}
