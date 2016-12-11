using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

	[Header("Initial node")]
	public Node nodeStart;
	[Header("Final node")]
	public Node nodeEnd;

	[Header("List of positions inbetween")]
	public List<Vector2> positions;

	[Header("Is the path visible?")]
	public bool isDiscovered;

	public Material mat;

	List<Vector3> vs;
	List<int> order;
	Mesh mesh;

	// Use this for initialization
	public void StartComponent () {
		vs = new List<Vector3>();
		order = new List<int>();
		mesh = new Mesh();
		UpdateMesh();
	}
	
	// Update is called once per frame
	public void UpdateComponent () {
		//If path is not visible, we disable mesh render on start and end nodes and not draw the mesh path
		if (isDiscovered) {
			nodeStart.isVisible= true;
			nodeEnd.isVisible = true;

			UpdateMesh ();
			Graphics.DrawMesh (mesh, transform.localToWorldMatrix, mat, 0);
		} else {
		}

	}

	void UpdateMesh(){
		vs.Clear();

		vs.Add (nodeStart.transform.localPosition);
		for(int i=0;i<positions.Count;i++){
			new Vector3 (positions[i].x, positions[i].y);
			vs.Add(new Vector3 (positions[i].x, positions[i].y));
		}
		vs.Add(nodeEnd.transform.localPosition);

		order.Clear();

		for(int i=0;i<vs.Count;i++){
			order.Add(i);
		}

		mesh.SetVertices(vs);
		mesh.SetIndices(order.ToArray(),MeshTopology.LineStrip,0);
	}
}
