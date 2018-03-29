using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ObjectManager 
 * This component tracks collections of PhysicsObjects
 * It also orchestrates the forces and applies them.
 */
public class ObjectManager : MonoBehaviour {

	// for PhysicsObjects that already exist in the scene
	// (populated by dragging them into the inspector)
	public List<Obstacle> obstacles;

	// for PhysicsObjects generated at runtime
	// (populated with the GeneratePOs() call
	List<Mover> movers = new List<Mover>();

	// a reference to the prefab used for PhysicsObjects
	// (also populated in the inspector)
	public GameObject seekerPrefab;

	void Start () {
		GenerateSeekers ();
	}

	void GenerateSeekers() {
		for (int i = 0; i < 250; i++) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (
				new Vector3 (Random.Range (0, Screen.width), Random.Range (0, Screen.height), 0));
			pos.z = 0;
			GameObject go = Instantiate (seekerPrefab, pos, Quaternion.identity);
			Seeker seeker = go.GetComponent<Seeker> ();
			seeker.objectManager = this;
			movers.Add (seeker);
		}
	}
}
