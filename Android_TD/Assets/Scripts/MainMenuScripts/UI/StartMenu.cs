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
            PlayerPrefs.SetString("GameSeed", seed);
        }
		SceneManager.LoadScene ("GameScene");
	}
}
