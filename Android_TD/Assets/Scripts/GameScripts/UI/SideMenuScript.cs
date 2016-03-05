using UnityEngine;
using System.Collections;

public class SideMenuScript : MonoBehaviour {

	public void ShowHideMenu()
    {
        if (!GetComponent<Animator>().GetBool("Switch"))
            GetComponent<Animator>().SetBool("Switch", true);
        else
            GetComponent<Animator>().SetBool("Switch", false);
    }
}
