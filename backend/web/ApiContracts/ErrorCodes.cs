using System.Text.Json.Serialization;

namespace backend.ApiContracts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SignUpErrorCodes
{
    UserAlreadyExists,
    PasswordTooSimple,
    PasswordTooLong
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BidErrorCodes
{
    BidFailed,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UpdateAuctionErrorCodes
{
    AuctionIsClosed,
}
