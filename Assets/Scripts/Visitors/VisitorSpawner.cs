using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VisitorSpawner : MonoBehaviour {
    [SerializeField] private GameObject visitorPrefab;
    [SerializeField] private List<InteractableArtifactHolder> artifactPoints = new();
    [SerializeField] private List<Transform> spawnPoints = new();
    [SerializeField] private Transform exitPoint;
    [SerializeField] private List<BloodType> bloodTypes = new();
    [SerializeField] private int visitorsToSpawn;
    [SerializeField] private Vector2 spawnTimeOffset = new(0.5f, 2f);

    private void OnEnable() {
        GameStateController.MuseumOpened += MuseumOpened;
    }

    private void OnDisable() {
        GameStateController.MuseumOpened -= MuseumOpened;
    }

    private void MuseumOpened() {
        StartCoroutine(SpawnVisitors());
    }

    private IEnumerator SpawnVisitors() {
        for (int i = 0; i < visitorsToSpawn; i++) {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            BloodType bloodType = bloodTypes[Random.Range(0, bloodTypes.Count)];
            
            SpawnVisitor(spawnPoint.position, bloodType);
            
            yield return new WaitForSeconds(Random.Range(spawnTimeOffset.x, spawnTimeOffset.y));
        }
    }

    private void SpawnVisitor(Vector3 spawnPosition, BloodType bloodType) {
        GameObject obj = Instantiate(visitorPrefab, spawnPosition, Quaternion.identity);
        
        Visitor visitor = obj.GetComponent<Visitor>();
        visitor.Init(artifactPoints, exitPoint, bloodType);
    }
}