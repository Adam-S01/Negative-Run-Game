using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScript : MonoBehaviour
{
    public GameObject tuto;

   public void LaunchGame()
   {
       SceneManager.LoadScene(1);
   }

   public void QuitGame()
   {
       Application.Quit();
   }

   public void LaunchTuto()
   {
       tuto.SetActive(true);
   }

   public void CloseTuto()
   {
       tuto.SetActive(false);
   }
}
