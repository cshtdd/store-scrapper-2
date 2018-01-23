using System.Collections.Generic;
using FluentAssertions;
using store_scrapper_2.Logging;
using Xunit;

namespace store_scrapper_2_Tests.Logging
{
  public class LogFormatterTest
  {
    [Fact]
    public void FormatsAnEmptyObject()
    {
      format(null, string.Empty);
      format(new Dictionary<string, object>(), string.Empty);
    }

    [Fact]
    public void FormatsFieldNames()
    {
      format(new Dictionary<string, object>
      {
        {"Key1", null}
      }, "Key1:null");
      
      format(new Dictionary<string, object>
      {
        {"Key1", null},
        {"Key2", null}
      }, "Key1:null, Key2:null");
    }

    private void format(IDictionary<string, object> input, string expected)
    {
      LogFormatter.Format(input).Should().Be(expected);      
    }
  }
}