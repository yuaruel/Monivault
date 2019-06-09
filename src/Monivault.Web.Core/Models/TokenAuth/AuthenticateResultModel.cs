namespace Monivault.AppModels.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AccountHolderId { get; set; }
    }
}
