using System.Collections.Generic;

namespace IAsset.Test
{
  public static class TestHelpers
  {
    public const string OneCity = "OneCity";
    public const string NoData = "NoData";
    public const string Incorrect = "Incorrect";
    public const string ManyCities = "ManyCities";

    // C# 6: Index Initializers
    public static Dictionary<string, string> Cities = new Dictionary<string, string>
    {
      { NoData, null },
      { OneCity, "<NewDataSet><Table><Country>Germany</Country><City>Berlin-Schoenefeld</City></Table></NewDataSet>" } ,
      { ManyCities, "<NewDataSet><Table><Country>Germany</Country><City>Berlin-Schoenefeld</City></Table><Table><Country>Germany</Country><City>Dresden-Klotzsche</City></Table><Table><Country>Germany</Country><City>Erfurt-Bindersleben</City></Table><Table><Country>Germany</Country><City>Frankfurt / M-Flughafen</City></Table><Table><Country>Germany</Country><City>Muenster / Osnabrueck</City></Table></NewDataSet>" },
      { Incorrect, "IncorrectXml" }
    };
  }
}