using System.Runtime.Serialization;

namespace Boundless.OmniAdapter.Models;

public enum Role
{
  Unknown = 0,
  System,
  Assistant,
  User,
  Tool
}