using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayerPlacer : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        Vector3 positionDifference = _startPosition.position - player.transform.position;
        player.transform.position += positionDifference;
        Camera.main.transform.position += positionDifference;
    }
}
