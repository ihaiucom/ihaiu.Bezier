using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ihaiu
{
    [CustomEditor(typeof(BezierSegmentView))]
    public class BezierSegmentEditor : Editor
    {

        private static GUIStyle _boxModuleStyle;
        public static GUIStyle boxModuleStyle
        {
            get
            {

                if (_boxModuleStyle == null)
                {
                    GUIStyle style = new GUIStyle(EditorStyles.helpBox);
                    style.padding = new RectOffset(10, 10, 10, 10);
                    style.margin = new RectOffset(5, 5, 5, 5);
                    _boxModuleStyle = style;
                }
                return _boxModuleStyle;
            }
        }

        BezierSegmentView _target;
    	GUIStyle style = new GUIStyle();
    	public static int count = 0;

    	void OnEnable()
        {
    		style.fontStyle = FontStyle.Bold;
    		style.normal.textColor = Color.white;
            _target = (BezierSegmentView)target;
    		

            //lock in a default path name:
            if(!_target.initialized){
                _target.initialized = true;
                _target.pathName = "New BezierSegment " + ++count;
                _target.initialName = _target.pathName;
            }
    	}

    	public override void OnInspectorGUI(){	
            

            EditorGUILayout.BeginVertical(boxModuleStyle);
    		//draw the path?
    		EditorGUILayout.BeginHorizontal();
    		EditorGUILayout.PrefixLabel("Path Visible");
    		_target.pathVisible = EditorGUILayout.Toggle(_target.pathVisible);
    		EditorGUILayout.EndHorizontal();

            if (_target.pathVisible)
            {
                // line
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Line Visible");
                _target.lineVisible = EditorGUILayout.Toggle(_target.lineVisible);
                EditorGUILayout.EndHorizontal();

                // node
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Node Visible");
                _target.nodeVisible = EditorGUILayout.Toggle(_target.nodeVisible);
                EditorGUILayout.EndHorizontal();

                if (_target.nodeVisible)
                {

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Node Size");
                    _target.nodeSize = EditorGUILayout.Slider(_target.nodeSize, 0.1f, 1f);
                    EditorGUILayout.EndHorizontal();
                }


                //path color:
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Path Color");
                _target.pathColor = EditorGUILayout.ColorField(_target.pathColor);
                EditorGUILayout.EndHorizontal();

                // point
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Point Visible");
                _target.pointVisible = EditorGUILayout.Toggle(_target.pointVisible);
                EditorGUILayout.EndHorizontal();

                if (_target.pointVisible)
                {

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Point Size");
                    _target.pointSize = EditorGUILayout.Slider(_target.pointSize, 0.01f, 1f);
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Point Color");
                    _target.pointColor = EditorGUILayout.ColorField(_target.pointColor);
                    EditorGUILayout.EndHorizontal();
                }




                //average point:
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Average Point Visible");
                _target.averagePointVisible = EditorGUILayout.Toggle(_target.averagePointVisible);
                EditorGUILayout.EndHorizontal();

                if (_target.averagePointVisible)
                {

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Average Accuracy");
                    _target.path.averageAccuracy = EditorGUILayout.IntSlider(_target.path.averageAccuracy, 2, 40);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Average Point Size");
                    _target.averagePointSize = EditorGUILayout.Slider(_target.averagePointSize, 0.01f, 1f);
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Average Point Color");
                    _target.averagePointColor = EditorGUILayout.ColorField(_target.averagePointColor);
                    EditorGUILayout.EndHorizontal();
                }


                GUILayout.Space(10);
                EditorGUILayout.LabelField("(path length=" + _target.path.GetPathLength() + ")");

            }



            EditorGUILayout.EndVertical();


            //path name:
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Path Name");
            _target.pathName = EditorGUILayout.TextField(_target.pathName);
            EditorGUILayout.EndHorizontal();

            if(_target.pathName == ""){
                _target.pathName = _target.initialName;
            }

    		//node display:
            _target.path.begin.anchorPoint  = EditorGUILayout.Vector3Field("Begin Anchor " , _target.path.begin.anchorPoint);
            _target.path.begin.controlPoint = EditorGUILayout.Vector3Field("Begin Control " , _target.path.begin.controlPoint);
            _target.path.end.controlPoint   = EditorGUILayout.Vector3Field("End Control " , _target.path.end.controlPoint);
            _target.path.end.anchorPoint    = EditorGUILayout.Vector3Field("End Anchor " , _target.path.end.anchorPoint);


    		//update and redraw:
    		if(GUI.changed){
    			EditorUtility.SetDirty(_target);			
    		}
    	}
    	
    	void OnSceneGUI(){
            if(_target.pathVisible && _target.nodeVisible)
            {	

                Undo.RecordObject(_target, "Adjust BezierSegment");

                Handles.Label(_target.path.begin.anchorPoint + Vector3.one * 1, _target.pathName, style);
                Handles.Label(_target.path.begin.anchorPoint, "Begin Anchor", style);
                Handles.Label(_target.path.begin.controlPoint, "Begin Control", style);
                Handles.Label(_target.path.end.controlPoint, "End Control", style);
                Handles.Label(_target.path.end.anchorPoint, "End Anchor", style);

                _target.path.begin.anchorPoint  =  Handles.PositionHandle(_target.path.begin.anchorPoint, Quaternion.identity);
                _target.path.begin.controlPoint =  Handles.PositionHandle(_target.path.begin.controlPoint, Quaternion.identity);
                _target.path.end.controlPoint   =  Handles.PositionHandle(_target.path.end.controlPoint, Quaternion.identity);
                _target.path.end.anchorPoint    =  Handles.PositionHandle(_target.path.end.anchorPoint, Quaternion.identity);

    		}
    	}
    }
}