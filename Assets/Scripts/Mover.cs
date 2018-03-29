using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour {

	public Vector3 position;
	public Vector3 acceleration;
	public Vector3 velocity;
	public Vector3 heading;

	public ObjectManager objectManager;

	public float mass;
	public float lookAhead;
	public float maxSpeed;
	public float maxTurn;
	public float radius;

	protected virtual void Start () {
		position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		acceleration = Vector3.zero;
		velocity = Vector3.zero;
		heading = transform.forward;
	}

	protected abstract void CalcSteeringForces ();

	protected void ApplyForce(Vector3 force) {
		acceleration += force / mass;
	}

	void UpdatePosition() {
		velocity += acceleration;
		velocity = VectorHelper.Clamp(velocity, maxSpeed);

		position += velocity * Time.deltaTime;

		heading = velocity.normalized;

		acceleration = Vector3.zero;
	}

	void SetTransform() {
		this.transform.SetPositionAndRotation (position, VectorHelper.QuatFromUnit(heading));
	}

	// Update is called once per frame
	void Update () {
		CalcSteeringForces ();
		UpdatePosition ();
		Wrap ();
		SetTransform ();
	}

	/**
	 * Avoids the closes obstacle that is in front and dangerous.
	 **/
	public Vector3 AvoidObstacles() {
		Obstacle closest = null;
		float minDist = float.MaxValue;

		// iterate over obstacles and find the closest one that is in front of this mover.
		foreach (Obstacle obstacle in objectManager.obstacles) {
			if (obstacle.IsInFrontOf (this)) {
				float dist = obstacle.ToObstacle (this).sqrMagnitude;
				if (dist < minDist) {
					minDist = dist;
					closest = obstacle;
				}
			}
		}

		// if an obstacle was found and it's dangerous
		// - apply a turning force based on the sign of the left/right differentiating dot product
		// - slow down!
		if (closest != null && closest.IsDangerousTo(this)) {
			Vector3 left = VectorHelper.Perpindicularize (heading);
			float dot = Vector3.Dot (left, closest.ToObstacle (this));
			float direction = dot < 0 ? 1 : -1;

			Vector3 forceTurn = left * this.maxSpeed * direction;
			Vector3 forceBrake = this.velocity * -1.5f;

			return forceTurn + forceBrake;
		}
		return Vector3.zero;
	}

	/**
	 * Seek returns a steering vector to steer towards provided target.
	 **/
	public Vector3 Seek(Vector3 target) {
		Vector3 desiredVelocity = target - position;
		desiredVelocity = desiredVelocity.normalized * maxSpeed;

		return desiredVelocity;
	}

	/**
	 * Wrap sends us from one edge to the opposite edge when leaving the screen.
	 **/
	void Wrap() {
		// calculate this once, instead of every if statement
		Vector3 screenPos = Camera.main.WorldToScreenPoint (position);

		if (screenPos.y < 0) {
			// bottom wrap
			position = Camera.main.ScreenToWorldPoint (new Vector3 (screenPos.x, Screen.height, 0));
		}
		else if (screenPos.y > Screen.height) {
			// top wrap
			position = Camera.main.ScreenToWorldPoint (new Vector3 (screenPos.x, 0, 0));
		}
		else if (screenPos.x > Screen.width) {
			// right wrap
			position = Camera.main.ScreenToWorldPoint (new Vector3 (0, screenPos.y, 0));
		}
		else if (screenPos.x < 0) {
			// left wrap
			position = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, screenPos.y, 0));
		}

		// zero out the z coordinate once for all checks
		position.z = 0;
	}
}
