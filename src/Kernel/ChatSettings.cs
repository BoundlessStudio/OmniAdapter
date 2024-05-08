using System;
using System.Collections.Generic;
using System.Text;

namespace Boundless.OmniAdapter.Kernel;

public class ChatSettings
{
  public ChatSettings(string model, int? maxTokens = null, double? temperature = null)
  {
    this.Model = model;
    this.MaxTokens = maxTokens;
    this.Temperature = temperature;
  }

  public string Model { get; set; }
  public int? MaxTokens { get; set; }
  public double? Temperature { get; set; }
}