using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    GameManager _gameManager;
    GameObject _player;


    float _enemyHealth = 100f;
    float _enemyMoveSpeed = 2f;
    Quaternion _targetRotation; // Rotation the enery should do to follow player movement
    bool _disableEnemy = false;
    Vector2 _moveDirection;




    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (!_gameManager.gameOver && !_disableEnemy)
        {
            MoveEnemy();
            RotateEnemy();
        }


    }



    void MoveEnemy()
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, _player.transform.position, _enemyMoveSpeed * Time.deltaTime);
        // gameObject.GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(gameObject.transform.position, _player.transform.position, _enemyMoveSpeed * Time.deltaTime));
    }


    void RotateEnemy()
    {
        // distance between the player and enemy
        _moveDirection = _player.transform.position - gameObject.transform.position;
        _moveDirection.Normalize();

        // 
        _targetRotation = Quaternion.LookRotation(Vector3.forward, _moveDirection);

        //Avoid unnecessary rotation
        if (gameObject.transform.rotation != _targetRotation)
        {
            // rotate
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, _targetRotation, 200 * Time.deltaTime);
        }
    }



    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Bullet")
        {

            StartCoroutine(Damaged());
            _enemyHealth -= 50;

            if (_enemyHealth <= 0)
            {
                // destroy enemy
                Destroy(gameObject);
            }

            // destroy bullet
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "Player")
        {
            _gameManager.gameOver = true;
            col.gameObject.SetActive(false);
        }

    }

    IEnumerator Damaged()
    {

        _disableEnemy = true;

        yield return new WaitForSeconds(1);

        _disableEnemy = false;



    }

}
