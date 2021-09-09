using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private EffectKeeper _effectKeeper;
    [SerializeField] private TestModeSetter _testModeSetter;
    [SerializeField] private CameraDefaultFollower _defaultFollower;

    private Vector3 _startOffset;
    private Quaternion _startRotation;
    private Vector3 _resultAdditivePosition;
    private Quaternion _resultAdditiveRotation;
    private Vector3 _previousCameraPosition;
    private Quaternion _previousCameraRotation;
    private Quaternion _previousBallRotation;
    private Vector3 _previousBallPosition;
    private Dictionary<ICameraMovingAdder, bool> _enabledMovingAddersUpdated;
    private Dictionary<ICameraRotatingAdder, bool> _enabledRotatingAddersUpdated;

    public Vector3 StartOffset => _startOffset;
    public Quaternion StartRotation => _startRotation;
    public Vector3 PreviousCameraPosition => _previousCameraPosition;
    public Quaternion PreviousCameraRotation => _previousCameraRotation;
    public Quaternion PreviousBallRotation => _previousBallRotation;
    public Vector3 PreviousBallPosition => _previousBallPosition;

    public event UnityAction AdderDisabled;
    public event UnityAction AdderEnabled;

    private void Awake()
    {
        if (_testModeSetter.IsTestMode == false)
            _effectKeeper.SavedEffectsEnabled += OnSavedEffectsEnabled;
        else
            OnSavedEffectsEnabled();

        if (_defaultFollower is ICameraMovingAdder adder)
            _enabledMovingAddersUpdated.Add(adder, false);
    }

    private void Start()
    {
        _startRotation = _mainCamera.transform.rotation;
        _startOffset = _mainCamera.transform.position - _ball.transform.position;
        ResetParameters();
    }

    private void OnDestroy()
    {
        if (_effectKeeper != null)
            _effectKeeper.SavedEffectsEnabled -= OnSavedEffectsEnabled;
    }

    private void OnSavedEffectsEnabled()
    {
        _enabledMovingAddersUpdated = InitAdderBoolDictionary<ICameraMovingAdder>();
        _enabledRotatingAddersUpdated = InitAdderBoolDictionary<ICameraRotatingAdder>();

        foreach (ICameraMovingAdder adder in _enabledMovingAddersUpdated.Keys.ToList())
        {
            if (adder is Effect effect)
                Debug.Log(effect.gameObject.name);
        }
    }

    private Dictionary<Adder, bool> InitAdderBoolDictionary<Adder>()
    {
        List<Adder> adders = _effectKeeper.GetTypedEffects<Adder>();
        Dictionary<Adder, bool>  addersAdded = new Dictionary<Adder, bool>();
        foreach (Adder adder in adders)
        {
            if (adder is Effect effect)
            {
                effect.Disabled += OnAdderDisabled;
                effect.Enabled += OnAdderEnabled;
                effect.Destroyed += OnAdderDestroyed;
                if (effect.enabled)
                    addersAdded.Add(adder, false);
            }
        }

        return addersAdded;
    }

    private void OnAdderEnabled(Effect adder)
    {
        if (adder is ICameraMovingAdder movingAdder && _enabledMovingAddersUpdated.ContainsKey(movingAdder) == false)
            _enabledMovingAddersUpdated.Add(movingAdder, false);
        if (adder is ICameraRotatingAdder rotatingAdder && _enabledRotatingAddersUpdated.ContainsKey(rotatingAdder) == false)
            _enabledRotatingAddersUpdated.Add(rotatingAdder, false);

        ResetCameraPosition();
        ResetParameters();
        AdderEnabled?.Invoke();
    }

    private void OnAdderDisabled(Effect adder)
    {
        if (adder is ICameraMovingAdder movingAdder)
            _enabledMovingAddersUpdated.Remove(movingAdder);
        if (adder is ICameraRotatingAdder rotatingAdder)
            _enabledRotatingAddersUpdated.Remove(rotatingAdder);

        ResetCameraPosition();
        ResetParameters();
        AdderDisabled?.Invoke();
    }

    private void OnAdderDestroyed(Effect adder)
    {
        adder.Disabled -= OnAdderDisabled;
        adder.Enabled -= OnAdderEnabled;
        adder.Destroyed -= OnAdderDestroyed;
    }

    private void ResetParameters()
    {
        ResetPositionParameters();
        ResetRotationParameters();
    }

    private void ResetPositionParameters()
    {
        _resultAdditivePosition = Vector3.zero;
        if (_mainCamera != null)
            _previousCameraPosition = _mainCamera.transform.position;

        if (_ball != null)
            _previousBallPosition = _ball.transform.position;


        foreach (ICameraMovingAdder adder in _enabledMovingAddersUpdated.Keys.ToList())
        {
            _enabledMovingAddersUpdated[adder] = false;
        }
    }

    private void ResetRotationParameters()
    {
        _resultAdditiveRotation = Quaternion.Euler(Vector3.zero);
        if (_mainCamera != null)
            _previousCameraRotation = _mainCamera.transform.rotation;

        if (_ball != null)
            _previousBallRotation = _ball.transform.rotation;

        foreach (ICameraRotatingAdder adder in _enabledRotatingAddersUpdated.Keys.ToList())
        {
            _enabledRotatingAddersUpdated[adder] = false;
        }
    }

    private void ResetCameraPosition()
    {
        if (_mainCamera != null)
        {
            _mainCamera.transform.rotation = _startRotation;
            if (_ball != null)
                _mainCamera.transform.position = _ball.transform.position + _startOffset;
        }
    }

    private void TryApplyAdditivePosition()
    {
        if (_enabledMovingAddersUpdated.ContainsValue(false) == false)
        {
            _mainCamera.transform.position += _resultAdditivePosition;
            ResetPositionParameters();
        }
    }

    private void TryApplyAdditiveRotation()
    {
        if (_enabledRotatingAddersUpdated.ContainsValue(false) == false)
        {
            _mainCamera.transform.rotation = _resultAdditiveRotation * _mainCamera.transform.rotation;
            ResetRotationParameters();
        }
    }

    public void AddPosition(Vector3 additivePosition, ICameraMovingAdder movingAdder)
    {
        if (_enabledMovingAddersUpdated.ContainsKey(movingAdder))
        {
            _resultAdditivePosition += additivePosition;
            _enabledMovingAddersUpdated[movingAdder] = true;
            TryApplyAdditivePosition();
        }
    }

    public void AddRotation(Quaternion additiveRotation, ICameraRotatingAdder rotatingAdder)
    {
        if (_enabledRotatingAddersUpdated.ContainsKey(rotatingAdder))
        {
            _resultAdditiveRotation = additiveRotation * _resultAdditiveRotation;
            _enabledRotatingAddersUpdated[rotatingAdder] = true;
            TryApplyAdditiveRotation();
        }
    }
}
