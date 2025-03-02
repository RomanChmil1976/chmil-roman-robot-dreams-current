using System;
using UnityEngine;

namespace Lesson6
{
    public class TNTController : MonoBehaviour
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        [SerializeField] private Collider _collider;
        [SerializeField] private Renderer _tntRenderer;
        [SerializeField] private Renderer _explosionRenderer;
        [SerializeField] private Transform _explosionTransform;
        [SerializeField] private AnimationCurve _explosionScaleCurve;
        [SerializeField] private Gradient _explosionGradient;
        [SerializeField] private float _fuseTime;
        [SerializeField] private float _explosionTime;
        [SerializeField] private float _fuseBlinkTime;

        private Material[] _tntMaterials;
        private Material _explosionMaterial;

        private SignalGenerator _signalGenerator;
        
        private SimplexTimer _fuseTimer;
        private SimplexTimer _explosionTimer;
        
        private void Awake()
        {
            _tntMaterials = _tntRenderer.materials;
            for (int i = 0; i < _tntMaterials.Length; ++i)
            {
                _tntMaterials[i].EnableKeyword("_EMISSION");
            }

            _explosionMaterial = _explosionRenderer.material;

            _signalGenerator = new SignalGenerator(_fuseBlinkTime, 0.5f);
        }

        private void OnEnable()
        {
            _collider.enabled = true;
            ResetTNTMaterial();
            ResetExplosion();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            
            if (_fuseTimer != null)
            {
                SetEmissionColor(Color.white * _signalGenerator.EvaluateSignal(_fuseTimer.Time));
                _fuseTimer.Update(deltaTime);
            }

            if (_explosionTimer != null)
            {
                float explosionProgress = _explosionTimer.Progress;
                _explosionTransform.localScale = Vector3.one * _explosionScaleCurve.Evaluate(explosionProgress);
                _explosionRenderer.material.SetColor(BaseColor, _explosionGradient.Evaluate(explosionProgress));
                _explosionTimer.Update(deltaTime);
            }
        }

        private void OnMouseDown()
        {
            _collider.enabled = false;
            _fuseTimer = new SimplexTimer(_fuseTime);
            _fuseTimer.OnCompleted += OnFuseComplete;
        }

        private void OnFuseComplete()
        {
            _fuseTimer.Dispose();
            _fuseTimer = null;
            
            _tntRenderer.enabled = false;
            _explosionRenderer.enabled = true;

            _explosionTimer = new SimplexTimer(_explosionTime);
            _explosionTimer.OnCompleted += OnExplosionComplete;
        }

        private void OnExplosionComplete()
        {
            _explosionTimer.Dispose();
            _explosionTimer = null;
            
            _explosionRenderer.enabled = false;
            enabled = false;
        }
        
        private void ResetTNTMaterial()
        {
            _tntRenderer.enabled = true;
            for (int i = 0; i < _tntMaterials.Length; ++i)
            {
                _tntMaterials[i].SetColor(EmissionColor, Color.black);
            }
        }

        private void SetEmissionColor(Color color)
        {
            for (int i = 0; i < _tntMaterials.Length; ++i)
            {
                _tntMaterials[i].SetColor(EmissionColor, color);
            }
        }
        
        private void ResetExplosion()
        {
            _explosionRenderer.enabled = false;
            _explosionTransform.localScale = Vector3.one * _explosionScaleCurve.Evaluate(0f);
            _explosionMaterial.SetColor(BaseColor, _explosionGradient.Evaluate(0f));
        }
    }
}