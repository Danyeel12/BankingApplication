using Banking_Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


abstract class Account
{
    //        Fields
    private static int CURRENT_NUMBER;
    protected readonly List<Transaction> transactions = new List<Transaction>();
    protected readonly List<Person> holders = new List<Person>();
    public readonly string Number;
    //-$ CURRENT_NUMBER : int
    //# transactions : List<Transaction>
    //# holders : List<Person>
    //+ «readonly» Number : string

    //          Properties
    public double Balance { get; protected set; }
    public double LowestBalance { get; protected set; }
    //+ «property protected setter» Balance : double
    //+ «property protected setter» LowestBalance : double

    //      Methods
    static Account()
    {
        CURRENT_NUMBER = 100000;
    }
    public Account(string type, double balance)
    {
        Balance = balance;
        LowestBalance = balance;
        Number = type + CURRENT_NUMBER;    //VS-100000
        CURRENT_NUMBER++;
    }
    public void AddUser(Person person) 
    {
        holders.Add(person);
    }

    public void Deposit(double amount, Person person)
    {
        Balance += amount;
        if (Balance < LowestBalance) { LowestBalance = Balance; }
        Transaction transact = new Transaction(Number, amount, Balance, person, DateTime.Now);
        transactions.Add(transact);
    }
    public bool IsHolder(string name) {
        foreach (Person person in holders)
        {
            if (person.Name == name)
            {
                return true;
            }
        }
        return false;
    }
    public abstract void PrepareMonthlyReport();
    public override string ToString()
    {
        Console.WriteLine();
        string result = $"Account Number: {Number}\nUsers: ";
        foreach (Person person in holders)
        {
            result += $"{person.Name}, ";
        }
        result += $"\nBalance: {Balance:C2}\nTransactions:\n";
        foreach (Transaction transaction in transactions)
        {
            result += $"{transaction}\n";
        }
        return result;
    }
   
}
