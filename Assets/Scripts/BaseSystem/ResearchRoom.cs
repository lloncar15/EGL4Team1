using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ResearchRoom : MonoBehaviour {
    [SerializeField] public List<Transform> standingSpots;
    [SerializeField] public List<Transform> spotsToAnimate;
    public Transform animateText;

    public void AnimateEnter() {
        foreach (Transform spot in spotsToAnimate) {
            spot.DOScale(new Vector3(1f, 2f, 1f), 1f).SetEase(Ease.OutBack);
        }
        
        animateText.DOScale(new Vector3(0.4f, 0.4f, 1f), 1f).SetEase(Ease.OutBack);
    }
}