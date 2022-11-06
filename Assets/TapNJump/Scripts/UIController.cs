using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

	// Use this for initialization


	public Text scoreTextMesh, finalScoreTextMesh, BestScoreTextMesh;
	int inGameScore = 0;
	public GameObject newScoreObj, ScoreBoard, MainMenuContainer, Tutorial, QuitButton, assetStoreReviewText, hudmenu;
	 
	public static bool isRestartPressed = false;
	bool readyToPlay = false;

	void OnEnable ()
	{

		UnityChan2DController.IncreaseScore += OnScoreIncrease;
		UnityChan2DController.PlayerDead += OnPlayerDead;

	
		MainMenuContainer.SetActive (true);
		newScoreObj.SetActive (false);
		ScoreBoard.SetActive (false);
		hudmenu.SetActive (false);

		//on game end scoreboard menu ,if user presses on restart ,
		//we need to disable the mainmenu and changing player to readyto play.
		if (isRestartPressed) {
			MainMenuContainer.SetActive (false);
			isRestartPressed = false;
			readyToPlay = true;
			hudmenu.SetActive (true);
			UnityChan2DController.currentState = UnityChan2DController.playerStates.alive;

		} else {
			UnityChan2DController.currentState = UnityChan2DController.playerStates.idle;

		}

		scoreTextMesh.text = "" + 0;


		#if UNITY_IOS
		//apple won't like quit button
		Destroy(QuitButton);
		#endif
		//to display messsage to developers
 
		#if UNITY_EDITOR

		assetStoreReviewText.SetActive (true);
		#else
		assetStoreReviewText.SetActive(false);
		#endif

	}
	 
	// Update is called once per frame
	RaycastHit hit;

	void Update ()
	{

 
	
		//to handle escape key
		if (Input.GetKeyUp (KeyCode.Escape)) {
			if (UnityChan2DController.currentState == UnityChan2DController.playerStates.idle) {
				
				Application.Quit ();
			} else {
				
				restart ();
			}
			
		}
	}

	public void OnButtonClicks (string incomingName)
	{
		Debug.Log ("clicked on " + incomingName);
		SoundController.Static.PlayClickSound ();

		switch (incomingName) {
		case "Play":
			MainMenuContainer.SetActive (false);
			Tutorial.SetActive (true);
			readyToPlay = true;
			UnityChan2DController.currentState = UnityChan2DController.playerStates.alive;

			hudmenu.SetActive (true);
			Invoke ("lateDeactivateTutorial", 3);
			break;
		case "Home":
			
			restart ();
			break;
			
		case "Restart":
			isRestartPressed = true;
		
			hudmenu.SetActive (true);
			restart ();

			break;
			
		case "Facebook":
			Application.OpenURL ("https://www.facebook.com/AceGamesHyderabad?ref=hl");
			break;
			
		case "Review":
			#if UNITY_ANDROID
			Application.OpenURL ("https://www.assetstore.unity3d.com/en/#!/publisher/920/page=1/sortby=popularity");
			#endif
			#if UNITY_IOS
			Application.OpenURL ("https://www.assetstore.unity3d.com/en/#!/publisher/920/page=1/sortby=popularity");
			#endif
			#if UNITY_WEBPLAYER
			Application.OpenURL ("https://www.assetstore.unity3d.com/en/#!/publisher/920/page=1/sortby=popularity");
			#endif
			#if UNITY_EDITOR
			Application.OpenURL ("https://www.assetstore.unity3d.com/en/#!/publisher/920/page=1/sortby=popularity");
			#endif
			
			break;
			
		case "Quit":
			
			Application.Quit ();
			
			break;
		}
	}

	public void lateDeactivateTutorial ()
	{
		Tutorial.SetActive (false);
	}

	public void restart ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		//Application.LoadLevel (Application.loadedLevelName);
	}

	void OnScoreIncrease (System.Object obj, EventArgs args)
	{
		inGameScore++;
		scoreTextMesh.text = "" + inGameScore;
		 
		SoundController.Static.PlayScoreIncrease ();
	}

	void OnPlayerDead (System.Object obj, EventArgs args)
	{
		hudmenu.SetActive (false);
		ScoreBoard.SetActive (true);

      
		finalScoreTextMesh.text = "Score : " + inGameScore;

		if (PlayerPrefs.GetInt ("BestScore", 0) < inGameScore) {
			PlayerPrefs.SetInt ("BestScore", inGameScore);
			newScoreObj.SetActive (true);
		}

		BestScoreTextMesh.text = "Best : " + PlayerPrefs.GetInt ("BestScore", inGameScore);

	}

	void OnDisable ()
	{

		UnityChan2DController.IncreaseScore -= OnScoreIncrease;
		UnityChan2DController.PlayerDead -= OnPlayerDead;

		 
	}
}
