using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Game game;

    [SerializeField] private VariableJoystick variableJoystick;

    private Camera main_camera;
    private Rigidbody rb;
    private BoxCollider boxcollider;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        main_camera = Camera.main;
    }

    private void Update()
    {
        if (IsGame() && variableJoystick.Vertical > 0 && transform.position.y < 1)
                transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        if (IsGame() && variableJoystick.Vertical < 0 && transform.position.y > 0)
                transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);

        if (IsGame() && variableJoystick.Horizontal < 0 && transform.position.x > 0)
                transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);

        if (IsGame() && variableJoystick.Horizontal > 0 && transform.position.x < 1)
                transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);


        main_camera.transform.position = new Vector3(main_camera.transform.position.x, main_camera.transform.position.y, transform.position.z - 4.0f);
    }

    bool IsGame()
    {
        if (game.gameState == GameState.Game)
        {
            return true;
        }
        else return false;
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * game.Speed;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("QuadEmpty"))
        {
            game.AddScore(collider.gameObject.GetComponent<Quad>().position);
            StartCoroutine(CollisionCooldown());
        }

        if (collider.CompareTag("Quad"))
        {
            game.ZeroingMultiplier(collider.gameObject.GetComponent<Quad>().position);
            StartCoroutine(CollisionCooldown());
        }

        if (collider.CompareTag("QuadFinish"))
        {
            game.FinishLevel();
            Destroy(collider.gameObject);
        }
    }

    IEnumerator CollisionCooldown()
    {
        boxcollider.enabled = false;
        yield return new WaitForSeconds(0.15f);
        boxcollider.enabled = true;
    }
}
