using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MobileHandling : MonoBehaviour
{
    [SerializeField] private GameObject _mobileInputCanvas;
    [SerializeField] private GameObject _orientationWarning;
    public float _unitsSize = 1; // size of your scene in unity units
    public Constraint _constraint = Constraint.Portrait;
    public Camera _camera;

    private float _width;
    private float _height;
    //*** bottom screen
    private Vector3 _bl;
    private Vector3 _bc;
    private Vector3 _br;
    //*** middle screen
    private Vector3 _ml;
    private Vector3 _mc;
    private Vector3 _mr;
    //*** top screen
    private Vector3 _tl;
    private Vector3 _tc;
    private Vector3 _tr;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        ComputeResolution();
    }

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _mobileInputCanvas.SetActive(true);
            if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                _orientationWarning.SetActive(true);
            }
        }
    }

    private void ComputeResolution()
    {
        float leftX, rightX, topY, bottomY;

        if (_constraint == Constraint.Landscape)
        {
            _camera.orthographicSize = 1f / _camera.aspect * _unitsSize / 2f;
        }
        else
        {
            _camera.orthographicSize = _unitsSize / 2f;
        }

        _height = 2f * _camera.orthographicSize;
        _width = _height * _camera.aspect;

        float cameraX, cameraY;
        cameraX = _camera.transform.position.x;
        cameraY = _camera.transform.position.y;

        leftX = cameraX - _width / 2;
        rightX = cameraX + _width / 2;
        topY = cameraY + _height / 2;
        bottomY = cameraY - _height / 2;

        //*** bottom
        _bl = new Vector3(leftX, bottomY, 0);
        _bc = new Vector3(cameraX, bottomY, 0);
        _br = new Vector3(rightX, bottomY, 0);
        //*** middle
        _ml = new Vector3(leftX, cameraY, 0);
        _mc = new Vector3(cameraX, cameraY, 0);
        _mr = new Vector3(rightX, cameraY, 0);
        //*** top
        _tl = new Vector3(leftX, topY, 0);
        _tc = new Vector3(cameraX, topY, 0);
        _tr = new Vector3(rightX, topY, 0);
    }

    private void Update()
    {
#if UNITY_EDITOR
        ComputeResolution();
#endif
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                _orientationWarning.SetActive(true);
            }
        }
    }

    public enum Constraint { Landscape, Portrait }
}
