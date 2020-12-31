using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStolenfurniture : MonoBehaviour
{

    public GameObject pointCounter, showFurniture;
    void Start()
    {
        showFurniture.GetComponent<Text>().text = "Theif stole "+pointCounter.GetComponent<Text>().text+" pieces of furniture";
    }


}
