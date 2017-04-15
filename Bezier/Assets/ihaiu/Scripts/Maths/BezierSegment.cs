using UnityEngine;
using System.Collections;

namespace ihaiu
{
    /// <summary>
    /// Bezier segment.贝塞尔线段，两个节点组成(两个锚点，两个控制点)。
    /// </summary>
    [System.Serializable]
    public class BezierSegment 
    {

        /** 开始点 */
        [SerializeField]
        public BezierPoint begin = BezierPoint.Zero;
        /** 结束点 */
        [SerializeField]
        public BezierPoint end   = BezierPoint.Zero;

        /** [辅助]平均速度   */
        public LinePath averagePath     = new LinePath();
        /** [辅助]平均速度 精度  */
        [SerializeField]
        public int      averageAccuracy = 10;


        private bool isInit;
        private bool isInitAverage;
        private float length = 0;


        private float Ax, Bx, Cx;
        private float Ay, By, Cy;
        private float Az, Bz, Cz;

        public BezierSegment()
        {
        }

        public BezierSegment(Vector3 beginAnchor, Vector3 beginControl, Vector3 endControl, Vector3 endAnchor)
        {
            begin.anchorPoint = beginAnchor;
            begin.controlPoint = beginControl;

            end.controlPoint = endControl;
            end.anchorPoint = endAnchor;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="beginAnchor">开始锚点</param>
        /// <param name="beginControl">开始控制点</param>
        /// <param name="endControl">结束控制点</param>
        /// <param name="endAnchor">结束锚点</param>
        public BezierSegment Init(Vector3 beginAnchor, Vector3 beginControl, Vector3 endControl, Vector3 endAnchor)
        {
            begin.anchorPoint = beginAnchor;
            begin.controlPoint = beginControl;

            end.controlPoint = endControl;
            end.anchorPoint = endAnchor;

            Reset();
            return this;
        }

        public BezierSegment Init(BezierPoint begin, BezierPoint end)
        {
            this.begin  = begin;
            this.end    = end;

            Reset();
            return this;
        }

        public BezierSegment Init()
        {
            if (isInit)
                return this;
            isInit          = true;
            isInitAverage   = false;
            length          = 0;

            //x多项式系数
            Cx = 3 * (begin.controlPoint.x  - begin.anchorPoint.x);
            Bx = 3 * (end.controlPoint.x    - begin.controlPoint.x) - Cx;
            Ax = end.anchorPoint.x - begin.anchorPoint.x - Cx - Bx;


            //y多项式系数
            Cy = 3 * (begin.controlPoint.y  - begin.anchorPoint.y);
            By = 3 * (end.controlPoint.y    - begin.controlPoint.y) - Cy;
            Ay = end.anchorPoint.y - begin.anchorPoint.y - Cy - By;

            //z多项式系数
            Cz = 3 * (begin.controlPoint.z  - begin.anchorPoint.z);
            Bz = 3 * (end.controlPoint.z    - begin.controlPoint.z) - Cz;
            Az = end.anchorPoint.z - begin.anchorPoint.z - Cz - Bz;

            return this;
        }


        public BezierSegment Reset()
        {
            isInit = false;
            Init();
            return this;
        }


        /// <summary>
        /// 获取当前位置 
        /// </summary>
        /// <param name="t">[0.0~1.0]</param>
        /// <returns></returns>
        public Vector3 Get(float t)
        {
            Init();

            float tSquared = t * t;
            float tCubed = tSquared * t;

            //计算返回值
            Vector3 result;
            result.x = (Ax * tCubed) + (Bx * tSquared) + (Cx * t) + begin.anchorPoint.x;
            result.y = (Ay * tCubed) + (By * tSquared) + (Cy * t) + begin.anchorPoint.y;
            result.z = (Az * tCubed) + (Bz * tSquared) + (Cz * t) + begin.anchorPoint.z;

            return result; 
        }

        /// <summary>
        /// 获取路径长度
        /// </summary>
        /// <returns>The path length.</returns>
        /// <param name="accuracy">精度.</param>
        public float GetPathLength(int accuracy = 10)
        {
            if (length > 0)
                return length;
            
            length = 0;
            Vector3 prevPt = Get(0);
            int SmoothAmount = 4*accuracy;
            for (int i = 1; i <= SmoothAmount; i++) {
                float pm = (float) i / SmoothAmount;
                Vector3 currPt = Get(pm);
                length += Vector3.Distance(prevPt,currPt);
                prevPt = currPt;
            }

            return length;
        }


//        public Vector3 GetPoint(float t)
//        {
//            return begin.anchorPoint * Mathf.Pow(1 - t, 3) + 3 * begin.controlPoint * t * Mathf.Pow(1 -t, 2) + 3 * end.controlPoint * Mathf.Pow(t, 2) * (1 - t) + end.anchorPoint * Mathf.Pow(t, 3);
//        }

        /// <summary>
        /// 初始化 匀速
        /// </summary>
        public BezierSegment InitAverage()
        {
            if (isInitAverage)
                return this;
            isInitAverage = true;

            Init();

            if (averagePath.points == null)
            {
                averagePath.points = new System.Collections.Generic.List<Vector3>();
            }
            else
            {
                averagePath.points.Clear();
            }

            int SmoothAmount = 4*averageAccuracy;

            for (int i = 0; i <= SmoothAmount; i++)
            {
                float pm = (float) i / SmoothAmount;
                averagePath.points.Add(Get(pm));
            }

            averagePath.Reset();
            return this;
        }


        /// <summary>
        /// 获取平均速度位置 
        /// </summary>
        /// <param name="t">[0.0~1.0]</param>
        /// <returns></returns>
        public Vector3 GetAverage(float t)
        {
            InitAverage();
            return averagePath.Get(t);
        }





        public BezierSegment DrawPath(Color color)
        {

            Gizmos.color=color;
            int SmoothAmount = 4*20;

            Vector3 prevPt = Get(0);
            for (int i = 1; i <= SmoothAmount; i++)
            {
                float pm = (float) i / SmoothAmount;
                Vector3 currPt = Get(pm);
                Gizmos.DrawLine(currPt, prevPt);
                prevPt = currPt;
            }

            return this;
        }


        public BezierSegment DrawNode(Color color, float size)
        {
            Gizmos.color=color;
            Gizmos.DrawSphere(begin.anchorPoint, size);
            Gizmos.DrawSphere(end.anchorPoint, size);


            Gizmos.DrawCube(begin.controlPoint, Vector3.one * size);
            Gizmos.DrawCube(end.controlPoint, Vector3.one * size);
            return this;
        }

        public BezierSegment DrawPoint(Color color, float size)
        {
            Gizmos.color=color;
            int SmoothAmount = 4*10;

            for (int i = 0; i <= SmoothAmount; i++)
            {
                float pm = (float) i / SmoothAmount;
                Gizmos.DrawWireSphere(Get(pm), size);
            }
            return this;
        }



        private float drawOffset = 0;
        public BezierSegment DrawAveragePoint(Color color, float size)
        {
            Gizmos.color=color;
            int SmoothAmount = 4*averageAccuracy;

            drawOffset += 0.02f;

            for (int i = 0; i <= SmoothAmount; i++)
            {
                float pm = (float) i / SmoothAmount + drawOffset;
                pm = Mathf.Repeat(pm, 1);
                Gizmos.DrawWireSphere(GetAverage(pm), size);
            }
            return this;
        }
    }
}
