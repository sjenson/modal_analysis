using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// parses text file into modal model
/// </summary>

public class ModalModel
{
	#region public vars
	public int nVertices;
	public int nTriangles;
	public int nModes;
	public List<Vector3> vertices;
	public int[] triangles;
	public double[] freqs;
	public double[] damps;
	public double[][] gain;

	//from symphony.pdf
	public Vector3[] f;
	public double[] c;
	public double[] m;
	public double[] g;
	#endregion

	#region parsing
	public ModalModel(String file) //constructor
	{
		Debug.Log ("modal model parsing begun");
		//i guess this is really where i should instantiate stuff

		StreamReader sr = null;
		sr = new StreamReader (file);
		if (sr == null) {
			Debug.LogError ("problem reading the file");
		}

		//read the file
		using (sr)
		{

			//local vars
			String line;
			int j;
			double d;

			//parsing
			//nVertices
			if (Int32.TryParse(sr.ReadLine(), out j))
			{
				this.nVertices = j;
			}
			else
			{
				Debug.LogError("ParsingError-->nVertices");
			}

			//nTriangles
			if (Int32.TryParse(sr.ReadLine(), out j))
			{
				this.nTriangles = j;
			}
			else
			{
				Debug.LogError("ParsingError-->nTriangles");
			}

			//nModes
			if (Int32.TryParse(sr.ReadLine(), out j))
			{
				this.nModes = j;
			}
			else
			{
				Debug.LogError("ParsingError-->nModes");
			}



			//parse the vertices
			//	lines of 4 vertices
			int i = 0;
			String[] ss;
			this.vertices = new List<Vector3>();
			float x, y, z, weight;
			for(i = 0; i < this.nVertices; i++){
				line = sr.ReadLine();
				ss = line.Split(' ');

				//the fourth var is the vertex, weight - for now I am ignoring

				float.TryParse (ss [0], out x);
				float.TryParse (ss [1], out y);
				float.TryParse (ss [2], out z);
				float.TryParse (ss [3], out weight);
				this.vertices.Add(new Vector3(x, y, z));
			}

			Debug.Log ("parsed vertices");

			//parse the triangles
			//	one tri per line
			this.triangles = new int[this.nTriangles*3];
			i = 0;
			int triCount = 0;
			while(triCount<this.nTriangles){
				// skip for now by just reading lines
				line = sr.ReadLine();
				ss = line.Split(' ');
				int a, b, c;
				if (int.TryParse (ss [0], out a) != true){
					Debug.LogError ("error parsing triangle" + triCount);	
				}
				if (int.TryParse (ss [1], out b) != true){
					Debug.LogError ("error parsing triangle" + triCount);	
				}
				if (int.TryParse (ss [2], out c) != true){
					Debug.LogError ("error parsing triangle" + triCount);	
				}
					
				this.triangles[i] = a;
				i++;

				this.triangles[i] = b;
				i++;

				this.triangles[i] = c;
				i++;

				triCount++;
			}

			Debug.Log("parsed triangles");


			//parse the freqs
			this.freqs = new double[this.nModes];
			for (i = 0; i < this.nModes; i++) {
				if (Double.TryParse(sr.ReadLine(), out d))
				{
					this.freqs[i] = d;
				}
				else
				{
					Debug.LogError("ParsingError-->freq_"+i);
				}
			}

			Debug.Log("parsed freqs");

			//parse the damps
			this.damps = new double[this.nModes];
			for (i = 0; i < this.nModes; i++) {
				if (Double.TryParse(sr.ReadLine(), out d))
				{
					this.damps[i] = d;
				}
				else
				{
					Debug.LogError("ParsingError-->damps_"+i);
				}
			}

			Debug.Log("parsed damps");

			//TODO 
			//  parse gains
			//	how the heck??


		}
		//if (f.Length != d.Length && d.Length != a.Length) Debug.LogError("inconsistent list length for f, d, and a");
	
		f = new Vector3[this.nVertices];
		c = new double[this.nVertices];
		g = new double[this.nVertices];
		m = new double[this.nVertices];


		Debug.Log ("end of model parsing");
	}
	#endregion
}
