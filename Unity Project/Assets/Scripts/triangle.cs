using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triangle {

	//three vertex indices
	private int a;
	private int b;
	private int c;

	//constructor
	public triangle(int a, int b, int c){
		this.a = a;
		this.b = b;
		this.c = c;
	}

	public string getVals(){
		return(a.ToString()+" "+b.ToString()+" "+c.ToString());
	}
}
