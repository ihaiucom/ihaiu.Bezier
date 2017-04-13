using UnityEngine;
using System.Collections;

namespace ihaiu
{
    [System.Serializable]
    public class BezierPathView : MonoBehaviour {


        public bool                 pathVisible     = true;
        public bool                 lineVisible     = true;
        public bool                 nodeVisible     = true;
        public float                nodeSize        = 0.2f;
        public Color                pathColor       = Color.cyan;
        public bool                 pointVisible    = false;
        public float                pointSize       = 0.01f;
        public Color                pointColor       = Color.black;

        public bool                 averagePointVisible    = false;
        public float                averagePointSize       = 0.01f;
        public Color                averagePointColor       = Color.white;




        public string               pathName        ="";
        public bool                 initialized     = false;
        public string               initialName     = "";


        [SerializeField]
        public BezierPath    path            = new BezierPath(Vector3.zero, new Vector3(2, 0, 2), new Vector3(-2, 0, 4), new Vector3(0, 0, 6));

        void OnEnable()
        {
            
        }

        void OnDisable()
        {
        }

        void OnDrawGizmosSelected()
        {
            if(pathVisible)
            {
                path.Reset();
                if (lineVisible)
                {
                    path.DrawPath(pathColor);
                }

                if (nodeVisible)
                {
                    path.DrawNode(pathColor, nodeSize);
                }

                if (pointVisible)
                {
                    path.DrawPoint(pointColor, pointSize);
                }

                if (averagePointVisible)
                {
                    path.DrawAveragePoint(averagePointColor, averagePointSize);
                }

            }
        }

    }
}