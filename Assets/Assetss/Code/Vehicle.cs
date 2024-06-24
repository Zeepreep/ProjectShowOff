using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [Header("Wheels")]
    public WheelCollider[] frontWheels;
    public WheelCollider[] backWheels;
    public DriveBias driveBias;
    public SteerBias steerBias;

    [Header("Misc")]
    public GameObject steeringAim, steeringAim2;
    public MeshRenderer body;
    public Object wheel;
    [Range(20, 45)] public int steerAngle;
    public int power, brakeTorque, bodyMaterialIndex, randomID;
    public bool reverse;

    private RoadPoint previousTarget, currentTarget, futureTarget, stoppingPoint;
    private Rigidbody rigid;
    private GameObject tempWheel;
    private Ray ray;
    private RaycastHit hit;
    private float zVelocity;
    public bool stop;
    private bool isSoundPlaying;

    public enum DriveBias
    {
        BackWheelDrive,
        FrontWheelDrive,
        AllWheelDrive
    }

    public enum SteerBias
    {
        FrontWheelSteering,
        BackWheelSteering,
        AllWheelSteering
    }

    void Awake()
    {
        body.materials[bodyMaterialIndex] = new Material(body.materials[bodyMaterialIndex]);
        body.materials[bodyMaterialIndex].color = new Color(Random.Range(0.4f, 0.8f), Random.Range(0.4f, 0.8f), Random.Range(0.4f, 0.8f));

        randomID = Random.Range(10000, 99999);
        rigid = GetComponent<Rigidbody>();
        currentTarget = GetClosestObjectWithTag(gameObject, "RoadPoint");

        if (currentTarget != null)
        {
            previousTarget = currentTarget;
            SetFutureTarget();
        }

        isSoundPlaying = false; // Initialize the sound playing state
    }

    void Start()
    {
        InitializeWheels();
    }

    void Update()
    {
        zVelocity = transform.InverseTransformDirection(rigid.velocity).z;

        if (rigid.velocity.magnitude > 1f)
        {
            if (!isSoundPlaying)
            {
                SoundManager.Instance.PlayCarEngineSound(gameObject);
                isSoundPlaying = true;
            }
        }
        else if (rigid.velocity.magnitude <= 1f)
        {
            if (isSoundPlaying)
            {
                SoundManager.Instance.StopCarEngineSound(gameObject);
                isSoundPlaying = false;
            }
        }

        if (currentTarget != null)
        {
            HandleTargetUpdate();
        }
        else
        {
            currentTarget = GetClosestObjectWithTag(gameObject, "RoadPoint");

            if (currentTarget != null)
            {
                SetFutureTarget();
            }
        }

        if (stoppingPoint != null)
        {
            stop = StoppingPointManager();
            if (Vector3.Distance(steeringAim.transform.position, stoppingPoint.transform.position) > 8 && Vector3.Distance(steeringAim.transform.position, stoppingPoint.transform.position) > 3)
            {
                stoppingPoint = null;
            }
        }
        else if (stoppingPoint == null)
        {
            stop = CheckForVehiclesInFront();
        }

        if (currentTarget == null)
        {
            stop = true;
        }

        HandleSteering();
        HandleDriving();
    }

    void InitializeWheels()
    {
        for (int i = 0; i < frontWheels.Length; i++)
        {
            InitializeWheel(frontWheels[i]);
        }

        for (int i = 0; i < backWheels.Length; i++)
        {
            InitializeWheel(backWheels[i]);
        }
    }

    void InitializeWheel(WheelCollider wheelCollider)
    {
        Vector3 wheelPosition = wheelCollider.transform.position;
        tempWheel = Instantiate(wheel, wheelPosition, transform.rotation, transform) as GameObject;
        Wheel tempComp = tempWheel.AddComponent<Wheel>();
        tempComp.wheelCollider = wheelCollider.GetComponent<WheelCollider>();
        tempComp.flippedOnY = wheelPosition.x < transform.position.x;
    }

    void HandleTargetUpdate()
    {
        if (Vector3.Distance(steeringAim.transform.position, currentTarget.transform.position) < 0.5f + (zVelocity / 6))
        {
            UpdateTargets();
        }
        if (Vector3.Distance(transform.position, currentTarget.transform.position) < Vector3.Distance(steeringAim.transform.position, transform.position))
        {
            UpdateTargets();
        }

        if (currentTarget.pointType == RoadPoint.PointType.StoppingPoint)
        {
            HandleStoppingPoint();
        }

        UpdateSteeringAim();
    }

    void UpdateTargets()
    {
        if (currentTarget.connectedPoints.Count > 0)
        {
            previousTarget = currentTarget;
            currentTarget = futureTarget;
            SetFutureTarget();
        }
    }

    void HandleStoppingPoint()
    {
        if (stoppingPoint == null)
        {
            stoppingPoint = currentTarget;
        }
    }

    void UpdateSteeringAim()
    {
        steeringAim2.transform.LookAt(currentTarget.transform.position);
        Vector3 pos = steeringAim.transform.position;
        pos.y = currentTarget.transform.position.y;
        steeringAim.transform.position = pos;
        Vector3 rot = steeringAim2.transform.localEulerAngles;
        rot.x = 0;
        steeringAim2.transform.localEulerAngles = rot;
    }

    void HandleSteering()
    {
        float steer = steeringAim.transform.localEulerAngles.y;
        for (int i = 0; i < frontWheels.Length; i++)
        {
            if (reverse)
            {
                frontWheels[i].steerAngle = CalculateReverseSteerAngle(steer);
            }
            else
            {
                frontWheels[i].steerAngle = CalculateSteerAngle(steer);
            }
        }
    }

    float CalculateReverseSteerAngle(float steer)
    {
        if (steer < 360 && steer > 360 - steerAngle)
        {
            return steerAngle;
        }
        else if (steer < 360 - steerAngle && steer > 180)
        {
            return steerAngle;
        }
        else if (steer < -steerAngle && steer > -180)
        {
            return steerAngle;
        }
        if (steer > 0 && steer < steerAngle)
        {
            return -steerAngle;
        }
        else if (steer > steerAngle && steer < 180)
        {
            return -steerAngle;
        }
        return steerAngle;
    }

    float CalculateSteerAngle(float steer)
    {
        if (steer <= 360 && steer >= 360 - steerAngle)
        {
            return steer;
        }
        else if (steer <= 360 - steerAngle && steer >= 180)
        {
            return -steerAngle;
        }
        else if (steer <= -steerAngle && steer >= -180)
        {
            return -steerAngle;
        }
        else if (steer >= 0 && steer <= steerAngle)
        {
            return steer;
        }
        else if (steer >= steerAngle && steer <= 180)
        {
            return steerAngle;
        }
        return steer;
    }

    void HandleDriving()
    {
        bool vehicleInFront = CheckForVehiclesInFront();

        foreach (var wheel in (driveBias == DriveBias.FrontWheelDrive ? frontWheels : backWheels))
        {
            int currentSpeed = currentTarget == null ? 4 : Mathf.RoundToInt(currentTarget.speed);

            if (wheel.rpm < (int)(currentSpeed * 30f) && !stop && !vehicleInFront)
            {
                wheel.motorTorque = power;
                wheel.brakeTorque = 0;
            }
            else
            {
                wheel.motorTorque = 0;
                wheel.brakeTorque = brakeTorque;
            }
        }
    }

    bool StoppingPointManager()
    {
        if (Vector3.Distance(steeringAim.transform.position, stoppingPoint.transform.position) < 2)
        {
            if (steeringAim.transform.InverseTransformPoint(stoppingPoint.transform.position).normalized.z > 0.2f)
            {
                if (Vector3.Distance(steeringAim.transform.position, stoppingPoint.transform.position) < 0.2f + zVelocity)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (steeringAim.transform.InverseTransformPoint(stoppingPoint.transform.position).normalized.z < 0.2f)
            {
                return true;
            }
            return true;
        }
        return false;
    }

    bool CheckForVehiclesInFront()
    {
        ray = new Ray(steeringAim.transform.position, steeringAim.transform.forward);
        if (Physics.SphereCast(ray, 1f, out hit, 1.5f + (zVelocity * 2)))
        {
            if (hit.transform.CompareTag("Vehicle"))
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        if (currentTarget != null)
        {
            Vector3 pos = currentTarget.transform.position;
            Vector3 pos2 = Vector3.zero;
            if (futureTarget != null)
            {
                pos2 = futureTarget.transform.position;
            }
            pos.y += 0.1f;
            pos2.y += 0.1f;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(steeringAim.transform.position, pos);
            Gizmos.DrawLine(pos, pos2);
            Gizmos.DrawWireSphere(currentTarget.transform.position, 0.5f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, Vector3.Distance(transform.position, steeringAim.transform.position));
            Gizmos.color = Color.black;
        }
        if (futureTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(futureTarget.transform.position, 0.5f);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(steeringAim.transform.position, steeringAim.transform.position + (steeringAim.transform.forward * (1 + (zVelocity * 2))));
        Gizmos.DrawWireSphere(steeringAim.transform.position + (steeringAim.transform.forward * (1 + (zVelocity * 2))), 1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(steeringAim.transform.position, 0.3f + (zVelocity / 6));
        if (stoppingPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(stoppingPoint.transform.position, 0.2f + (zVelocity / 2));
        }
    }

    RoadPoint GetClosestObjectWithTag(GameObject vehicle, string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        GameObject closestObject = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject obj in objectsWithTag)
        {
            float distanceToObj = Vector3.Distance(vehicle.transform.position, obj.transform.position);
            if (distanceToObj < shortestDistance)
            {
                shortestDistance = distanceToObj;
                closestObject = obj;
            }
        }

        return closestObject != null ? closestObject.GetComponent<RoadPoint>() : default(RoadPoint);
    }

    void SetFutureTarget()
    {
        if (currentTarget != null && currentTarget.connectedPoints != null && currentTarget.connectedPoints.Count > 0)
        {
            futureTarget = currentTarget.connectedPoints[Random.Range(0, currentTarget.connectedPoints.Count)];
        }
        else
        {
            futureTarget = null;
        }
    }

    bool IsVehicleInCloseProximity()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f); // Adjust the radius as needed
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Vehicle") && hitCollider.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }
}
