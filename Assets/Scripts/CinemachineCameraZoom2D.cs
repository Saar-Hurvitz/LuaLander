using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{

    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;

    public static CinemachineCameraZoom2D Instance { get; private set; }

    [SerializeField] private CinemachineCamera cinemachineCamera;

    private float targetOrthoGraphicSize = 10f;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float zoomSpeed = 2f;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize,
                                                  targetOrthoGraphicSize, Time.deltaTime * zoomSpeed);
    }

    public void SetTargetOrthoGraphicSize(float targetOrthoGraphicSize)
    {
        this.targetOrthoGraphicSize = targetOrthoGraphicSize;
    }

    public void SetNormalOrthographicSize()
    {
        SetTargetOrthoGraphicSize(NORMAL_ORTHOGRAPHIC_SIZE);
    }

}
