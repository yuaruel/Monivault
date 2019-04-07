namespace Monivault.Web.Models.TopUpSaving
{
    public class OneCardPinViewModel
    {
        public string Pin { get; set; }

        public string Comment { get; set; }

        public string RequestOriginatingPlatform { get; set; }

        public string PlatformSpecificDetail { get; set; }      
    }

    public class TopUpRequestOriginatingPlatform
    {
        public const string Web = "Web";
        public const string Mobile = "Mobile";
    }
}