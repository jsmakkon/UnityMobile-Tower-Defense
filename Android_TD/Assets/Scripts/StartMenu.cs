using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	public void LoadGameScene() {
		
		SceneManager.LoadScene ("GameScene");
	}
}
