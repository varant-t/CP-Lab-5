using UnityEngine;
using System.Collections;
using TMPro;

public class WeaponPickerUpper : MonoBehaviour
{

    public GameObject weapon;
    public GameObject weapon2; // Used to store the Weapon that gets picked up
    public GameObject weaponAttach;
    public GameObject weaponAttach2;// Used to place Weapon Object in Player
    public float weaponDropForce;    // Used to add a force when 'Character' hits something

    public bool isPrimaryWeapon;

    public TextMeshProUGUI ammoText;

    // Use this for initialization
    void Start()
    {
        // weapon should be left empty
        weapon = null;
        weapon2 = null;

        // Check if weaponAttach exists and was dragged into variable
        if (weaponAttach == null)
        {
            // Find the point to attach the weapon to
            // WeaponPlacement is an Empty GameObject used to connect weapon to
            weaponAttach = GameObject.Find("WeaponPlacement");
        }

        if (weaponAttach2 == null)
        {
            // Find the point to attach the weapon to
            // WeaponPlacement is an Empty GameObject used to connect weapon to
            weaponAttach2 = GameObject.Find("WeaponPlacement");
        }

        if (weaponDropForce <= 0)
        {
            weaponDropForce = 10.0f;

           // Debug.Log("WeaponDropForce not set on " + name + ". Defaulting to " + weaponDropForce);
        }

        ammoText.text = string.Empty;

    }

    // Update is called once per frame
    void Update()
    {

        // Drop weapon when 'T' is pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Is there a weapon to drop
            if (weapon != null)
            {
                // Remove weapon as a Child of Player
                weaponAttach.transform.DetachChildren();

                // Turn collision back on
                StartCoroutine(EnableCollisions(1.0f));

                // Turn Physics back on	
                weapon.GetComponent<Rigidbody>().isKinematic = false;

                // Throw Weapon forward
                weapon.GetComponent<Rigidbody>().AddForce(weapon.transform.forward * weaponDropForce, ForceMode.Impulse);

                ammoText.text = string.Empty;

            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            // Is there a weapon to drop
            if (weapon2 != null)
            {
                // Remove weapon as a Child of Player
                weaponAttach2.transform.DetachChildren();

                // Turn collision back on
                StartCoroutine(EnableCollisions(1.0f));

                // Turn Physics back on	
                weapon2.GetComponent<Rigidbody>().isKinematic = false;

                // Throw Weapon forward
                weapon2.GetComponent<Rigidbody>().AddForce(weapon2.transform.forward * weaponDropForce, ForceMode.Impulse);

                ammoText.text = string.Empty;

            }
        }

        if (Input.GetKeyDown("1") && !isPrimaryWeapon && weapon != null)
        {
            isPrimaryWeapon = true;
            weapon.SetActive(true);
            if (weapon2.transform.parent == weaponAttach.transform)
            {
                weapon2.SetActive(false);
            }
        }
        else if (Input.GetKeyDown("2") && isPrimaryWeapon && weapon2 != null)
        {
            isPrimaryWeapon = false;
            weapon2.SetActive(true);
            if (weapon.transform.parent == weaponAttach.transform)
            {
                weapon.SetActive(false);
            }

        }

     
        // Check if the Fire key was pressed
        if (Input.GetButtonDown("Fire1"))
        {
            // Check if there is weapon attached to the Player
            if (weapon != null)
            {
                // Grab WeaponScript to fire projectile
                WeaponShoot ws = weapon.GetComponent<WeaponShoot>();
                if (ws)
                {
                    ammoText.text = ws.Shoot().ToString();
                }
            }
            else if (weapon2 != null)
            {
                WeaponShoot ws = weapon2.GetComponent<WeaponShoot>();
                if (ws)
                {
                    ammoText.text = ws.Shoot().ToString();
                }
            }
        }

    }

    // Must set Collider to isTrigger to function
    void OnTriggerEnter(Collider other)
    {
        // Did the player collide with a weapon
        if (other.gameObject.CompareTag("Weapon"))
        {
            // Store a copy of Weapon


            // Stop applying Physics to Weapon	


            // Move Weapon to weaponAttach position


            // Make weaponAttach the parent of Weapon


            // Rotate it to the parent identity


            // Stop Collision between Player and Weapon

        }
    }

    // Does not work with the Character Controller
    void OnCollisionEnter(Collision c)
    {


    }

    // Used when working with a Character Controller to check for collision
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Did 'Player' collide with a GameObject tagged as "Weapon"
        if (weapon == null && hit.collider.CompareTag("Weapon"))
        {
            // Store a copy of Weapon
            weapon = hit.gameObject;

            // Stop applying Physics to Weapon	
            weapon.GetComponent<Rigidbody>().isKinematic = true;

            // Move Weapon to weaponAttach position
            weapon.transform.position = weaponAttach.transform.position;

            // Make weaponAttach the parent of Weapon
            weapon.transform.SetParent(weaponAttach.transform);

            // Rotate it to the parent identity
            weapon.transform.localRotation = weaponAttach.transform.localRotation;

            //weapon.transform.SetPositionAndRotation(weaponAttach.transform.position, weaponAttach.transform.localRotation);

            // Stop Collision between Player and Weapon
            Physics.IgnoreCollision(weapon.transform.GetComponent<Collider>(), transform.GetComponent<Collider>());


        }

        if (weapon2 == null && hit.collider.CompareTag("Weapon2"))
        {
            // Store a copy of Weapon
            weapon2 = hit.gameObject;

            // Stop applying Physics to Weapon	
            weapon2.GetComponent<Rigidbody>().isKinematic = true;

            // Move Weapon to weaponAttach position
            weapon2.transform.position = weaponAttach.transform.position;

            // Make weaponAttach the parent of Weapon
            weapon2.transform.SetParent(weaponAttach.transform);

            // Rotate it to the parent identity
            weapon2.transform.localRotation = weaponAttach.transform.localRotation;

            //weapon.transform.SetPositionAndRotation(weaponAttach.transform.position, weaponAttach.transform.localRotation);

            // Stop Collision between Player and Weapon
            Physics.IgnoreCollision(weapon2.transform.GetComponent<Collider>(), transform.GetComponent<Collider>());


        }

        //// Did 'Player' collide with a GameObject tagged as "Gurt"
        //if (hit.collider.tag == "Gurt")
        //{
        //    hit.gameObject.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * bodyCheckForce, ForceMode.Impulse);
        //}
    }

    IEnumerator EnableCollisions(float timeToDisable)
    {
        yield return new WaitForSeconds(timeToDisable);

        // Turn collision back on after timeToDisable seconds
        Physics.IgnoreCollision(weapon.transform.GetComponent<Collider>(), transform.GetComponent<Collider>(), false);

        // Reset weapon to null so a new weapon can be collected
        weapon = null;
    }
}

