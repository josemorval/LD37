using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreation : MonoBehaviour {

	public Material mat;
	List<Vector3> vs;
	List<int> inds;
	Mesh mesh;

	void Start () {

		vs = new List<Vector3>();
		inds = new List<int>();
		mesh = new Mesh();

		UpdateMesh();
	}
	
	void Update () {
		Graphics.DrawMesh(mesh,transform.localToWorldMatrix,mat,0);
	}

	void UpdateMesh(){
		vs.Clear();

		for(int i=0;i<transform.childCount;i++){
			vs.Add(transform.GetChild(i).transform.localPosition);
		}

		inds.Clear();

		for(int i=0;i<vs.Count;i++){
			inds.Add(i);
		}

		mesh.SetVertices(vs);
		mesh.SetIndices(inds.ToArray(),MeshTopology.LineStrip,0);
	}
}
