using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorHelper : MonoBehaviour {

	/**
	 * Returns a new vector with a magnitude less than or equal to maxMagnitude.
	 **/
	public static Vector3 Clamp(Vector3 original, float maxMagnitude) {
		if (original.sqrMagnitude > maxMagnitude * maxMagnitude) {
			return original.normalized * maxMagnitude;
		}
		return new Vector3 (original.x, original.y, original.z);
	}

	/**
	 * Computes a vector that is perpindicular to the left of input.
	 **/
	public static Vector3 Perpindicularize(Vector3 input) {
		return new Vector3 (-input.y, input.x, 0);
	}

	/**
	 * Computes a quaternion from a 2D (xy-plane) unit vector.
	 **/
	public static Quaternion QuatFromUnit(Vector3 unit) {
		float angle = Mathf.Atan2 (unit.y, unit.x) * Mathf.Rad2Deg;
		return Quaternion.AngleAxis (angle, Vector3.forward);
	}
}
