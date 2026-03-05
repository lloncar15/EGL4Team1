using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum VisitorState {
    Walking,
    Mesmerized,
    Drained,
    Standing,
    GoToExit
}

public class Visitor : MonoBehaviour { 
    [SerializeField] private VisitorState state;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject mesmerizedTear;
    
    [Header("Standing Time")]
    [SerializeField] public Vector2 standTimeRange = new(2f, 5f);
    private float _standTime;

    [Header("Speed")]
    [SerializeField]public Vector2 speedRange = new(1f, 3.5f);
    
    private NavMeshAgent _agent;
    
    private BloodType _bloodType;

    private Transform _exitPoint;

    private List<InteractableArtifactHolder> _artifactSpots = new();
    private int _currentSpotIndex = 0;
    private float _standTimer;
    private float _mesmerizedTimer;

    private const float MESMERIZE_DURATION = 2f;
    private const float DISTANCE_CHECK = 0.2f;

    public void Init(List<InteractableArtifactHolder> spots, Transform exit, BloodType setType) {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        
        _exitPoint = exit;
        
        _bloodType = setType;
        spriteRenderer.color = setType.color;
        
        // shuffle the spots
        _artifactSpots = new List<InteractableArtifactHolder>(spots);
        Shuffle(_artifactSpots);
        
        // get random speed
        _agent.speed = Random.Range(speedRange.x, speedRange.y);
        
        // get random standing time
        _standTime = Random.Range(standTimeRange.x, standTimeRange.y);
        
        GoToNextSpot();
    }

    private void Update() {
        switch (state) {
            case VisitorState.Walking: {
                CheckArrival();
                break;
            }
            case VisitorState.Standing: {
                _standTimer -= Time.deltaTime;
                if (_standTimer <= 0)
                    GoToNextSpot();
                break;
            }
            case VisitorState.GoToExit: {
                CheckExitArrival();
                break;
            }
            case VisitorState.Mesmerized: {
                _mesmerizedTimer -= Time.deltaTime;
                if (_mesmerizedTimer <= 0) {
                    mesmerizedTear.SetActive(false);
                    _agent.isStopped = false;
                }
                break;
            }
        }
    }

    private bool HasArrivedOnSpot() {
        return !_agent.pathPending && _agent.remainingDistance < DISTANCE_CHECK;
    }

    private void CheckArrival() {
        if (HasArrivedOnSpot())
            StartStanding();
    }

    private void StartStanding() {
        state = VisitorState.Standing;
        _standTimer = _standTime;
        _agent.isStopped = true;
    }

    private void GoToNextSpot() {
        while (true) {
            _agent.isStopped = false;

            if (_currentSpotIndex >= _artifactSpots.Count) {
                GoToExit();
                return;
            }

            InteractableArtifactHolder artifactSpot = _artifactSpots[_currentSpotIndex];
            _currentSpotIndex++;

            if (!artifactSpot.HoldsAnArtifact) {
                continue;
            }

            List<Transform> standingSpots = artifactSpot.standingTransforms;
            Transform target = standingSpots[Random.Range(0, standingSpots.Count)];

            _agent.SetDestination(target.position);

            state = VisitorState.Walking;
            break;
        }
    }

    private void GoToExit() {
        state = VisitorState.GoToExit;

        _agent.isStopped = false;
        _agent.SetDestination(_exitPoint.position);
    }

    private void CheckExitArrival() {
        if (HasArrivedOnSpot()) {
            Destroy(gameObject);
        }
    }

    public void Mesmerize() {
        if (state is VisitorState.Drained or VisitorState.GoToExit)
            return;
        
        state = VisitorState.Mesmerized;
        _agent.isStopped = true;
        _mesmerizedTimer = MESMERIZE_DURATION;
        mesmerizedTear.SetActive(true);
    }

    public void Drain() {
        if (state != VisitorState.Mesmerized)
            return;

        BloodManager manager = BloodManager.Instance;
        if (manager.HasCollectedBlood)
            return;
        
        mesmerizedTear.SetActive(false);
        state = VisitorState.Drained;
        manager.AddBlood(_bloodType);
        
        GoToExit();
    }
    
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]); // tuple swap
        }
    }
}