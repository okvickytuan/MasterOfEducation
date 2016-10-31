using UnityEngine;
using System.Collections;

public class GameConfig {

	internal static string SERVER_URL = "http://127.0.0.1/MasterOfEducation/";

	internal static string LOGIN_URL = GameConfig.SERVER_URL + "Login/Login.php";
	internal static string REGISTER_URL = GameConfig.SERVER_URL + "Login/Register.php";
	internal static string KEEPCONNECTION_URL = GameConfig.SERVER_URL + "Connecting/KeepConnection.php";

	internal static string QUESTION_URL = GameConfig.SERVER_URL + "Question/Question.php";
	internal static string CHECK_ANSWER_URL = GameConfig.SERVER_URL + "Question/CheckAnswer.php";

}
