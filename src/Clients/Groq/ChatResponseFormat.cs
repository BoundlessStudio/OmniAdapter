using System.Runtime.Serialization;

namespace Boundless.OmniAdapter.Groq;

public enum ChatResponseFormat
{
  //[EnumMember(Value = "text")]
  Text,
  //[EnumMember(Value = "json_object")]
  JsonObject
}
