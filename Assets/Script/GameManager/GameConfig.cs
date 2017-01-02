
using UnityEngine;
using System.Collections;

public class GameConfig {

	//internal static string SERVER_URL = "http://1.52.64.52:19000/MOEWebservice/";
	internal static string SERVER_URL = "http://127.0.0.1:7000/MOEWebservice/";

	internal static string LOGIN_URL = GameConfig.SERVER_URL + "Login/Login.php";
	internal static string REGISTER_URL = GameConfig.SERVER_URL + "Login/Register.php";
	internal static string KEEPCONNECTION_URL = GameConfig.SERVER_URL + "Connecting/KeepConnection.php";

	internal static string CHANGE_CLOSEST_URL = GameConfig.SERVER_URL + "Closest/ChangeClosest.php";

	internal static string QUESTION_URL = GameConfig.SERVER_URL + "Question/Question.php";
	internal static string CHECK_ANSWER_URL = GameConfig.SERVER_URL + "Question/CheckAnswer.php";

	internal static string ROLL_DICE_URL = GameConfig.SERVER_URL + "Dice/RollDice.php";
}
