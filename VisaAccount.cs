using Banking_Application;
using System;

class VisaAccount : Account
{
    //fields
    private static double _INTEREST_RATE = 0.1995;
    private double _creditLimit;
    //    -$ INTEREST_RATE 		: double
    //- credilLimit	: double

    //Properties

    //Methods

    public VisaAccount(double balance = 0, double creditLimit = 1200) : base("VS-", balance)
        => (_creditLimit) = (creditLimit);
    public double CreditLimit
    {
        get { return _creditLimit; }
        set { _creditLimit = value; }
    }
    public void DoPayment(double amount, Person person)
        => Deposit(amount, person);
    public void DoPurchase(double amount, Person person)
    {
        if (!holders.Contains(person))
        {
            throw new AccountException(AccountException.NAME_NOT_ASSOCIATED_WITH_ACCOUNT);
        }

        if (!person.IsAuthenticated)
        {
            throw new AccountException(AccountException.USER_NOT_LOGGED_IN);
        }

        if (Balance - amount < -_creditLimit)
        {
            throw new AccountException(AccountException.CREDIT_LIMIT_HAS_BEEN_EXCEEDED);
        }

        Deposit(-amount, person);
    }
    public override void PrepareMonthlyReport()
    {
        double interest = LowestBalance * (_INTEREST_RATE / 12);
        Balance -= interest;
        transactions.Clear();
    }
    //+ «constructor» VisaAccount(balance : double, creditLimit : double)
    //+ DoPayment(amount: double, person Person) : : void
    //+ DoPurchase(amount: double, person Person) : : void
    //+ PrepareMonthlyReport() : : void
}

