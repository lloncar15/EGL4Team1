using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VisitorSpawner : MonoBehaviour {
    [SerializeField] private GameObject visitorPrefab;
    [SerializeField] private List<Transform> artifactPoints = new();
    [SerializeField] private List<Transform> spawnPoints = new();
    [SerializeField] private Transform exitPoint;
    [SerializeField] private List<BloodType> bloodTypes = new();
    [SerializeField] private int visitorsToSpawn;

    private void OnEnable() {
        GameStateController.MuseumOpened += MuseumOpened;
    }

    private void OnDisable() {
        GameStateController.MuseumOpened -= MuseumOpened;
    }

    private void MuseumOpened() {
        SpawnVisitors();
    }

    private void SpawnVisitors() {
        for (int i = 0; i < visitorsToSpawn; i++) {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            BloodType bloodType = bloodTypes[Random.Range(0, bloodTypes.Count)];
            
            SpawnVisitor(spawnPoint.position, bloodType);
        }
    }

    private void SpawnVisitor(Vector3 spawnPosition, BloodType bloodType) {
        GameObject obj = Instantiate(visitorPrefab, spawnPosition, Quaternion.identity);
        
        Visitor visitor = obj.GetComponent<Visitor>();
        visitor.Init(artifactPoints, exitPoint, bloodType);
    }
}