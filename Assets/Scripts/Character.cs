using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {

    public float speed = 6.0f;
    public float jumpSpeed = 5.0f;
    public float rotateSpeed = 5.0f;
    public float gravity = 9.8f;
    public int type;
 
    Vector3 moveDirection = Vector3.zero;
    CharacterController cc;
    public Rigidbody[] projectilePrefab = new Rigidbody[3];
    public Transform projectileSpawnPoint, holderSpawnPoint;
    public float fireSpeed = 10.0f;
    bool hasProjectile, canGet, canMove, canCollect, moving = false;
    GameObject Target;
    int[] ProjectileList = new int[3];
    int projectileCounter = 0;
    int currentFire = 0;
    string lastCollide;
    
    int playerScore = 0;

	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
        if (!cc)
            Debug.Log("Character Controller does not exist.");
        Debug.Log("CONTROLS: \n"
            + "MOVE: Arrow Keys \n"
            + "Jump: space \n"
            + "Shoot: left ctrl \n"
            + "Switch Bullet: 1, 2, or 3 \n"
            + "Collect Object: z \n"
            + "Carry Object/Put Object Down: x");
	}
	
	// Update is called once per frame
	void Update () {

       if (cc.isGrounded){
            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
        }
        InputManager();
        moveDirection.y -= gravity * Time.deltaTime;
        cc.Move(moveDirection * Time.deltaTime);
	}

    void PickUpObject(){
        if (Target.name != lastCollide){
            lastCollide = Target.name;
            Destroy(Target);
            projectileCounter = projectileCounter + 1;
            currentFire = projectileCounter;
            hasProjectile = true;
            ProjectileList[projectileCounter - 1] = (int)char.GetNumericValue(Target.name[16]) - 1;
            //Debug.Log("PC:" + projectileCounter);
            Debug.Log("Item Grabbed and stored in slot " + currentFire);
        }
    }

    void SwapPickUpObject(){
        if (Target.name != lastCollide) {
            lastCollide = Target.name;
            Destroy(Target);
            ProjectileList[currentFire - 1] = (int)char.GetNumericValue(Target.name[16]) - 1;
            Debug.Log("Swapped:" + currentFire);
        }
    }

    void InputManager(){
        //-------------JUMP---------------
        if (Input.GetButtonDown("Jump") && cc.isGrounded )
            moveDirection.y = jumpSpeed;

        //-------------SWAP PROJECTILE---------------
        if (Input.GetButtonDown("FirstSlot") & !(projectileCounter < 1))
            currentFire = 1;
        if (Input.GetButtonDown("SecondSlot") & !(projectileCounter < 2))
            currentFire = 2;
        if (Input.GetButtonDown("ThirdSlot") & !(projectileCounter < 3))
            currentFire = 3;

        //-------------SHOOT---------------
        if (Input.GetButton("Shoot") && hasProjectile){
            Debug.Log(ProjectileList[currentFire - 1]);
            if (projectilePrefab[ProjectileList[currentFire - 1]]) {
                Rigidbody temp = Instantiate(projectilePrefab[ProjectileList[currentFire - 1]], projectileSpawnPoint.position, projectileSpawnPoint.rotation) as Rigidbody;
                temp.AddForce(transform.forward * fireSpeed, ForceMode.Impulse);
            }
            else
                Debug.Log("No prefab found.");
        }

        //-------------PICK UP PROJECTILE---------------
        if (Input.GetButtonDown("Pickup")){
            if (canGet && projectileCounter < 3)
                PickUpObject();
            else if (canGet && projectileCounter >= 3)
                SwapPickUpObject();
        }

        if (Input.GetButtonDown("Grab"))
        {
            if (!moving && canMove)
                moving = true;
            else
                moving = false;
        }

        if (moving)
        {
            Debug.Log("Moving");
            Vector3 targetPosition = holderSpawnPoint.position;
            Vector3 currentPosition = Target.transform.position;
            if (Vector3.Distance(currentPosition, targetPosition) > 0.1f)
            {
                Vector3 directionOfTravel = targetPosition - currentPosition;
                directionOfTravel.Normalize();

                    Target.transform.Translate(
                        (directionOfTravel.x * 50 * Time.deltaTime),
                        0,
                        (directionOfTravel.z * 50 * Time.deltaTime),
                        Space.World);
            }
        }
        if (canCollect)
        {
            Destroy(Target);
            playerScore += 50;
            Debug.Log("Score: " + playerScore);
            canCollect = false;
        }
    }

    void OnTriggerEnter(Collider other){
           switch (other.gameObject.tag)
            {
                case "PickUp":
                    canGet = true;
                    canMove = false;
                    Target = other.gameObject;
                    break;
                case "Moveable":
                    canMove = true;
                    canGet = false;
                    Target = other.gameObject;
                    break;
                case "Collectable":
                    canCollect = true;
                    Target = other.gameObject;
                    break;
                default:
                    break;
        }
    }


}
