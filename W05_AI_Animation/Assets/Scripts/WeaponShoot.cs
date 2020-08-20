using UnityEngine;
using System.Collections;

public class WeaponShoot : MonoBehaviour {

    // Used to Create projectile
    public Rigidbody projectile;

    // Used to keep track of how much ammo there is
    public int ammo;

    // Used to position the bullet once spawned
    public Transform projectileSpawnPoint;

    // Used to apply force to the bullet being fired
    public float projectileForce;

	// Use this for initialization
	void Start () {

        if (ammo <= 0)
        {
            // Set the ammo count to 20
            ammo = 20;
        }

        if (projectileForce <= 0)
        {
            // Set the bullet force
            projectileForce = 3.0f;
        }
	}

    public int Shoot()
    {
        // Check if there is enough ammo
        if (projectile && ammo > 0)
        {
            // Create the bullet if there is enough ammo
            Rigidbody temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as Rigidbody;

            // Add the force to fire the bullet
            temp.AddForce(transform.forward * projectileForce, ForceMode.Impulse);

            // Remove one ammo count
            ammo--;
        }
        // Do something if there isnt enough ammo
        else
        {
            // Play audio for reload

            // Print message
            Debug.Log("Reload");
        }

        return ammo;
    }
}
