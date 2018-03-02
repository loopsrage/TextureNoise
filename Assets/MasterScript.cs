using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour {

    public static MasterScript master;
	// Use this for initialization
	void Start () {
        if (master != this)
        {
            master = this;
            DontDestroyOnLoad(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
