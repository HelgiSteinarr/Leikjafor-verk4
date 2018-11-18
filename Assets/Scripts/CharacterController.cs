using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CharacterController : NetworkBehaviour {

    public float speed = 10.0f;
    public Camera playerCam;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 10.0f;
    public float shootDelay = 0.5f;
    public float jumpForce = 10.0f;

    private bool canShoot = true;
    private bool touchingGround = true;
    private Rigidbody rb;
    private TerrainScript terrainS;
    private Vector3 startPosition;

    // Use this for initialization
    private void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCam.enabled = isLocalPlayer;
        rb = GetComponent<Rigidbody>();
        terrainS = Terrain.activeTerrain.GetComponent<TerrainScript>();
        startPosition = rb.transform.localPosition;
    }

    // Update is called once per frame
    private void Update () {
        if (!isLocalPlayer) return;

        if (touchingGround && Controls.up.CreateHole)
        {
            terrainS.CreateHole(transform.position, 0.0f);
        }

        float extraSpeed = 0;
        if (Controls.hold.Sprint) extraSpeed = speed * 0.7f;

        float translation = Input.GetAxis("Vertical") * (speed + extraSpeed) * Time.deltaTime;
        float strafe = Input.GetAxis("Horizontal") * (speed + extraSpeed) * Time.deltaTime;
        transform.Translate(strafe, 0, translation);

        if (canShoot && Controls.hold.Shoot)
        {
            StartCoroutine("ShootCR");
        }
        if (touchingGround && Controls.down.Jump)
        {
            rb.AddForce(transform.up * jumpForce);
        }

        if (rb.transform.localPosition.y < -100.0f)
        {
            rb.transform.localPosition = startPosition;
        }

        if (Controls.down.Esc)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if(Controls.up.Quit)
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_WEBPLAYER
                        Application.OpenURL("about:blank");
            #else
                        Application.Quit();
            #endif
        }
    }

    private IEnumerator ShootCR()
    {
        canShoot = false;
        CmdShoot();
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    [Command]
    private void CmdShoot()
    {
        var bullet = Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * Time.deltaTime * bulletSpeed;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 5.0f);
    }

    /*private IEnumerator ShootCR()
    {
        canShoot = false;

        var bullet = Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * Time.deltaTime * bulletSpeed;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 5.0f);

        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "briefcase")
        {
            gameObject.transform.parent = collision.gameObject.transform;
        }
        if (collision.gameObject.tag == "ground")
        {
            touchingGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            touchingGround = false;
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
