using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinePath
{

	public List<Vector3> points;

	private int count;
	public float length;
	private float[] lengths;
	private float[] rates;
	private float[] sumrates;

	public void Reset()
	{
		isInit = false;
		Init();
	}

	private bool isInit;
	private void Init()
	{
		if(isInit) return;
		isInit = true;
		count = points.Count;
		if(count < 2) return;

		length = 0;

		lengths = new float[count];
		lengths[0] = 0;

		if(count == 2)
		{
			lengths[1] = Vector3.Distance(points[0], points[1]);
			length = lengths[1];
		}
		else
		{
			for(int i = 1; i < count; i ++)
			{
				Vector3 p1 = points[i - 1];
				Vector3 p2 = points[i];

				float distance = Vector3.Distance(p1, p2);
				lengths[i] = distance;
				length += distance;
			}
		}

		rates = new float[count];
		sumrates = new float[count];
		float r = 0;
		for(int i = 0; i < count; i ++)
		{
			rates[i] = lengths[i] / length;
			r += rates[i];
			sumrates[i] = r;
		}
	}

	private int GetBeginIndex(float t)
	{
		for(int i = 0; i < count - 1; i ++)
		{
			if(t < sumrates[i + 1])
			{
				return i;
			}
		}
		return count - 1;
	}


	public Vector3 Get(float t)
	{
		Init();

		if(t <= 0) return points[0];
		if(t >= 1) return points[count - 1];
		int begionIndex = GetBeginIndex(t);
		Vector3 p0 = points[begionIndex];
		Vector3 p1 = points[begionIndex + 1];

		float rate = (t - sumrates[begionIndex]) / rates[begionIndex + 1];
		return p0 + (p1 - p0) * rate;
	}
}

