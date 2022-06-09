using UnityEngine;

public class Player : MonoBehaviour
{
    public Game game;

    [SerializeField] private VariableJoystick variableJoystick;

    private Camera mainCamera;
    private Rigidbody rb;
    private Transform trans, cameraTrans;
    private float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        cameraTrans = mainCamera.gameObject.GetComponent<Transform>();
        trans = GetComponent<Transform>();

        game.UpdateGameSpeedHandlerEvent += UpdateSpeed;
    }

    private void Update()
    {
        if (IsGame())
        {
            Move();
            cameraTrans.position = new Vector3(cameraTrans.position.x, cameraTrans.position.y, trans.position.z - 4.0f);
        }
    }

    private void Move()
    {
        if (variableJoystick.Vertical > 0 && transform.position.y < 1)
        { 
            trans.position = new Vector3(trans.position.x, trans.position.y + 1, trans.position.z);
            return;
        }

        if (variableJoystick.Vertical < 0 && transform.position.y > 0)
        {
            trans.position = new Vector3(trans.position.x, trans.position.y - 1, trans.position.z);
            return;
        }

        if (variableJoystick.Horizontal < 0 && transform.position.x > 0)
        {
            trans.position = new Vector3(trans.position.x - 1, trans.position.y, trans.position.z);
            return;
        }

        if (variableJoystick.Horizontal > 0 && transform.position.x < 1)
        {
            trans.position = new Vector3(trans.position.x + 1, trans.position.y, trans.position.z);
            return;
        }
    }

    private void UpdateSpeed(float speed)
    {
        this.speed = speed;
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
        rb.velocity = trans.forward * speed;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("QuadEmpty"))
        {
            game.AddScore(collider.gameObject.GetComponent<IPosition>().Take());
        }

        if (collider.CompareTag("Quad"))
        {
            game.BreakMultiplier(collider.gameObject.GetComponent<IPosition>().Take());
            Destroy(collider.gameObject);
        }

        if (collider.CompareTag("QuadFinish"))
        {
            game.FinishLevel();
            Destroy(collider.gameObject);
        }
    }
}
