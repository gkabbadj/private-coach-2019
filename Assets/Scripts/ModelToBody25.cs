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
    private int globalCounter;
    public GameObject ball;
    private GameObject[] Myballs= new GameObject[6];
    private string MessageToDisplay;
    private MeshRenderer[] renderballs= new MeshRenderer[6];
    public static string [] messages = {"Error on the right shoulder : ",
    "Error on the left shoulder : ", "Error on the right hip : ",
    "Error on the left hip : ", "Error on the right knee : ", "Error on the left knee : "};
    public static string [,] advices = {{"Raise your shoulder", "Lower your shoulder"}, {"Raise your shoulder", "Lower your shoulder"},
    {"Get your torso up", "Get your torso down"}, {"Get your torso up", "Get your torso down"}, {"Get your thighs up", "Get your thighs down"}, {
    "Get your thighs up", "Get your thighs down"}};
    public int [,] errorCounter = new int [6,100];
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
        globalCounter = 0;
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
            }
    }

    private void Update()
    {
        globalCounter += 1;
        for (int i = 0; i < 6; i++) {
            if (EvaluateAngle.PointScore[i,0] == 0)
            {
                renderballs[i].enabled = true;
                renderballs[i].material.color = Color.gray;
            }
            else if (EvaluateAngle.PointScore[i,0] < 0.7)
            {
                //if (counter[i] == 0) {
                //    callsw(i).Start();
                //}
                errorCounter[i, globalCounter%100] = 1;
                //counter[i] = callsw(i).ElapsedMilliseconds;
                if (written[i] >= 60) {
                    if (written[i] == 0) {
                        written[i] = i+1;
                        if (written.Max() == written[i]) {
                            conditionGUI = true;
                            MessageToDisplay = messages[i] + advices [i, (int)EvaluateAngle.PointScore[i,1]];
                            UnityEngine.Debug.Log(MessageToDisplay);
                        } else {
                            written[i] = 0;
                        }
                    }
                } else {
                    conditionGUI = false;
                    written[i] = 0;
                }
                renderballs[i].enabled = true;
                renderballs[i].material.color = new Color(1 - EvaluateAngle.PointScore[i,0], 0, 0, 1);
            }
            else
            {
                renderballs[i].enabled = false;
                errorCounter[i, globalCounter%100] = 0;
                // conditionGUI = false;
                // callsw(i).Reset();
                // counter[i] = 0;
                // written[i] = 0;
            }   
        } 

    }
}
