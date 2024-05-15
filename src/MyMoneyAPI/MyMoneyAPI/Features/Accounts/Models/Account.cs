using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyMoneyAPI.Features.Accounts.Models;

public class Account
{
    public string id { get; set; }
    public string name { get; set; }
    public string userId { get; set; }
    public string currency { get; set; }
    public bool isDeleted { get; set; }
    public decimal amount { get; set; }
}