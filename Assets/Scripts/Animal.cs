using UnityEngine;

public class Animal : MonoBehaviour
{
    [HideInInspector] public Transform targetBurrow;
    public float speed = 3f;
    private Animator animator;

    void Start() => animator = GetComponent<Animator>();

    void Update()
    {
        if (targetBurrow == null) return;

        Vector2 dir = (targetBurrow.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetBurrow.position, speed * Time.deltaTime);

        if (animator)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                animator.SetInteger("direction", dir.x > 0 ? 3 : 1);
            else
                animator.SetInteger("direction", dir.y > 0 ? 2 : 0);
        }

        if (Vector2.Distance(transform.position, targetBurrow.position) < 0.2f)
            Destroy(gameObject);
    }
}