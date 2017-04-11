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

    public GameObject go;

	// Use this for initialization
	void Start () 
    {
	
        Hashtable args = new Hashtable();
        //设置路径的点
        args.Add("path",paths);
        //设置类型为线性，线性效果会好一些。
        args.Add("easeType", iTween.EaseType.linear);
        //设置寻路的速度
        args.Add("speed",10f);
        //是否先从原始位置走到路径中第一个点的位置
        args.Add("movetopath",true);
        //是否让模型始终面朝当面目标的方向，拐弯的地方会自动旋转模型
        //如果你发现你的模型在寻路的时候始终都是一个方向那么一定要打开这个
        args.Add("orienttopath",true);

        //让模型开始寻路   
        iTween.MoveTo(go,args);
	}
	
	void Update () 
    {
	
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
