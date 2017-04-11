using UnityEngine;
using System.Collections;

namespace ihaiu
{
    /// <summary>
    /// Bezier segment.贝塞尔线段，两个节点组成(两个锚点，两个控制点)。不经过控制点
    /// </summary>
    public class BezierCurveSegment 
    {

        public BezierPoint begin = BezierPoint.Zero;
        public BezierPoint end   = BezierPoint.Zero;

        public BezierCurveSegment()
        {
        }

        public BezierCurveSegment(BezierPoint begin, BezierPoint end)
        {
            this.begin  = begin;
            this.end    = end;
        }


        public Vector3 GetPoint(float t)
        {
            return begin.anchorPoint * Mathf.Pow(1 - t, 3) + 3 * begin.controlPoint * t * Mathf.Pow(1 -t, 2) + 3 * end.controlPoint * Mathf.Pow(t, 2) * (1 - t) + end.anchorPoint * Mathf.Pow(t, 3);
        }

    }
}
