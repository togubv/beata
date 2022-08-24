using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private Core core;
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

        core.UpdateGameSpeedHandlerEvent += UpdateSpeed;
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

    private bool IsGame()
    {
        return core.gameState == GameState.Game;
    }

    private void FixedUpdate()
    {
        rb.velocity = trans.forward * speed;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<Quad>(out Quad quad))
        {
            if (quad.GetQuadType == QuadType.Empty)
            {
                core.AddScore(quad.Take());
            }

            if (quad.GetQuadType == QuadType.None)
            {
                core.BreakMultiplier(quad.Take());
                quad.ReturnToPool();
            }

            if (quad.GetQuadType == QuadType.Finish)
            {
                core.FinishLevel();
                quad.ReturnToPool();
            }
        }
    }
}
