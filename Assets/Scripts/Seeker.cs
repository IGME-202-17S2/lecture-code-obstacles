using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Mover {

	protected override void Start () {
		base.Start ();
		mass = 3f;
		lookAhead = 3f;
		maxSpeed = 1f;
		maxTurn = 0.25f;
		radius = 0.125f;
		transform.localScale = new Vector3 (0.27f * radius, 0.27f * radius, 0.27f * radius);
		velocity = new Vector3 (Random.Range (-3f, 3f), Random.Range (-3f, 3f), 0);
	}

	protected override void CalcSteeringForces ()
	{
		Vector3 steerForce = Vector3.zero;

		Vector3 avoidForce = AvoidObstacles ();

		if (avoidForce == Vector3.zero) {
			// no obstacle? keep going straight
			steerForce += heading;
		} else {
			// obstacle? avoid!!!
			steerForce += avoidForce;
		}

		steerForce = VectorHelper.Clamp (steerForce, maxTurn);

		ApplyForce (steerForce);
	}
}