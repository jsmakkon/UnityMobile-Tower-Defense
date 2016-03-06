using UnityEngine;
using System.Collections;
/* Carries data through */
public class DataCarrierScript : MonoBehaviour {

    public static bool created;

    public int seed = -1;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(transform.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
