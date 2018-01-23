using System;
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
      
      format(new Dictionary<string, object>
      {
        {"Key Name", null},
        {"Key2", null}
      }, "\"Key Name\":null, Key2:null");
      
      format(new Dictionary<string, object>
      {
        {"Ke\"y1", null}
      }, "Key1:null");
      
      format(new Dictionary<string, object>
      {
        {"Ke\"y 1", null}
      }, "\"Key 1\":null");
      
      format(new Dictionary<string, object>
      {
        {"Ke+)(*&^%$#@!`~=][{}';:/.,<>?y-_1234567890", null}
      }, "Key_1234567890:null");
    }

    [Fact]
    public void FieldNamesCannotBeEmpty()
    {
      ((Action) (() =>
      {
        LogFormatter.Format(new Dictionary<string, object>
        {
          {null, null},
          {"Key2", null}
        });
      })).ShouldThrow<ArgumentException>();
      
      ((Action) (() =>
      {
        LogFormatter.Format(new Dictionary<string, object>
        {
          {"", null},
          {"Key2", null}
        });
      })).ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void FormatsFielValues()
    {
      format(new Dictionary<string, object>
      {
        {"Key1", null}
      }, "Key1:null");
      
      format(new Dictionary<string, object>
      {
        {"Key1", ""}
      }, "Key1:\"\"");
      
      format(new Dictionary<string, object>
      {
        {"Key1", " "}
      }, "Key1:\" \"");
      
      format(new Dictionary<string, object>
      {
        {"Key1", ""},
        {"Key2", 12}
      }, "Key1:\"\", Key2:12");
      
      format(new Dictionary<string, object>
      {
        {"Key1", "pepe\"lolo\""}
      }, "Key1:\"pepe\\\"lolo\\\"\"");
      
      format(new Dictionary<string, object>
      {
        {"Key1", "el pepe\"lolo\""}
      }, "Key1:\"el pepe\\\"lolo\\\"\"");
      
      format(new Dictionary<string, object>
      {
        {"Key2", "1'2"}
      }, "Key2:\"1'2\"");
      
      format(new Dictionary<string, object>
      {
        {"Key2", "1:2"}
      }, "Key2:\"1:2\"");
      
      format(new Dictionary<string, object>
      {
        {"Key2", "1{2"}
      }, "Key2:\"1{2\"");
      
      format(new Dictionary<string, object>
      {
        {"Key2", "1}2"}
      }, "Key2:\"1}2\"");
      
      format(new Dictionary<string, object>
      {
        {"Key2", "1(2"}
      }, "Key2:\"1(2\"");
      
      format(new Dictionary<string, object>
      {
        {"Key2", "1)2"}
      }, "Key2:\"1)2\"");
      
      format(new Dictionary<string, object>
      {
        {"Key2", "1-2"}
      }, "Key2:\"1-2\"");
      
      format(new Dictionary<string, object>
      {
        {"Key2", "1,2"}
      }, "Key2:\"1,2\"");
    }
    
    private void format(IDictionary<string, object> input, string expected)
    {
      LogFormatter.Format(input).Should().Be(expected);      
    }
  }
}