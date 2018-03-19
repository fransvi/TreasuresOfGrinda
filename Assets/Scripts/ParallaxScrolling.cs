using UnityEngine;
using System.Collections;

public class ParallaxScrolling: MonoBehaviour
{

    public Transform[] backgrounds;
    private float[] parallaxScales;
    public float smoothing = 1f;
    private Transform cam;
    private Vector3 previousCamPos;

    private GameObject _player;

    void Awake()
    {
        cam = Camera.main.transform;
        _player = GameObject.Find("Player");
    }

    void Start()
    {
        previousCamPos = cam.position;
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }


    void Update()
    {

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            float backgroundTargetPosy = backgrounds[i].position.y + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        previousCamPos = cam.position;
    }
}
