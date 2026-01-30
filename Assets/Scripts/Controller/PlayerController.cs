using UnityEngine;

public class PlayerController : MonoBehaviour, IEntity
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 5f;

    private float targetX;
    private bool isMoving;

    public void MoveToX(float x)
    {
        targetX = x;
        isMoving = true;

        Debug.Log($"[PLAYER] Move to X = {x}");
    }

    public bool ReachedX(float threshold = 0.05f)
    {
        return Mathf.Abs(transform.position.x - targetX) <= threshold;
    }

    public void OnUpdate(float deltaTime)
    {
        if (isMoving)
        {
            float newX = Mathf.MoveTowards(
                transform.position.x,
                targetX,
                moveSpeed * deltaTime
            );

            transform.position = new Vector2(newX, transform.position.y);

            if (ReachedX())
            {
                isMoving = false;
                Debug.Log("[PLAYER] Reached X target");
            }
        }

        SnapToGround();
    }

    private void SnapToGround()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y + 1f);

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            groundCheckDistance,
            groundMask
        );

        if (hit.collider != null)
        {
            transform.position = new Vector2(
                transform.position.x,
                hit.point.y
            );
        }
    }
}