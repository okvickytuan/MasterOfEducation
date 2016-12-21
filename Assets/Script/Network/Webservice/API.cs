using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum API {

	//LOGIN
	LoginFailed = 0,
	LoginSuccess = 1,

	//REGISTER
	RegisterFailed = 2,
	UsernameHasAlreadyExist = 3,
	RegisterSuccess = 4,

	//DATABASE CONNECTION
	DatabaseConnected = 5,
	DatabaseCannotConnect = 6,
	DatabaseConnectionTimeOut = 7,

	//CLOSEST
	ChangeClosestSuccess = 8,
	ChangeClosestFailed = 9,

	//QUESTION
	AnswerCorrect = 12,
	AnswerWrong = 13,
	QuestionNotExist = 14

}
