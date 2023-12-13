using Banking_Application;
public class AccountException : Exception
{
    //fields
    public const string ACCOUNT_DOES_NOT_EXIST = "Account does not exist";
    public const string CREDIT_LIMIT_HAS_BEEN_EXCEEDED = "Credit limit has been exceeded";
    public const string NAME_NOT_ASSOCIATED_WITH_ACCOUNT = "Name not associated with account";
    public const string NO_OVERDRAFT = "No overdraft";
    public const string PASSWORD_INCORRECT = "Incorrect Password";
    public const string USER_DOES_NOT_EXIST = "User dont exist.";
    public const string USER_NOT_LOGGED_IN = "User not logged in.";
    //Properties

    //Methods
    public AccountException() : base()
    {

    }
    public AccountException(string reason) : base(reason) { }

}