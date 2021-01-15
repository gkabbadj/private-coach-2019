using System.Collections;
using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class ModelToBody25 : MonoBehaviour
{
    //--------------------- PILS ------------------------------//
    public Text error_textbox; //en écriture, pour afficher les erreurs
    public Text isVisible_textBox; //que en lecture, pour voir si le corps est visible
    public List<Transform> poseStandard;
    private int globalCounter;
    public GameObject ball;
    private GameObject[] Myballs= new GameObject[6];
    private MeshRenderer[] renderballs= new MeshRenderer[6];
    public static string [] messages = {"Error on the right shoulder : ",
    "Error on the left shoulder : ", "Error on the right hip : ",
    "Error on the left hip : ", "Error on the right knee : ", "Error on the left knee : "};
    public static string [,] advices = {{"levez le bras", "baissez le bras"}, {"levez le bras", "baissez le bras"},
    {"Descendez sur vos cuisses", "Montez sur vos cuisses"}, {"Descendez sur vos cuisses", "Montez sur vos cuisses"}, {"Montez sur vos cuisses", "Descendez sur vos cuisses"}, 
    {"Montez sur vos cuisses 1", "Descendez sur vos cuisses 1"}};
    public int [,] errorCounter = new int [6,100];
    public int [,] moyPointScore = new int [6,2]; //pour chaque point : [sum, moy]

    public int [] written = {0,0,0,0,0,0};
    //--------------------- PILS ------------------------------/
    private void Awake()
    {
        globalCounter = 0;
        ball = Resources.Load<GameObject>("Sphere") as GameObject;
        for (int i = 0; i < 6; i++)
        {
                Myballs[i]=Instantiate(ball, poseStandard[EvaluateAngle.OP_anglePoints[i, 1]].transform);
                renderballs[i] = Myballs[i].GetComponent<MeshRenderer>();
                renderballs[i].enabled = false;
        }
    }

    private int ratioCol(int [,] tab, int col){
        //retourne un pourcentage (entier)
        int sum = 0;
        for(int i=0;i<tab.GetLength(1);i++){
            sum += tab[col,i];
        }
        UnityEngine.Debug.Log("RATIO : "+sum.ToString()+"   COL : "+col.ToString());
        return sum;
    }

    private  void Update()
    {
        bool there_is_an_error = false;

        if (isVisible_textBox.text != "full body not visible"){
            globalCounter +=1;

            for (int i = 0; i < 6; i++) {
                if (EvaluateAngle.PointScore[i,0] == 0) //cas spécial
                {
                    renderballs[i].enabled = true;
                    renderballs[i].material.color = Color.gray;
                }
                else if (EvaluateAngle.PointScore[i,0] < 0.7) // il y a une erreur
                {
                    moyPointScore[i,0] += 1;
                    moyPointScore[i,1] = (int) ((100*moyPointScore[i,0]) / globalCounter);
                    there_is_an_error = true;

                    errorCounter[i,globalCounter%errorCounter.GetLength(1)] = 1;
                    
                    //mise à jour affichage
                    if(globalCounter%40==0){
                        UnityEngine.Debug.Log(moyPointScore[i,1]);
                        if (ratioCol(errorCounter, i) > 50) {       //>50%
                            UnityEngine.Debug.Log("in ERROR");
                            if (written[i] == 0) { //affectation du niveau de priorité
                                written[i] = i+1;
                            }
                            if (written.Max() == written[i]) { //vérification de la priorité
                                error_textbox.text = messages[i] + advices[i, (int) EvaluateAngle.PointScore[i,1]];
                                UnityEngine.Debug.Log(EvaluateAngle.PointScore[i,1].ToString());

                                //update stat
                                StaticItems.ErrorMessage = messages[i] + advices[i, (int) EvaluateAngle.PointScore[i,1]];
                                StaticItems.ErrorTime = DateTime.Now.ToString();
                            }
                        }
                    }
                    renderballs[i].enabled = true;
                    if (EvaluateAngle.PointScore[i,1] == 1) // angle trop grand -> rouge
                        renderballs[i].material.color = new Color(1 - EvaluateAngle.PointScore[i,0], 0, 0, 1);
                    else
                        renderballs[i].material.color = new Color(0, 0, 1 - EvaluateAngle.PointScore[i,0], 1);
                }
                else // cas où il n'y a pas d'erreur
                {
                    errorCounter[i,globalCounter%errorCounter.GetLength(1)] = 0;
                    renderballs[i].enabled = false;
                    written[i] = 0;
                }   
            }
            if (there_is_an_error == false){
                error_textbox.text = "";
            } 
        }

    }
}
