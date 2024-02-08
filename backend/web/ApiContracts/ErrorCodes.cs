using System.Text.Json.Serialization;

namespace backend.ApiContracts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CreateUserErrorCodes
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
