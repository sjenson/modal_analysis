using UnityEngine;
using System;

public class modalObj : MonoBehaviour {

	//TODO might have to build the mesh using a file which sage also uses for precomputation
	//to keep the mesh indicies consistent 

	#region public vars
	public string modalDataFileName;
	#endregion


	#region private vars
	//modal model
	private ModalModel modalModel;
	private int point = 0;

	//physics stuff
	private Vector3 F; //force of collision
	private double tpi = 2 * Math.PI; //2pi

	//audio stuff
	private float[] buffer;
	private int samplerate;
	private double t = 0;
	private double K = 0;

	//components
	private MeshFilter mf;
	private Mesh mesh;
	private MeshCollider coll;
	private Rigidbody rb;
	#endregion


	// Use this for initialization
	void Awake () {

		Debug.Log ("Awake");

		//instantiate stuff
		this.mf = GetComponent<MeshFilter> ();
		this.coll = GetComponent<MeshCollider> ();
		this.rb = GetComponent<Rigidbody> ();

		//load the modal model
		this.modalDataFileName = "./Assets/ModalData/" + this.modalDataFileName;
		this.modalModel = new ModalModel(this.modalDataFileName);

		//debug the results of loading the model
		Debug.Log ("n Vertices: "+ this.modalModel.nVertices+"\n"+
					"n Triangles: " + this.modalModel.nTriangles+"\n"+
					"n Modes: " + this.modalModel.nModes);

		//build the mesh with this.modalModel
		this.mesh = new Mesh();
		mesh.SetVertices (this.modalModel.vertices);
		mesh.triangles = this.modalModel.triangles;
		mesh.RecalculateNormals ();

		//check to see if the indexing is the same
		//TRUE! :)
		bool sameIndex = true;
		for (int i = 0; i < modalModel.nVertices; i++) {
			if (modalModel.vertices [i] != mesh.vertices [i]) {
				sameIndex = false;
			}
		}
		Debug.Log ("same indexing: " + sameIndex);

		//set the meshfilter and mesh collider Components to the created mesh
		this.mf.mesh = this.mesh;
		this.coll.sharedMesh = this.mesh;

		//setup audio
		this.samplerate = AudioSettings.outputSampleRate;
	}


	#region collision
	void OnCollisionEnter(Collision collision) {
		//when something hits our object 

		//get the force of the impact
		this.F = collision.impulse / Time.fixedDeltaTime;		//see:https://docs.unity3d.com/ScriptReference/Collision-impulse.html
		//time
		this.K = t;
		//assume 1 contact point, get the index of the closest vertex
		this.point = this.closestVertexIndex (collision.contacts [0].point);	
		Debug.Log ("strike!");
	}

	void OnCollisionStay(Collision collision){
		//some way to deal with a sound moving against a surface
		//this is what i had before
		this.F = this.F / 0.5f;
	}

	void OnCollisionExit(Collision collision){
		this.K = this.t;
	}
	#endregion

	/// Closest Vector3 vertex to the provided Vector3
	/// </summary>
	/// <returns>The vertex index.</returns>
	/// <param name="contactPoint">Contact point.</param>
	int closestVertexIndex(Vector3 contactPoint){

		Debug.Log ("closest vertex");

		int smallestDistance = 0; //if only 1 vertex then that's the smallest
		float temp;
		for (int i = 0; i < mesh.vertexCount; i++) {
			temp = Vector3.Distance (contactPoint, transform.TransformPoint(mesh.vertices [i]));
			if (temp < smallestDistance) {
				smallestDistance = i;
			}
		}
		return smallestDistance;
	}


	void OnAudioFilterRead(float[] data, int channels){

		//write to the audio buffer
		for (int i = 0; i < data.Length; i += channels) {
			
			double temp = 0;
			//pre bake some variables
			double tk = t - K;
			double ci = 0;

			//make da sound
			for (int n = 0; n < modalModel.nModes; n++) {

				/// according to symphony.pdf i'm going to have something like this:
				/// where theres an array of c, g, m , d, f for each mode
				/// 	-> add to modal model where it doesn't already exist
				/// c[i] += g[i] / (m[i] (damps[i] - freqs[i]) * Math.Exp(damps[i] + t0))

				/// also need array f[nVertices] which contains Vector3 impulse forces
				/// for each vertex 

				// damped oscillators
				temp += (modalModel.gain [this.point][n]) *
					(Math.Exp (-modalModel.damps [n] * tk)) *
					(Math.Cos (tpi * tk * modalModel.freqs [n])
					* F.magnitude);
			}

			//write to audio buffer
			data [i] = (float)temp;
			if (channels == 2) data [i + 1] = data [i];
			t += 1D / this.samplerate;

		}
	}	
}