using UnityEngine;

public class LightCulling : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private float shadowCullingDistance = 15f;
    [SerializeField] private float lightCullingDistance = 30f;

    private Light _light;
    public bool enableShadows = false;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Update()
    {
        float cameraDistance = Vector3.Distance(playerCamera.transform.position, transform.position);

        if (cameraDistance <= shadowCullingDistance && enableShadows)
        {
            _light.shadows = LightShadows.Soft;
        }
        else
        {
            _light.shadows = LightShadows.None;
        }

        _light.enabled = cameraDistance <= lightCullingDistance;
    }
}
