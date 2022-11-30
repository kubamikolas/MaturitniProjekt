using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float mouseSensitivityX = 3f;

    [SerializeField]
    private float mouseSensitivityY = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Spring settings:")]
	[SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
	private float jointSpring = 20f;
	[SerializeField]
	private float jointMaxForce = 40f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;


    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);
    }

    private void Update()
    {
        // Vypočítárychlost (rychlost) pohybu našeho hráče
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);

       // Vypočítáme rotaci hráče ve Vector3
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;

        motor.Rotate(rotation);

        // Vypočítáme rotaci kamery ve Vector3

        float xRot = Input.GetAxisRaw("Mouse Y");

      float cameraRotationX = xRot * mouseSensitivityY;

        motor.RotateCamera(cameraRotationX);

       //Vypočítáváme thrusterforce na základě, jestli jsme něco zmáčkli
        Vector3 _thrusterForce = Vector3.zero;       
        //Aplikovat truster force
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        } else
        {
            SetJointSettings(jointSpring);
        }

        motor.ApplyThruster (_thrusterForce);
    }

    private void SetJointSettings (float _jointSpring)
    {
        joint.yDrive = new JointDrive{ 
        mode = jointMode,
        positionSpring = jointSpring, 
        maximumForce = jointMaxForce
        };
    }
}
