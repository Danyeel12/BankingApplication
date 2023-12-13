using Banking_Application;
class CheckingAccount : Account
{
    private static readonly double COST_PER_TRANSACTION = 0.05;
    private static readonly double INTEREST_RATE = 0.005;
    private bool hasOverdraft;

    public CheckingAccount(double balance = 0, bool hasOverdraft = false) : base("CK-", balance)
    {
        this.hasOverdraft = hasOverdraft;
    }

    // Methods
    public new void Deposit(double amount, Person person)
    {
        if (IsHolder(person.Name)||!IsHolder(person.Name))
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

        if (amount > Balance && !hasOverdraft)
        {
            throw new AccountException(AccountException.NO_OVERDRAFT);
        }

        base.Deposit(-amount, person);
    }
    public override void PrepareMonthlyReport()
    {
        double serviceCharge = transactions.Count * COST_PER_TRANSACTION;
        double interest = (Balance * INTEREST_RATE) / 12;
        Balance += interest - serviceCharge;
        transactions.Clear();
    }
}

