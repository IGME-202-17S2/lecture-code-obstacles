using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public float radius = 1f;

	void Start() {
		transform.localScale = new Vector3 (0.27f * radius, 0.27f * radius, 0.27f * radius);
	}

	/**
	 * Helper method for getting the vector from Mover to Obstacle
	 **/
	public Vector3 ToObstacle(Mover mover) {
		return transform.position - mover.transform.position;
	}

	/**
	 * Uses dot product of (Mover's forward unit vector) and (ToObstacle)
	 * to determine if obstacle is in front of Mover
	 **/
	public bool IsInFrontOf(Mover mover) {
		Vector3 forward = mover.heading;
		float dot = Vector3.Dot (forward, ToObstacle (mover));
		return dot > 0;
	}

	/**
	 * Uses dot product of (Mover's side unit vector) and (ToObstacle)
	 * Compares it to (Obstacle radius) + (Mover's radius) + (a little extra spacing)
	 * to determin if Mover is on a collision course with Obstacle
	 **/
	public bool IsDangerousTo(Mover mover) {
		Vector3 left = VectorHelper.Perpindicularize (mover.heading);
		float dot = Vector3.Dot (left, ToObstacle (mover));

		bool onCollisionCourse = Mathf.Abs (dot) < radius + mover.radius + 0.125f;
		bool isCloseEnough = dot < mover.lookAhead;

		return isCloseEnough && onCollisionCourse;
	}
}
