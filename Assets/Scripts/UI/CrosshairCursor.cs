using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairCursor : MonoBehaviour
{
    public static CrosshairCursor instance;

    [SerializeField] private Image image;
    [SerializeField] private Sprite[] cursorSprites;
    [SerializeField] float frameDuration;
    [SerializeField] private Animation rotateAnimation;
    [SerializeField] private AnimationClip rotateAnim;

    private float t;
    private int spriteIndex;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Cursor.visible = false;
            WeaponHolster.OnPlayerInitiatedAttack += Rotate;
            return;
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        t += Time.deltaTime;
        if(t > frameDuration)
        {
            t -= frameDuration;
            image.sprite = cursorSprites[spriteIndex];
            spriteIndex = (spriteIndex + 1) % cursorSprites.Length;
        }
    }

    private void LateUpdate()
    {
        FollowCursor();
    }

    private void FollowCursor()
    {
        image.transform.position = Input.mousePosition;
    }

    private void Rotate(float x)
    {
        rotateAnimation.Stop();
        rotateAnimation.Play();
    }
}
