namespace localsound.backend.Domain.ModelAdaptor
{
    public class JwtSettingsAdaptor
    {
        public const string JwtSettings = "JwtKeySecret";

        public string Secret { get; set; }
    }
}
