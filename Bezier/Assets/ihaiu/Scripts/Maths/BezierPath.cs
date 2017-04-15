using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ihaiu
{
    /// <summary>
    /// Bezier path.贝塞尔路径，曲线经过路径点
    /// </summary>
    public class BezierPath 
    {
        public List<Vector3> paths;
        private Vector3[] pts;


        /** [辅助]平均速度   */
        public LinePath averagePath     = new LinePath();
        /** [辅助]平均速度 精度  */
        [SerializeField]
        public int      averageAccuracy = 10;


        private bool isInit;
        private bool isInitAverage;
        public int   nodeCount = 0;
        private float length    = 0;

        public BezierPath()
        {
        }

        public BezierPath(params Vector3[] points)
        {
            paths = new List<Vector3>();
            for(int i = 0 ; i < points.Length; i ++)
            {
                paths.Add(points[i]);
            }
        }

        public BezierPath(List<Vector3> paths)
        {
            this.paths = paths;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="paths">路径点列表</param>
        public BezierPath Init(List<Vector3> paths)
        {
            this.paths = paths;

            Reset();
            return this;
        }


        public BezierPath Init()
        {
            if (paths == null || paths.Count == 0)
                return this;
            
            if (isInit)
                return this;
            isInit          = true;
            isInitAverage   = false;
            length          = 0;

            ControlPointGenerator();

            return this;
        }


        public BezierPath Reset()
        {
            isInit = false;
            Init();
            return this;
        }

        void ControlPointGenerator()
        {
            nodeCount = paths.Count;
            pts = new Vector3[paths.Count + 2];
            for(int i = 0; i <  paths.Count; i ++)
            {
                pts[i + 1] = paths[i];
            }

            pts[0] = pts[1] + (pts[1] - pts[2]);
            pts[pts.Length-1] = pts[pts.Length-2] + (pts[pts.Length-2] - pts[pts.Length-3]);

            if(pts[1] == pts[pts.Length-2])
            {
                pts[0]=pts[pts.Length-3];
                pts[pts.Length-1]=pts[2];
            }  
        }


        /// <summary>
        /// 获取当前位置 
        /// </summary>
        /// <param name="t">[0.0~1.0]</param>
        /// <returns></returns>
        public Vector3 Get(float t) 
        {
            int numSections = pts.Length - 3;
            int currPt = Mathf.Min(Mathf.FloorToInt(t * (float) numSections), numSections - 1);
            float u = t * (float) numSections - (float) currPt;
            Vector3 a = pts[currPt];
            Vector3 b = pts[currPt + 1];
            Vector3 c = pts[currPt + 2];
            Vector3 d = pts[currPt + 3];
            return .5f*((-a+3f*b-3f*c+d)*(u*u*u)+(2f*a-5f*b+4f*c-d)*(u*u)+(-a+c)*u+2f*b);
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
            int SmoothAmount = nodeCount*accuracy;
            for (int i = 1; i <= SmoothAmount; i++) {
                float pm = (float) i / SmoothAmount;
                Vector3 currPt = Get(pm);
                length += Vector3.Distance(prevPt,currPt);
                prevPt = currPt;
            }

            return length;
        }



        /// <summary>
        /// 初始化 匀速
        /// </summary>
        public BezierPath InitAverage()
        {
            if (paths == null || paths.Count == 0)
                return this;
            
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

            int SmoothAmount = nodeCount*averageAccuracy;

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



        public BezierPath DrawPath(Color color)
        {
            if (paths== null || paths.Count == 0)
                return this;

            Gizmos.color=color;
            int SmoothAmount = nodeCount*20;

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


        public BezierPath DrawNode(Color color, float size)
        {
            if (paths== null || paths.Count == 0)
                return this;
            
            Gizmos.color=color;
            for (int i = 0; i < paths.Count; i++) 
            {
                Gizmos.DrawSphere(paths[i], size);
            }

            return this;
        }

        public BezierPath DrawPoint(Color color, float size)
        {
            if (paths== null || paths.Count == 0)
                return this;
            
            Gizmos.color=color;
            int SmoothAmount = nodeCount*10;

            for (int i = 0; i <= SmoothAmount; i++)
            {
                float pm = (float) i / SmoothAmount;
                Gizmos.DrawWireSphere(Get(pm), size);
            }
            return this;
        }



        public BezierPath DrawAveragePoint(Color color, float size)
        {
            if (paths== null || paths.Count == 0)
                return this;
            
            Gizmos.color=color;
            int SmoothAmount = nodeCount*averageAccuracy;

            for (int i = 0; i <= SmoothAmount; i++)
            {
                float pm = (float) i / SmoothAmount;
                Gizmos.DrawWireSphere(GetAverage(pm), size);
            }
            return this;
        }
    }
}