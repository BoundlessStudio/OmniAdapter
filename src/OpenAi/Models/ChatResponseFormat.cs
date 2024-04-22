using System.Runtime.Serialization;

namespace Boundless.OmniAdapter.OpenAi.Models;

public enum ChatResponseFormat
{
  //[EnumMember(Value = "text")]
  Text,
  //[EnumMember(Value = "json_object")]
  JsonObject
}
