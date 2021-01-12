using System.Collections;
using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ModelToBody25 : MonoBehaviour
{
    //--------------------- PILS ------------------------------//
    public List<Transform> poseStandard;
    private bool conditionGUI;
    private bool showLabel;
    public GameObject ball;
    private GameObject[] Myballs= new GameObject[6];
    private string MessageToDisplay;
    private MeshRenderer[] renderballs= new MeshRenderer[6];
    public static string [] messages = {"Error on the right shoulder : ",
    "Error on the left shoulder : ", "Error on the right hip : ",
    "Error on the left hip : ", "Error on the right knee : ", "Error on the left knee : "};
    public static string [,] advices = {["Raise your shoulder", "Lower your shoulder"], ["Raise your shoulder", "Lower your shoulder"],
    ["Get your torso up", "Get your torso down"], ["Get your torso up", "Get your torso down"], ["Get your thighs up", "Get your thighs down"], [
    "Get your thighs up", "Get your thighs down"]};
    Stopwatch sw1 = new Stopwatch();
    Stopwatch sw2 = new Stopwatch();
    Stopwatch sw3 = new Stopwatch();
    Stopwatch sw4 = new Stopwatch();
    Stopwatch sw5 = new Stopwatch();
    Stopwatch sw6 = new Stopwatch();

    private long [] counter = {0,0,0,0,0,0};
    public int [] written = {0,0,0,0,0,0};
    //--------------------- PILS ------------------------------/
    private void Awake()
    {
        ball = Resources.Load<GameObject>("Sphere") as GameObject;
        for (int i = 0; i < 6; i++)
        {
                Myballs[i]=Instantiate(ball, poseStandard[EvaluateAngle.OP_anglePoints[i, 1]].transform);
                renderballs[i] = Myballs[i].GetComponent<MeshRenderer>();
                renderballs[i].enabled = false;
        }
        
    }

    private Stopwatch callsw(int index)
    {
        Stopwatch sw = new Stopwatch();
        if (index == 1) {
            return sw1;
        }
        if (index == 2) {
            return sw2;
        }
        if (index == 3) {
            return sw3;
        }
        if (index == 4) {
            return sw4;
        }
        if (index == 5) {
            return sw5;
        }
        if (index == 6) {
            return sw6;
        }
        else {
            return sw;
        }
    }

    private void OnGUI()
    {
        if (conditionGUI) {
            GUI.color = Color.red;
            GUI.Label(new Rect(550, 300, 150, 100), MessageToDisplay);
            Thread.Sleep(1000);
            }
    }

    private void Update()
    {
        for (int i = 0; i < 6; i++) {
            if (EvaluateAngle.PointScore[i,0] == 0)
            {
                renderballs[i].enabled = true;
                renderballs[i].material.color = Color.gray;
            }
            else if (EvaluateAngle.PointScore[i,0] < 0.7)
            {
                if (counter[i] == 0) {
                    callsw(i).Start();
                }
                counter[i] = callsw(i).ElapsedMilliseconds;
                if (counter[i] >= 5000) {
                    if (written[i] == 0) {
                        written[i] = i;
                        if (written.Max() == written[i]) {
                            conditionGUI = true;
                            MessageToDisplay = messages[i];
                            UnityEngine.Debug.Log(messages[i]);
                        } else {
                            written[i] = 0;
                        }
                    }
                }
                renderballs[i].enabled = true;
                renderballs[i].material.color = new Color(1 - EvaluateAngle.PointScore[i,0], 0, 0, 1);
            }
            else
            {
                renderballs[i].enabled = false;
                conditionGUI = false;
                callsw(i).Reset();
                counter[i] = 0;
                written[i] = 0;
            }   
        } 

    }
}
