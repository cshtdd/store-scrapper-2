using System;
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
      Format().Should().BeEmpty();
    }

    [Fact]
    public void FormatsFieldNames()
    {
      AssertFormat("Key1:null", "Key1", null);
      AssertFormat("Key1:null, Key2:null", "Key1", null, "Key2", null);
      AssertFormat("\"Key Name\":null, Key2:null", "Key Name", null, "Key2", null);
      AssertFormat("Key1:null", "Ke\"y1", null);
      AssertFormat("\"Key 1\":null", "Ke\"y 1", null);
      AssertFormat("Key_1234567890:null", "Ke+)(*&^%$#@!`~=][{}';:/.,<>?y-_1234567890", null);
    }

    [Fact]
    public void FieldNamesCannotBeEmpty()
    {
      ((Action) (() =>
      {
        Format(null, null, "Key2", null);
      })).Should().Throw<ArgumentException>();
      
      ((Action) (() =>
      {
        Format("", null, "Key2", null);
      })).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void FieldNamesMustBeStrings()
    {
      ((Action) (() =>
      {
        Format(12, "value");
      })).Should().Throw<ArgumentException>();
      
      ((Action) (() =>
      {
        Format(DateTime.Now, "value");
      })).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void KeysCannotBeDuplicated()
    {
      ((Action) (() =>
      {
        Format("key1", "value1", "key1", "value2");
      })).Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void ParamsMustBeEvenNumbered()
    {
      ((Action) (() =>
      {
        Format(null);
      })).Should().Throw<ArgumentException>();
      
      ((Action) (() =>
      {
        Format(string.Empty);
      })).Should().Throw<ArgumentException>();
      
      ((Action) (() =>
      {
        Format("key1");
      })).Should().Throw<ArgumentException>();
      
      ((Action) (() =>
      {
        Format("key1", "value1", "key1");
      })).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void FormatsFieldValues()
    {
      AssertFormat("Key1:null", "Key1", null);
      AssertFormat("Key1:\"\"", "Key1", "");
      AssertFormat("Key1:\" \"", "Key1", " ");
      AssertFormat("Key1:\"\", Key2:12", "Key1", "","Key2", 12);
      AssertFormat("Key1:\"pepe\\\"lolo\\\"\"", "Key1", "pepe\"lolo\"");
      AssertFormat("Key1:\"el pepe\\\"lolo\\\"\"", "Key1", "el pepe\"lolo\"");
      AssertFormat("Key2:\"1'2\"", "Key2", "1'2");
      AssertFormat("Key2:\"1:2\"", "Key2", "1:2");
      AssertFormat("Key2:\"1{2\"", "Key2", "1{2");
      AssertFormat("Key2:\"1}2\"", "Key2", "1}2");
      AssertFormat("Key2:\"1(2\"", "Key2", "1(2");
      AssertFormat("Key2:\"1)2\"", "Key2", "1)2");
      AssertFormat("Key2:\"1-2\"", "Key2", "1-2");
      AssertFormat("Key2:\"1,2\"", "Key2", "1,2");
    }
    
    private void AssertFormat(string expected, params object[] kvp)
    {
      Format(kvp).Should().Be(expected);  
    }
    
    private string Format(params object[] kvp)
    {
      return LogFormatter.Format(kvp);
    }
  }
}