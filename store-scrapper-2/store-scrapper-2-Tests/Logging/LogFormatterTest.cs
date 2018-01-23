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
      LogFormatter.Format().Should().BeEmpty();
    }

    [Fact]
    public void FormatsFieldNames()
    {
      assertFormat("Key1:null", "Key1", null);
      assertFormat("Key1:null, Key2:null", "Key1", null, "Key2", null);
      assertFormat("\"Key Name\":null, Key2:null", "Key Name", null, "Key2", null);
      assertFormat("Key1:null", "Ke\"y1", null);
      assertFormat("\"Key 1\":null", "Ke\"y 1", null);
      assertFormat("Key_1234567890:null", "Ke+)(*&^%$#@!`~=][{}';:/.,<>?y-_1234567890", null);
    }

    [Fact]
    public void FieldNamesCannotBeEmpty()
    {
      ((Action) (() =>
      {
        LogFormatter.Format(null, null, "Key2", null);
      })).ShouldThrow<ArgumentException>();
      
      ((Action) (() =>
      {
        LogFormatter.Format("", null, "Key2", null);
      })).ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void FieldNamesMustBeStrings()
    {
      ((Action) (() =>
      {
        LogFormatter.Format(12, "value");
      })).ShouldThrow<ArgumentException>();
      
      ((Action) (() =>
      {
        LogFormatter.Format(DateTime.Now, "value");
      })).ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void KeysCannotBeDuplicated()
    {
      ((Action) (() =>
      {
        LogFormatter.Format("key1", "value1", "key1", "value2");
      })).ShouldThrow<ArgumentException>();
    }
    
    [Fact]
    public void ParamsMustBeEvenNumbered()
    {
      ((Action) (() =>
      {
        LogFormatter.Format(null);
      })).ShouldThrow<ArgumentException>();
      
      ((Action) (() =>
      {
        LogFormatter.Format(string.Empty);
      })).ShouldThrow<ArgumentException>();
      
      ((Action) (() =>
      {
        LogFormatter.Format("key1");
      })).ShouldThrow<ArgumentException>();
      
      ((Action) (() =>
      {
        LogFormatter.Format("key1", "value1", "key1");
      })).ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void FormatsFieldValues()
    {
      assertFormat("Key1:null", "Key1", null);
      assertFormat("Key1:\"\"", "Key1", "");
      assertFormat("Key1:\" \"", "Key1", " ");
      assertFormat("Key1:\"\", Key2:12", "Key1", "","Key2", 12);
      assertFormat("Key1:\"pepe\\\"lolo\\\"\"", "Key1", "pepe\"lolo\"");
      assertFormat("Key1:\"el pepe\\\"lolo\\\"\"", "Key1", "el pepe\"lolo\"");
      assertFormat("Key2:\"1'2\"", "Key2", "1'2");
      assertFormat("Key2:\"1:2\"", "Key2", "1:2");
      assertFormat("Key2:\"1{2\"", "Key2", "1{2");
      assertFormat("Key2:\"1}2\"", "Key2", "1}2");
      assertFormat("Key2:\"1(2\"", "Key2", "1(2");
      assertFormat("Key2:\"1)2\"", "Key2", "1)2");
      assertFormat("Key2:\"1-2\"", "Key2", "1-2");
      assertFormat("Key2:\"1,2\"", "Key2", "1,2");
    }
    
    private void assertFormat(string expected, params object[] kvp)
    {
      LogFormatter.Format(kvp).Should().Be(expected);  
    }
  }
}