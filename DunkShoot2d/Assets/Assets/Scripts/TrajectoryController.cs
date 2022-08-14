using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryController : MonoBehaviour
{
    
    [SerializeField] private GameObject _ball;

    private LineRenderer _line;
    private void Start()
    {
        _line = GetComponent<LineRenderer>();
        InputManager.instance.OnMouseUp.AddListener(SetPosCountLine);
    }


    private void SetPosCountLine()
    {
        _line.positionCount = 0;
    }

    public void ShowTrajectory(Vector3 speed)
    {
        // подготовка
        var ball = Instantiate(_ball, transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody2D>().AddForce(speed, ForceMode2D.Impulse);

        Physics2D.simulationMode = SimulationMode2D.Script;
        //симуляция
        Vector3[] points = new Vector3[50];
        _line.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            Physics2D.Simulate(Time.fixedDeltaTime);
            points[i] = ball.transform.position;
        }

        _line.SetPositions(points);


        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        //зачистка
        Destroy(ball);
    }
}
