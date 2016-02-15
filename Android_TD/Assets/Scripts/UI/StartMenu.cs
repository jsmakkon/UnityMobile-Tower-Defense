using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	//New Game Button
	public void LoadGameScene() {
		SceneManager.LoadScene ("GameScene");
	}
}
