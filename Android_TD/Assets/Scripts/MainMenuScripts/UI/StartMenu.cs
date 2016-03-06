using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	//New Game Button
	public void LoadGameScene() {
        string seed = GameObject.Find("SeedField").GetComponent<UnityEngine.UI.InputField>().text;
        int seedInt = 0;
        if (int.TryParse(seed, out seedInt))
        {
            GameObject.Find("DataCarrier").GetComponent<DataCarrierScript>().seed = int.Parse(seed);
        }
		SceneManager.LoadScene ("GameScene");
	}
}
