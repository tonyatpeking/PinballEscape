using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public enum Orientation
    {
        CLOCKWISE,
        COUNTER_CLOCKWISE,
        COLLINEAR
    }

    public enum Side
    {
        Left,
        Right
    }

    public static bool IsCollinearPointOnSegment2D(Vector2 p, Vector2 lineStart, Vector2 lineEnd)
    {
        if (lineStart.x <= Mathf.Max(p.x, lineEnd.x) && lineStart.x >= Mathf.Min(p.x, lineEnd.x) &&
        lineStart.y <= Mathf.Max(p.y, lineEnd.y) && lineStart.y >= Mathf.Min(p.y, lineEnd.y))
        {
            return true;
        }
        return false;
    }

    // the orientation of three points ( clockwise c-clockwise, collinear )
    public static Orientation OrientationOfPoints2D(Vector2 p, Vector2 q, Vector2 r)
    {
        // See http://www.geeksforgeeks.org/orientation-3-ordered-points/
        // for details of below formula.
        float val = (q.y - p.y) * (r.x - q.x) -
                  (q.x - p.x) * (r.y - q.y);

        if (val == 0)
            return Orientation.COLLINEAR;  // collinear
        return (val > 0) ? Orientation.CLOCKWISE : Orientation.COUNTER_CLOCKWISE; // clock or counter clock wise
    }

    public static bool DoLinesIntersect(Vector2 lineAStart, Vector2 lineAEnd, Vector2 lineBStart, Vector2 lineBEnd)
    {
        // Find the four orientations needed for general and
        // special cases
        Orientation o1 = OrientationOfPoints2D(lineAStart, lineAEnd, lineBStart);
        Orientation o2 = OrientationOfPoints2D(lineAStart, lineAEnd, lineBEnd);
        Orientation o3 = OrientationOfPoints2D(lineBStart, lineBEnd, lineAStart);
        Orientation o4 = OrientationOfPoints2D(lineBStart, lineBEnd, lineAEnd);

        // General case
        if (o1 != o2 && o3 != o4)
            return true;

        // Special Cases
        // p1, q1 and p2 are collinear and p2 lies on segment p1q1
        if (o1 == Orientation.COLLINEAR && IsCollinearPointOnSegment2D(lineAStart, lineBStart, lineAEnd))
            return true;

        // p1, q1 and p2 are collinear and q2 lies on segment p1q1
        if (o2 == Orientation.COLLINEAR && IsCollinearPointOnSegment2D(lineAStart, lineBEnd, lineAEnd))
            return true;

        // p2, q2 and p1 are collinear and p1 lies on segment p2q2
        if (o3 == Orientation.COLLINEAR && IsCollinearPointOnSegment2D(lineBStart, lineAStart, lineBEnd))
            return true;

        // p2, q2 and q1 are collinear and q1 lies on segment p2q2
        if (o4 == Orientation.COLLINEAR && IsCollinearPointOnSegment2D(lineBStart, lineAEnd, lineBEnd))
            return true;

        return false; // Doesn't fall in any of the above cases
    }

    public static bool IsPointInTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        var s = p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y;
        var t = p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y;

        if ((s < 0) != (t < 0))
            return false;

        var A = -p1.y * p2.x + p0.y * (p2.x - p1.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y;
        if (A < 0.0)
        {
            s = -s;
            t = -t;
            A = -A;
        }
        return s > 0 && t > 0 && (s + t) <= A;
    }

}
