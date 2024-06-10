using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Transform cam;
    private Vector3 camStartPos;
    private float distance;

    private GameObject[] backgrounds;
    private Material[] materials;
    private float[] backSpeed;

    private float farthestBack;

    [Range(0.01f, 0.05f)]
    public float parallaxSpeed;

    private void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        materials = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            materials[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        CalculateBackgroundSpeed(backCount);
    }

    private void CalculateBackgroundSpeed(int backCount)
    {
        for (int i = 0; i < backCount; i++) // Find furthest background
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }

        for (int i = 0; i < backCount; i++) // set speed of backgrounds
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }

    private void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;
        //transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;

            materials[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
