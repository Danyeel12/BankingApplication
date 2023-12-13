using Banking_Application;
class SavingsAccount : Account
{
    //fields
    private static double COST_PER_TRANSACTION = 0.05;
    private static double INTEREST_RATE = 0.015;
    //properties

    //methods
    public SavingsAccount(double balance = 0, bool hasOverDraft = false) : base("SV-", balance)
    {

    }
    public new void Deposit(double amount, Person person)
    {
        if (!IsHolder(person.Name) || IsHolder(person.Name))
        {
            base.Deposit(amount, person);
        }

    }
    public void Withdraw(double amount, Person person)
    {
        if (!IsHolder(person.Name))
        {
            throw new AccountException(AccountException.NAME_NOT_ASSOCIATED_WITH_ACCOUNT);
        }

        if (!person.IsAuthenticated)
        {
            throw new AccountException(AccountException.USER_NOT_LOGGED_IN);
        }

        if (amount > Balance)
        {
            throw new AccountException(AccountException.NO_OVERDRAFT);
        }

        base.Deposit(-amount, person);

    }
    public override void PrepareMonthlyReport()
    {
        double serviceCharge = transactions.Count * COST_PER_TRANSACTION;
        double interest = LowestBalance * INTEREST_RATE / 12;
        Balance += interest - serviceCharge;
        transactions.Clear();
    }
}
