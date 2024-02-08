namespace backend.ApiContracts;

public class StakeContract
{
    public string Username { get; set; }
    public decimal Value { get; set; }
    public DateTime Timestamp { get; set; }
}