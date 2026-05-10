using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;

    [Header("Translation")]
    [SerializeField] private Vector3 baseOffset;
    [SerializeField] private float smoothTranslationTime;
    [SerializeField, Range(0,1)] private float cursorInfluence;
    [SerializeField, Range(0,1)] private float cursorRatioThreshold;

    [Header("Rotation")]
    [SerializeField] private float maxPanAngle;
    [SerializeField] private float smoothRotationTime;

    private float shakeMagnitude;
    private float shakeDuration;

    private Vector3 velocity = Vector3.zero;
    private float angleVelocity = 0;
    private Vector3 lastPosition;
    private float shakeTimer;

    public static CameraManager instance;

    private Vector3 CursorPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    private Vector3 TargetPosition => Vector3.Lerp(objectToFollow.position, CursorPosition, cursorInfluence);

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
        Vector2 mousePos = Input.mousePosition;
        float xRatio = Mathf.Abs((mousePos.x / Screen.width - 0.5f) * 2f);
        float yRatio = Mathf.Abs((mousePos.y / Screen.height - 0.5f) * 2f);

        Vector3 newPosition = baseOffset;

        if (xRatio >= cursorRatioThreshold)
            newPosition.x += Mathf.Lerp(objectToFollow.position.x, CursorPosition.x, cursorInfluence);
        else
            newPosition.x += objectToFollow.position.x;

        if (yRatio >= cursorRatioThreshold)
            newPosition.y += Mathf.Lerp(objectToFollow.position.y, CursorPosition.y, cursorInfluence);
        else
            newPosition.y += objectToFollow.position.y;

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