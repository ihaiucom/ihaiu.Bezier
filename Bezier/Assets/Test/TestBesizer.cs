using UnityEngine;
using System.Collections;

public class TestBesizer : MonoBehaviour {

    public Transform[] paths;
    public bool line            = true;
    public bool lineGizmos      = true;
    public bool lineHandles     = true;

    public bool path            = true;
    public bool pathGizmos      = true;
    public bool pathHandles     = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        if(line) 
            iTween.DrawLine(paths);
        
        if(lineGizmos)
            iTween.DrawLineGizmos(paths);

        if(lineHandles)
            iTween.DrawLineHandles(paths);



        if(path) 
            iTween.DrawPath(paths);

        if(pathGizmos)
            iTween.DrawPathGizmos(paths);

        if(pathHandles)
            iTween.DrawPathHandles(paths);

    }

    [ContextMenu("Set")]
    public void Set()
    {
        iTweenPath path = GetComponent<iTweenPath>();
        path.nodes.Clear();

        foreach(Transform node in paths)
        {
            path.nodes.Add(node.position);
        }
    }
}
