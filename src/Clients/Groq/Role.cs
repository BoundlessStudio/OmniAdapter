﻿using System.Runtime.Serialization;

namespace Boundless.OmniAdapter.Groq;

public enum Role
{
  //[EnumMember(Value = "system")]
  System = 1,
  //[EnumMember(Value = "assistant")]
  Assistant,
  //[EnumMember(Value = "user")]
  User,
  //[EnumMember(Value = "tool")]
  Tool
}