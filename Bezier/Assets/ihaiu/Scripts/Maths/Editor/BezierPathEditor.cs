using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ihaiu
{
    [CustomEditor(typeof(BezierPathView))]
    public class BezierPathEditor : Editor
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

        BezierPathView _target;
    	GUIStyle style = new GUIStyle();
    	public static int count = 0;

    	void OnEnable()
        {
    		style.fontStyle = FontStyle.Bold;
    		style.normal.textColor = Color.white;
            _target = (BezierPathView)target;


            _nodeCount = _target.path.paths.Count;
    		

            //lock in a default path name:
            if(!_target.initialized){
                _target.initialized = true;
                _target.pathName = "New BezierPath " + ++count;
                _target.initialName = _target.pathName;
            }
    	}

        private int _nodeCount = 0;
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


            //exploration segment count control:
            EditorGUILayout.BeginHorizontal();
            _nodeCount = Mathf.Max(2, EditorGUILayout.IntField("Node Count", _nodeCount));
            if (GUILayout.Button("Change"))
            {
                _target.path.nodeCount = _nodeCount;
            }
            EditorGUILayout.EndHorizontal();

            //add node?
            if(_target.path.nodeCount > _target.path.paths.Count){
                for (int i = 0; i < _target.path.nodeCount - _target.path.paths.Count; i++) {
                    _target.path.paths.Add(Vector3.zero);    
                }
            }

            //remove node?
            if(_target.path.nodeCount < _target.path.paths.Count){
                if (EditorUtility.DisplayDialog("Remove path node?", "Shortening the node list will permantently destory parts of your path. This operation cannot be undone.", "OK", "Cancel"))
                {
                    int removeCount = _target.path.paths.Count - _target.path.nodeCount;
                    _target.path.paths.RemoveRange(_target.path.paths.Count - removeCount, removeCount);
                }
                else {
                    _target.path.nodeCount =_target.path.paths.Count;
                }
            }

            //node display:
            EditorGUI.indentLevel = 4;
            for (int i = 0; i < _target.path.paths.Count; i++) {
                _target.path.paths[i] = EditorGUILayout.Vector3Field("Node " + (i+1), _target.path.paths[i]);
            }

    		//update and redraw:
    		if(GUI.changed){
    			EditorUtility.SetDirty(_target);			
    		}
    	}
    	
    	void OnSceneGUI()
        {
            if(_target.pathVisible && _target.nodeVisible && _target.path.paths.Count > 0)
            {	
                //allow path adjustment undo:
                Undo.RecordObject(_target, "Adjust iTween Path");

                //path begin and end labels:
                Handles.Label(_target.path.paths[0], "'" + _target.pathName + "' Begin", style);
                Handles.Label(_target.path.paths[_target.path.paths.Count-1], "'" + _target.pathName + "' End", style);

                //node handle display:
                for (int i = 0; i < _target.path.paths.Count; i++) 
                {
                    _target.path.paths[i] = Handles.PositionHandle(_target.path.paths[i], Quaternion.identity);
                }   

    		}
    	}
    }
}