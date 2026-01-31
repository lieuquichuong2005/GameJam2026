using EditorAttributes;
using Spine.Unity;
using UnityEngine;

public class PlayerController : MonoBehaviour, IEntity
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 5f;

    private string Idle = "Idle";
    private string Walk = "Walk";

    [Required] [SerializeField] private SkeletonAnimation _character;

    private bool _isFacingRight = true;
    private float targetX;
    private bool isMoving;
    private string _currentAnim;

    public void MoveToX(float x)
    {
        targetX = x;
        isMoving = true;

        UpdateFacing(x);
        PlayAnim(Walk, true);
    }

    public bool ReachedX(float x, float threshold = 0.3f)
    {
        return Mathf.Abs(transform.position.x - x) <= threshold;
    }

    public void OnUpdate(float deltaTime)
    {
        if (isMoving)
        {
            UpdateFacing(targetX);

            float newX = Mathf.MoveTowards(
                transform.position.x,
                targetX,
                moveSpeed * deltaTime
            );

            transform.position = new Vector2(newX, transform.position.y);

            PlayAnim(Walk, true);

            if (ReachedX(targetX))
            {
                isMoving = false;
                PlayAnim(Idle, true);
            }
        }
        else
        {
            PlayAnim(Idle, true);
        }

        SnapToGround();
    }

    private void SnapToGround()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y + 0.5f);

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            groundCheckDistance,
            groundMask
        );

        if (hit.collider == null) return;

        float distance = origin.y - hit.point.y;

        if (distance > 0f && distance < 0.3f)
        {
            transform.position = new Vector2(
                transform.position.x,
                hit.point.y
            );
        }
    }

    private void PlayAnim(string animName, bool loop)
    {
        if (_currentAnim == animName) return;

        _character.AnimationState.SetAnimation(0, animName, loop);
        _currentAnim = animName;
    }

    private void UpdateFacing(float targetX)
    {
        bool shouldFaceRight = targetX > transform.position.x;

        if (_isFacingRight != shouldFaceRight)
        {
            _isFacingRight = shouldFaceRight;

            _character.Skeleton.ScaleX = _isFacingRight ? 1 : -1;
        }
    }
}