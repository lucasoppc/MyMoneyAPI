using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyMoneyAPI.Features.Transactions.Models;

public class Transaction
{
    public string id { get; set; }
    public string accountId { get; set; }
    public string userId { get; set; }
    public string description { get; set; }
    public string currency { get; set; }
    public decimal amount { get; set; }
    public string date { get; set; }
}