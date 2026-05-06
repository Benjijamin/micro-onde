using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;

    [Header("Translation")]
    [SerializeField] private Vector3 baseOffset;
    [SerializeField] private float smoothTranslationTime;

    [Header("Rotation")]
    [SerializeField] private float maxPanAngle;
    [SerializeField] private float smoothRotationTime;

    private float shakeMagnitude;
    private float shakeDuration;

    private Vector3 velocity = Vector3.zero;
    private float angleVelocity = 0;
    private Vector3 newPosition = Vector3.zero;
    private Vector3 lastPosition;
    private float shakeTimer;

    public static CameraManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        objectToFollow = GameObject.FindGameObjectWithTag("Player").transform;
        lastPosition = objectToFollow.position;
        transform.position = lastPosition + baseOffset;
    }

    private void FixedUpdate()
    {
        FollowTarget();
        RotateWithTargetMove();

        if (shakeTimer > 0)
        {
            transform.position += Random.insideUnitSphere * shakeMagnitude;
            shakeTimer -= Time.fixedDeltaTime;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeMagnitude = magnitude;

        shakeDuration = duration;
        shakeTimer = shakeDuration;
    }

    private void RotateWithTargetMove()
    {
        if (objectToFollow.position.x != lastPosition.x)
        {
            int multi = getPanAngleMulti();
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, maxPanAngle * multi, ref angleVelocity, smoothRotationTime);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        lastPosition = objectToFollow.position;
    }

    private void FollowTarget()
    {
        newPosition.x = objectToFollow.position.x + baseOffset.x;
        newPosition.y = objectToFollow.position.y + baseOffset.y;
        newPosition.z = objectToFollow.position.z + baseOffset.z;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTranslationTime);
    }

    private int getPanAngleMulti()
    {
        int multi = 1;
        if (objectToFollow.position.x < lastPosition.x)
            multi = -1;

        return multi;
    }
}