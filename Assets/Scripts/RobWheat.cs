using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using Random = System.Random;

public class RobWheat : MonoBehaviour
{
    private int chanceDrought = 1, chanceCombine = 2, chanceBird = 3;

    private void Start()
    {
        UnityThread.initUnityThread();

        Thread thread = new Thread(() =>
        {
            while (true)
            {
                Random rnd = new Random();
                int chislo = rnd.Next(1, 900);

                if (chislo == chanceDrought)
                {
                    chislo = rnd.Next(0, 64);
                    UnityThread.executeInUpdate(() =>
                    {
                        transform.GetChild(chislo).GetComponent<Renderer>().material.color =
                            new Color32(84, 53, 13, 255);
                        foreach (Transform eachChild in transform.GetChild(chislo))
                        {
                            if (eachChild.name == "P_AncientRuins_Plants20(Clone)")

                                Destroy(eachChild.gameObject);
                        }
                    });
                    Thread.Sleep(100);
                }

                /* if (chislo == chanceCombine)
                 {
                     Debug.Log("Combine! Combine! Combine! Combine!");
                 }
 
                 if (chislo == chanceBird)
                 {
                     Debug.Log("Bird! Bird! Bird! Bird!");
                 }*/
            }
        });

        thread.Start();
    }
}