using System.Runtime.Serialization;

namespace Boundless.OmniAdapter.OpenAi.Models.Image
{
    public enum ImageResponseFormat
    {
        [EnumMember(Value = "url")]
        Url,
        [EnumMember(Value = "b64_json")]
        B64_Json
    }
}
