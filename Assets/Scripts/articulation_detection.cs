using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class articulation_detection : MonoBehaviour
{
    public GameObject[] userArticulations;
    public Text textBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        bool body_is_visible = true;
        for(int i=0 ; i<14 ; i++){
            if (userArticulations[i].activeSelf == false){
                body_is_visible = false;
            }
        }
        if (body_is_visible == false){
            textBox.text = "full body not visible";
            // UnityEngine.Debug.Log("articulation "+i.ToString()+" is missing");
        }
        else{
            textBox.text = "";
        }
    }
}
