using UnityEngine;
using System.Collections;

public class SelectedHexa : MonoBehaviour {

    GameObject selectedHexa = null;

    public void setSelectedHexa (GameObject obj, bool select)
    {
        selectedHexa = obj;
        obj.GetComponent<MapHexa>().setSelected(select);
    }

    public void switchSelectedHexa(GameObject obj)
    {
        if (obj == selectedHexa)
        {
            obj.GetComponent<MapHexa>().setSelected(false);
            selectedHexa = null;
        }
        else
        {
            if (selectedHexa != null)
                selectedHexa.GetComponent<MapHexa>().setSelected(false);
            selectedHexa = obj;
            obj.GetComponent<MapHexa>().setSelected(true);
        }
    }

    public GameObject getSelected()
    {
        return selectedHexa;
    }
}
