﻿using System;
using System.Collections.Generic;

namespace IAsset.Models.DTOs
{
  public class Coord
  {
    public double lon { get; set; }
    public double lat { get; set; }
  }

  public class Weather
  {
    public double id { get; set; }
    public string main { get; set; }
    public string description { get; set; }
    public string icon { get; set; }
  }

  public class Main
  {
    public double temp { get; set; }
    public double pressure { get; set; }
    public double humidity { get; set; }
    public double temp_min { get; set; }
    public double temp_max { get; set; }
  }

  public class Wind
  {
    public double speed { get; set; }
  }

  public class Clouds
  {
    public double all { get; set; }
  }

  public class Sys
  {
    public double type { get; set; }
    public double id { get; set; }
    public double message { get; set; }
    public string country { get; set; }
    public double sunrise { get; set; }
    public double sunset { get; set; }
  }

  public class CityInfo
  {
    public Coord coord { get; set; }
    public List<Weather> weather { get; set; }
    public string @base { get; set; }
    public Main main { get; set; }
    public double visibility { get; set; }
    public Wind wind { get; set; }
    public Clouds clouds { get; set; }
    public double dt { get; set; }
    public Sys sys { get; set; }
    public double id { get; set; }
    public string name { get; set; }
    public double cod { get; set; }
  }

  public class CityInfoSend
  {
    public string name { get; set; }
    public Coord coord { get; set; }

    // C# 6: Auto-Property Initializer
    public string time { get; set; } = DateTime.Now.ToString("g");
    public Wind wind { get; set; }
    public double visibility { get; set; }
    public string skycondition { get; set; }
    public double temp { get; set; }
    public double humidity { get; set; }
    public double pressure { get; set; }
  }
}