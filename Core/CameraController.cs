using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private float zoomExponent;
    [SerializeField] private float minOrthZoom;
    [SerializeField] private float maxOrthZoom;

    private float x;
    private float y;
    private float distanceApart;
    private float distanceApartX;
    private float distanceApartY;

    // Update is called once per frame
    void Update()
    {
        x = (player1.position.x + player2.position.x) / 2;
        y = (player1.position.y + player2.position.y) / 2;
        distanceApart = Mathf.Sqrt(x * x + y * y);
        distanceApartX = Mathf.Abs(player1.position.x - player2.position.x);
        distanceApartY = Mathf.Abs(player1.position.y - player2.position.y);

        transform.position = new Vector3(x, y, transform.position.z);

        distanceApart = distanceApartX > distanceApartY ? distanceApartX : distanceApartY;

        if (distanceApart < 1)
        {
            distanceApart = 1;
        }

        if (Mathf.Pow(distanceApart, zoomExponent) < minOrthZoom)
        {
            Camera.main.orthographicSize = minOrthZoom;
        }
        else if (Mathf.Pow(distanceApart, zoomExponent) > maxOrthZoom)
        {
            Camera.main.orthographicSize = maxOrthZoom;
        }
        else
        {
            Camera.main.orthographicSize = Mathf.Pow(distanceApart, zoomExponent);
        }
    }
}
