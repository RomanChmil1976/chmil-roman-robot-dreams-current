using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//using UnityEngine.InputSystem;

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
    [SerializeField] private Transform yawAnchor;   // Player
    [SerializeField] private Transform pitchAnchor; // CameraPivot
    
    [Header("WeaponController")]
    [SerializeField] private WeaponController weaponController;
    
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionDistance = 5f;
    [SerializeField] private float explosionLifetime = 5f;
    [SerializeField] private float heightOffset = 1.0f; // насколько выше земли будет взрыв
    
    [Header("Music Toggle")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private Image musicToggleImage;
    [SerializeField] private Sprite musicOnIcon;
    [SerializeField] private Sprite musicOffIcon;
    [SerializeField] private MusicToggle musicToggle;


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
    ///private PlayerInputActions _input = new PlayerInputActions();

    private CharacterController _controller;
    //private Animator _animator;

    private void Awake()
    {
        _input = new PlayerInputActions();
        _input.Player.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _input.Player.Movement.canceled += ctx => _moveInput = Vector2.zero;
        _input.Player.Jump.performed += ctx => Jump();
        _input.Player.Sprint.started += ctx => _isSprinting = true;
        _input.Player.Sprint.canceled += ctx => _isSprinting = false;
        _input.Player.ToggleAimMode.performed += ctx => ToggleAimMode();
        _input.Player.Crouch.performed += ctx => ToggleCrouch();
        _input.Player.MouseLook.performed += ctx => _mouseDelta = ctx.ReadValue<Vector2>();
        _input.Player.Fire.performed += ctx => Fire();             // ЛКМ
        _input.Player.Shoot.performed += ctx => TriggerExplosion(); // ПКМ
        _input.Player.BackToMenu.performed += ctx => GoBackToMainMenu();
        _input.Player.ToggleMusic.performed += ctx => ToggleMusic();
        _input.Player.CameraZoom.performed += ctx => _scrollDelta = ctx.ReadValue<Vector2>();
        _input.Player.CameraZoom.canceled += ctx => _scrollDelta = Vector2.zero;




    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        //_animator = GetComponentInChildren<Animator>();
        _defaultZoom = offset.magnitude;
        _currentZoom = _defaultZoom;

        if (mainPlayerBody != null)
            _initialBodyY = mainPlayerBody.localPosition.y;

        if (crosshairCanvas != null)
            crosshairCanvas.SetActive(false);

        if (yawAnchor == null || pitchAnchor == null)
            Debug.LogWarning("❗ Не назначен yawAnchor или pitchAnchor!");

        if (SceneManager.GetActiveScene().name == "Scene_4_Game_1")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        
        if (musicAudioSource == null)
            Debug.LogWarning("🎵 Music AudioSource is not assigned!");

        if (musicToggleImage == null)
            Debug.LogWarning("🎵 Music Toggle Image is not assigned!");

    }

    private void Update()
    {
        HandleMovement();
        ApplyGravity();
        ApplyMovement();
        HandleMouseLook();
        //UpdateAnimations();
        HandleCameraControl();
        UpdateAimTargetPosition();
        _mouseDelta = Vector2.zero;
        
        //Debug.Log($"🖱️ Scroll delta: {_scrollDelta.y}");
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
        
        if (_isAiming)
        {
            _xRotation = Mathf.Clamp(_xRotation, -50f, 50f); //  Боевое положение min-вверх, max-вниз
        }
        else
        {
            _xRotation = Mathf.Clamp(_xRotation, -20f,  30f); // ️ Обычное положение min-вверх, max-вниз
        }


        if (pitchAnchor != null)
            pitchAnchor.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }


    // private void UpdateAnimations()
    // {
    //     if (_animator == null) return;
    //     float speed = new Vector3(_moveDirection.x, 0f, _moveDirection.z).magnitude;
    //     _animator.SetFloat("Speed", speed);
    //     _animator.SetBool("IsJumping", !_controller.isGrounded);
    // }

    private void ToggleAimMode()
    {
        _isAiming = !_isAiming;
        if (crosshairCanvas != null)
            crosshairCanvas.SetActive(_isAiming);
        if (_isAiming)
            _currentZoom = _defaultZoom;
    }

    private void Fire()
    {
        if (!_isAiming) return;
        weaponController?.Fire2(); // 🔄 Новый вызов Fire2()
    }



    private void HandleCameraControl()
    {
        float scroll = _scrollDelta.y;

        Transform target = _isAiming ? aimTarget : followTarget;
        if (target == null) return;

        //float scroll = Input.mouseScrollDelta.y;
        if (!_isAiming && Mathf.Abs(scroll) > 0.01f)
        {
            _currentZoom -= scroll * zoomSpeed;
            _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);
        }

        if (_isAiming)
            _currentZoom = Mathf.Lerp(_currentZoom, normalZoom, Time.deltaTime * 10f);

        //  Заменил target.forward на pitchAnchor.forward, чтобы камера смотрела туда же, куда повёрнут pitchAnchor
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
    
    // private void UpdateAimTargetPosition()
    // {
    //     if (aimTarget == null || Camera.main == null) return;
    //
    //     Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    //
    //     if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
    //     {
    //         aimTarget.position = hit.point;
    //     }
    //     else
    //     {
    //         aimTarget.position = ray.GetPoint(100f);
    //     }
    // }

    private void TriggerExplosion()
    {
        if (!_isAiming) return;
        if (playerCamera == null || explosionPrefab == null) return;


        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);


        Vector3 explosionPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            explosionPoint = hit.point;
        }
        else
        {
            explosionPoint = ray.GetPoint(explosionDistance);

            if (Terrain.activeTerrain != null)
            {
                float terrainHeight = Terrain.activeTerrain.SampleHeight(explosionPoint);
                if (explosionPoint.y < terrainHeight + heightOffset)
                    explosionPoint.y = terrainHeight + heightOffset;
            }
        }

        // Создание взрыва
        GameObject explosion = Instantiate(explosionPrefab, explosionPoint, Quaternion.identity);
        Destroy(explosion, explosionLifetime);

        // Взрывная сила
        float explosionForce = 500f;
        float explosionRadius = 5f;

        Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius);
        foreach (var hitObj in colliders)
        {
            Rigidbody rb = hitObj.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius);
            }
        }

        Debug.Log($"Взрыв с физическим воздействием: {explosionPoint}");
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene("Scene_1.2_Pause");
        Debug.Log("Переключение на меню паузы");
    }

    private void ToggleMusic()
    {
        if (musicToggle != null)
        {
            musicToggle.ToggleMusic();
        }
    }


    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();
}
