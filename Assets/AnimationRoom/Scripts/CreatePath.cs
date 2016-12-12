using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreatePath : MonoBehaviour {

	public float widthLine;
	List<Vector2> uvs;
	List<Vector3> vs;
	List<Vector3> nodeTransforms;
	List<int> inds;
	Mesh mesh;


	void Start () {

		nodeTransforms = new List<Vector3>();
		vs = new List<Vector3>();
		inds = new List<int>();
		uvs = new List<Vector2>();
		mesh = new Mesh();

		UpdateMesh();
	}

	void Update () {
		UpdateMesh();
	}

	void UpdateMesh(){

		nodeTransforms.Clear();

		for(int i=0;i<transform.childCount;i++){
			nodeTransforms.Add(transform.GetChild(i).transform.position);
		}
			
		vs.Clear();
		uvs.Clear();

		Vector3 v0,v1 = Vector3.zero;

		v0 = nodeTransforms[1]-nodeTransforms[0];
		v0.Normalize();
		v0.z = v0.x;
		v0.x = v0.y;
		v0.y = -v0.z;
		v0.z = 0f;
		vs.Add(nodeTransforms[0]+widthLine*v0);
		vs.Add(nodeTransforms[0]-widthLine*v0);

		uvs.Add(new Vector2(0f,0f));
		uvs.Add(new Vector2(1f,0f));

		for(int i=1;i<nodeTransforms.Count-1;i++){
			
			v0 = nodeTransforms[i]-nodeTransforms[i-1];
			v0.Normalize();

			v1 = nodeTransforms[i+1]-nodeTransforms[i];
			v1.Normalize();


			v0 =v0+v1;
			v0.Normalize();
			v0.z = v0.x;
			v0.x = v0.y;
			v0.y = -v0.z;
			v0.z = 0f;



			vs.Add(nodeTransforms[i]+widthLine*v0);
			vs.Add(nodeTransforms[i]-widthLine*v0);

			uvs.Add(new Vector2(0f,1f*i/(nodeTransforms.Count-1)));
			uvs.Add(new Vector2(1f,1f*i/(nodeTransforms.Count-1)));

		}

		v0 = nodeTransforms[nodeTransforms.Count-1]-nodeTransforms[nodeTransforms.Count-2];
		v0.Normalize();
		v0.z = v0.x;
		v0.x = v0.y;
		v0.y = -v0.z;
		v0.z = 0f;
		vs.Add(nodeTransforms[nodeTransforms.Count-1]+widthLine*v0);
		vs.Add(nodeTransforms[nodeTransforms.Count-1]-widthLine*v0);

		uvs.Add(new Vector2(0f,1f));
		uvs.Add(new Vector2(1f,1f));

		inds.Clear();

		int k = 0;
		for(int i=0;i<nodeTransforms.Count-1;i++){
			inds.Add(k);
			inds.Add(k+1);
			inds.Add(k+3);

			inds.Add(k);
			inds.Add(k+3);
			inds.Add(k+2);

			k=k+2;
		}

		mesh.SetVertices(vs);
		mesh.SetUVs(0,uvs);
		mesh.SetIndices(inds.ToArray(),MeshTopology.Triangles,0);

		GetComponent<MeshFilter>().mesh = mesh;
	}
}
