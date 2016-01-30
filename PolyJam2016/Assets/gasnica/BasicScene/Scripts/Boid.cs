using UnityEngine;
using System.Collections.Generic;

// Class taken from Processing3's flocking example

public class Boid : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
	
	}



  Vector3 velocity;
  Vector3 acceleration;
  float r;
  float maxforce;    // Maximum steering force
  float maxspeed;    // Maximum speed

  public void Start() {
    acceleration = new Vector3(0, 0);

    // This is a new Vector3 method not yet implemented in JS
    // velocity = Vector3.random2D();

    // Leaving the code temporarily this way so that this example runs in JS
    float angle = Random.Range(0, Mathf.PI * 2);
    velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));

    r = 2.0f;
    maxspeed = 2;
    maxforce = 0.03f;
  }

  public void run(List<Boid> boids) {
    flock(boids);
    update();
  }

  void applyForce(Vector3 force) {
    // We could add mass here if we want A = F / M
    acceleration += force; // add (fixed) delta time here?!
  }

  // We accumulate a new acceleration each time based on three rules
  void flock(List<Boid> boids) {
    Vector3 sep = separate(boids);   // Separation
    Vector3 ali = align(boids);      // Alignment
    Vector3 coh = cohesion(boids);   // Cohesion
    // Arbitrarily weight these forces
    sep *= 1.5f;
    ali *= 1.0f;
    coh *= 1.0f;
    // Add the force vectors to acceleration
    applyForce(sep);
    applyForce(ali);
    applyForce(coh);
  }

  // Method to update transform.position
  void update() {
    // Update velocity
    velocity += acceleration;
    velocity.y = 0;
    // Limit speed
    velocity = Vector3.ClampMagnitude(velocity, maxspeed);
    transform.position += velocity;// * Time.deltaTime;
    // Reset accelertion to 0 each cycle
    acceleration = Vector3.zero;
  }

  // A method that calculates and applies a steering force towards a target
  // STEER = DESIRED MINUS VELOCITY
  Vector3 seek(Vector3 target) {
    Vector3 desired = target - transform.position;  // A vector pointing from the transform.position to the target
    // Scale to maximum speed
    desired.Normalize();
    desired *= maxspeed;

    // Above two lines of code below could be condensed with new Vector3 setMag() method
    // Not using this method until Processing.js catches up
    // desired.setMag(maxspeed);

    // Steering = Desired minus Velocity
    Vector3 steer = desired - velocity;
    steer = Vector3.ClampMagnitude(steer, maxforce);  // Limit to maximum steering force
    return steer;
  }

  // Separation
  // Method checks for nearby boids and steers away
  Vector3 separate (List<Boid> boids) {
    float desiredseparation = 25.0f;
    Vector3 steer = new Vector3(0, 0, 0);
    int count = 0;
    // For every boid in the system, check if it's too close
    foreach (Boid other in boids) {
      float d = Vector3.Distance(transform.position, other.transform.position);
      // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
      if ((d > 0) && (d < desiredseparation)) {
        // Calculate vector pointing away from neighbor
        Vector3 diff = transform.position - other.transform.position;
        diff.Normalize();
        diff /= d;        // Weight by distance
        steer += diff;
        count++;            // Keep track of how many
      }
    }
    // Average -- divide by how many
    if (count > 0) {
      steer /= (float)count;
    }

    // As long as the vector is greater than 0
    if (steer.magnitude > 0) {
      // First two lines of code below could be condensed with new Vector3 setMag() method
      // Not using this method until Processing.js catches up
      // steer.setMag(maxspeed);

      // Implement Reynolds: Steering = Desired - Velocity
      steer.Normalize();
      steer *= maxspeed;
      steer -= velocity;
      steer = Vector3.ClampMagnitude(steer, maxforce);
    }
    return steer;
  }

  // Alignment
  // For every nearby boid in the system, calculate the average velocity
  Vector3 align (List<Boid> boids) {
    float neighbordist = 50;
    Vector3 sum = new Vector3(0, 0);
    int count = 0;
    foreach (Boid other in boids) {
      float d = Vector3.Distance(transform.position, other.transform.position);
      if ((d > 0) && (d < neighbordist)) {
        sum += other.velocity;
        count++;
      }
    }
    if (count > 0) {
      sum /= (float)count;
      // First two lines of code below could be condensed with new Vector3 setMag() method
      // Not using this method until Processing.js catches up
      // sum.setMag(maxspeed);

      // Implement Reynolds: Steering = Desired - Velocity
      sum.Normalize();
      sum *= maxspeed;
      Vector3 steer = sum -  velocity;
      steer = Vector3.ClampMagnitude(steer, maxforce);
      return steer;
    } 
    else {
      return new Vector3(0, 0);
    }
  }

  // Cohesion
  // For the average transform.position (i.e. center) of all nearby boids, calculate steering vector towards that transform.position
  Vector3 cohesion (List<Boid> boids) {
    float neighbordist = 50;
    Vector3 sum = new Vector3(0, 0);   // Start with empty vector to accumulate all transform.positions
    int count = 0;
    foreach (Boid other in boids) {
      float d = Vector3.Distance(transform.position, other.transform.position);
      if ((d > 0) && (d < neighbordist)) {
        sum += other.transform.position; // Add transform.position
        count++;
      }
    }
    if (count > 0) {
      sum /= count;
      return seek(sum);  // Steer towards the transform.position
    } 
    else {
      return new Vector3(0, 0);
    }
  }
}