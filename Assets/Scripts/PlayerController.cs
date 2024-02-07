using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameManager _gameManager;

    // player's varaibles
    Rigidbody2D _rigidbody;
    Camera _mainCamera;

    // movement variables
    float _moveVertical;
    float _moveHorizontal;
    float _moveSpeed = 5f;
    float _speedLimiter = 0.7f;
    Vector2 _moveVelocity;

    // mouse's variables
    Vector2 _mousePosition;
    Vector2 _mouseTankOffset;

    // shotting variables
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawn;
    bool _isShooting = false; // true if the tank is shooting
    float _bulletSpeed = 15f;


    void Start()
    {
        // set rigidbody and main camera
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // get axis
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");

        // convert the axis to vector2
        _moveVelocity = new Vector2(_moveHorizontal, _moveVertical) * _moveSpeed;

        // check mouse click
        if (Input.GetMouseButtonDown(0))
        {
            _isShooting = true;
        }

    }


    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();

        if (_isShooting)
        {
            StartCoroutine(Fire());
        }
    }



    // basic movement code
    void MovePlayer()
    {

        // check horizontal and vertical axis
        if (_moveHorizontal != 0 || _moveVertical != 0)
        {

            // to prevent moving faster when pressing diagonal buttons 
            if (_moveHorizontal != 0 && _moveVertical != 0)
            {
                _moveVelocity *= _speedLimiter;
            }

            // move player
            _rigidbody.velocity = _moveVelocity;
        }
        else
        {

            // if no movement, then stop player;
            _moveVelocity = new Vector2(0, 0);
            _rigidbody.velocity = _moveVelocity;

        }

    }


    // rotate player based on mouse position
    void RotatePlayer()
    {

        // get mouse position
        _mousePosition = Input.mousePosition;
        // get location of the player inside the camera view
        //in other words, get the player location based on the camera position
        // this step is mandatory to rotate the player at same rate no matter how far the mouse is
        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(gameObject.transform.localPosition);

        // distance between the player and the mouse
        _mouseTankOffset = new Vector2(_mousePosition.x - screenPoint.x, _mousePosition.y - screenPoint.y).normalized;
       
        // calculate the angle required to rotate the player
        // float angle = Mathf.Atan2(_mouseTankOffset.y, _mouseTankOffset.x) * Mathf.Rad2Deg;


        // rotate player towards the mouse
        // transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // rotate player towards the mouse
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _mouseTankOffset);


    }

    IEnumerator Fire()
    {
        _isShooting = false;

        // instantiate bullet
        GameObject cloneBullet = Instantiate(bullet, bulletSpawn.transform.position, Quaternion.identity);

        // "shoot" bullet towards the mouse position
        Rigidbody2D bulletRb = cloneBullet.GetComponent<Rigidbody2D>();

        bulletRb.velocity = _mouseTankOffset * _bulletSpeed;

        // wait 3 seconds
        yield return new WaitForSeconds(3);

        // destoy game objectw
        Destroy(cloneBullet);
    }


}
