using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class level1loder : MonoBehaviour
{

public int level ;

public void changescene(){
level= Random.Range(1 , 1) ;
 SceneManager.LoadScene(level) ;
}

}
