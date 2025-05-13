using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class UnifiedPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintSpeed = 9f;

    [Header("Jump & Gravity")]
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _coyoteTime = 0.2f;

    [Header("Camera Settings")]
    [SerializeField] private Transform followTarget;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, -5f);
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float normalZoom = 5f;
    [SerializeField] private float cameraSmooth = 10f;
    [SerializeField] private Camera playerCamera;

    [Header("Crosshair & Body")]
    [SerializeField] private GameObject crosshairCanvas;
    [SerializeField] private Transform mainPlayerBody;

    [Header("Crouch Settings")]
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchingHeight = 1f;
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 1f, 0);
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private float crouchYOffset = -1.1f;
    [SerializeField] private float standingAimY = 1.7f;
    [SerializeField] private float crouchingAimY = 1.2f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform yawAnchor;
    [SerializeField] private Transform pitchAnchor;

    [Header("WeaponController")]
    [SerializeField] private WeaponController weaponController;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionDistance = 5f;
    [SerializeField] private float explosionLifetime = 5f;
    [SerializeField] private float heightOffset = 1.0f;

    [Header("Music Toggle")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private Image musicToggleImage;
    [SerializeField] private Sprite musicOnIcon;
    [SerializeField] private Sprite musicOffIcon;
    [SerializeField] private MusicToggle musicToggle;

    [Header("Character Models")]
    [SerializeField] private GameObject thirdPersonModel;
    [SerializeField] private GameObject firstPersonArms;
    [SerializeField] private Animator characterAnimator;

    private float _currentZoom;
    private float _defaultZoom;
    private float _verticalVelocity;
    private float _coyoteTimer;
    private bool _isAiming;
    private bool _isCrouching;
    private bool _isSprinting;

    private Vector2 _moveInput;
    private Vector3 _moveDirection;
    private Vector2 _mouseDelta;
    private Vector2 _smoothedMouseDelta;
    private float _xRotation;
    private float _initialBodyY;
    private Vector2 _scrollDelta;

    private PlayerInputActions _input;
    private CharacterController _controller;

    private InputActionMap _gameplayMap;
    private bool _isPaused;

    public void SetPaused(bool value)
    {
        _isPaused = value;
        if (_gameplayMap == null) return;
        if (value)
            _gameplayMap.Disable();
        else
            StartCoroutine(ReenableInputDelayed());
    }

    private void Awake()
    {
        _input = new PlayerInputActions();
        _gameplayMap = _input.Player;

        _input.Player.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _input.Player.Movement.canceled += ctx => _moveInput = Vector2.zero;
        _input.Player.Jump.performed += ctx => Jump();
        _input.Player.Sprint.started += ctx => _isSprinting = true;
        _input.Player.Sprint.canceled += ctx => _isSprinting = false;
        _input.Player.ToggleAimMode.performed += ctx => ToggleAimMode();
        _input.Player.Crouch.performed += ctx => ToggleCrouch();
        _input.Player.MouseLook.performed += ctx => _mouseDelta = ctx.ReadValue<Vector2>();
        _input.Player.Fire.performed += ctx => Fire();
        _input.Player.Shoot.performed += ctx => TriggerExplosion();
        _input.Player.BackToMenu.performed += ctx => GoBackToMainMenu();
        _input.Player.ToggleMusic.performed += ctx => ToggleMusic();
        _input.Player.CameraZoom.performed += ctx => _scrollDelta = ctx.ReadValue<Vector2>();
        _input.Player.CameraZoom.canceled += ctx => _scrollDelta = Vector2.zero;
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _defaultZoom = offset.magnitude;
        _currentZoom = _defaultZoom;

        if (mainPlayerBody != null)
            _initialBodyY = mainPlayerBody.localPosition.y;

        if (crosshairCanvas != null)
            crosshairCanvas.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Scene_4_Game_1")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (_isPaused) return;

        HandleMovement();
        ApplyGravity();
        ApplyMovement();
        HandleMouseLook();
        HandleCameraControl();
        UpdateAimTargetPosition();
        _mouseDelta = Vector2.zero;
        
        if (characterAnimator != null)
        {
            float speedPercent = _moveDirection.magnitude;
            characterAnimator.SetFloat("Speed", speedPercent);
        }

    }

    private void HandleMovement()
    {
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        _moveDirection = camForward * _moveInput.y + camRight * _moveInput.x;
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -0.5f;
            _coyoteTimer = _coyoteTime;
        }
        else
        {
            _verticalVelocity -= _gravity * Time.deltaTime;
            _coyoteTimer -= Time.deltaTime;
        }
    }

    private void ApplyMovement()
    {
        float speed = _isSprinting && _moveInput.y > 0.5f ? _sprintSpeed : _speed;
        Vector3 velocity = new Vector3(_moveDirection.x * speed, _verticalVelocity, _moveDirection.z * speed);
        _controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (_coyoteTimer > 0f)
        {
            _verticalVelocity = _jumpForce;
            _coyoteTimer = 0f;
        }
    }

    private void ToggleCrouch()
    {
        _isCrouching = !_isCrouching;
        _controller.height = _isCrouching ? crouchingHeight : standingHeight;
        _controller.center = _isCrouching ? crouchingCenter : standingCenter;

        if (mainPlayerBody != null)
        {
            Vector3 pos = mainPlayerBody.localPosition;
            pos.y = _isCrouching ? _initialBodyY + crouchYOffset : _initialBodyY;
            mainPlayerBody.localPosition = pos;
        }

        if (aimTarget != null)
        {
            Vector3 aimPos = aimTarget.localPosition;
            aimPos.y = _isCrouching ? crouchingAimY : standingAimY;
            aimTarget.localPosition = aimPos;
        }
    }

    private void HandleMouseLook()
    {
        _smoothedMouseDelta = Vector2.Lerp(_smoothedMouseDelta, _mouseDelta, Time.deltaTime * 20f);

        float mouseX = Mathf.Clamp(_smoothedMouseDelta.x, -10f, 10f) * mouseSensitivity;
        float mouseY = Mathf.Clamp(_smoothedMouseDelta.y, -10f, 10f) * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _isAiming ? -50f : -20f, _isAiming ? 50f : 30f);

        if (pitchAnchor != null)
            pitchAnchor.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    // private void ToggleAimMode()
    // {
    //     _isAiming = !_isAiming;
    //     if (crosshairCanvas != null)
    //         crosshairCanvas.SetActive(_isAiming);
    //     if (_isAiming)
    //         _currentZoom = _defaultZoom;
    // }
    
    private void ToggleAimMode()
    {
        _isAiming = !_isAiming;

        if (crosshairCanvas != null)
            crosshairCanvas.SetActive(_isAiming);

        if (firstPersonArms != null)
            firstPersonArms.SetActive(_isAiming);
    
        if (thirdPersonModel != null)
            thirdPersonModel.SetActive(!_isAiming);

        if (_isAiming)
            _currentZoom = _defaultZoom;
    }


    private void Fire()
    {
        if (!_isAiming) return;
        weaponController?.Fire2();
    }

    private void HandleCameraControl()
    {
        float scroll = _scrollDelta.y;

        Transform target = _isAiming ? aimTarget : followTarget;
        if (target == null) return;

        if (!_isAiming && Mathf.Abs(scroll) > 0.01f)
        {
            _currentZoom -= scroll * zoomSpeed;
            _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);
        }

        if (_isAiming)
            _currentZoom = Mathf.Lerp(_currentZoom, normalZoom, Time.deltaTime * 10f);

        Vector3 offsetPos = -pitchAnchor.forward * _currentZoom + Vector3.up * offset.y;
        Vector3 desiredPos = target.position + offsetPos;
        Vector3 smoothedPos = Vector3.Lerp(playerCamera.transform.position, desiredPos, Time.deltaTime * cameraSmooth);

        playerCamera.transform.position = smoothedPos;
        playerCamera.transform.LookAt(target);
    }

    private void UpdateAimTargetPosition()
    {
        if (aimTarget == null) return;
        Vector3 aimPos = aimTarget.localPosition;
        float targetY = _isCrouching ? crouchingAimY : standingAimY;
        aimPos.y = Mathf.Lerp(aimPos.y, targetY, Time.deltaTime * 10f);
        aimTarget.localPosition = aimPos;
    }

    private void TriggerExplosion()
    {
        if (!_isAiming || playerCamera == null || explosionPrefab == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Vector3 explosionPoint = Physics.Raycast(ray, out RaycastHit hit, 100f) ? hit.point : ray.GetPoint(explosionDistance);

        if (Terrain.activeTerrain != null)
        {
            float terrainHeight = Terrain.activeTerrain.SampleHeight(explosionPoint);
            if (explosionPoint.y < terrainHeight + heightOffset)
                explosionPoint.y = terrainHeight + heightOffset;
        }

        GameObject explosion = Instantiate(explosionPrefab, explosionPoint, Quaternion.identity);
        Destroy(explosion, explosionLifetime);

        float explosionForce = 500f;
        float explosionRadius = 5f;

        Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius);
        foreach (var hitObj in colliders)
        {
            Rigidbody rb = hitObj.attachedRigidbody;
            if (rb != null)
                rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius);
        }
    }

    private void GoBackToMainMenu()
    {
        PauseMenuUI.Instance.TogglePause();
    }

    private void ToggleMusic()
    {
        musicToggle?.ToggleMusic();
    }

    public void ResumeWithInputDelay()
    {
        SetPaused(false);
        StartCoroutine(ReenableInputDelayed());
    }

    private IEnumerator ReenableInputDelayed()
    {
        yield return null;
        _input.Enable();
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();
}
