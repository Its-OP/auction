namespace backend.ApiContracts;

public enum ErrorCodes
{
    UserAlreadyExists,
    PasswordTooSimple,
    PasswordTooLong,
    
    MissingThumbnail,
    InvalidPageSize,
    InvalidPageNumber
}