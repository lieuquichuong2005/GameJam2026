using UnityEngine;

public static class WalkableUtility
{
    /// <summary>
    /// Từ điểm click, tìm điểm đứng hợp lệ trên Ground
    /// </summary>
    public static bool TryGetWalkablePoint(
        Vector2 clickWorldPos,
        Vector2 playerPos,
        out Vector2 walkablePos,
        float maxVerticalOffset,
        LayerMask groundMask)
    {
        float deltaY = clickWorldPos.y - playerPos.y;

        if (Mathf.Abs(deltaY) > maxVerticalOffset)
        {
            walkablePos = default;
            return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(clickWorldPos.x, playerPos.y + 1f),
            Vector2.down,
            maxVerticalOffset + 2f,
            groundMask);

        if (hit.collider != null)
        {
            walkablePos = hit.point;
            return true;
        }

        walkablePos = default;
        return false;
    }


    /// <summary>
    /// Check đường thẳng từ start -> end có bị obstacle chặn không
    /// </summary>
    public static bool IsPathClear(
        Vector2 start,
        Vector2 end,
        LayerMask obstacleMask)
    {
        Vector2 dir = end - start;
        float dist = dir.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(
            start,
            dir.normalized,
            dist,
            obstacleMask);

        return hit.collider == null;
    }
}