﻿using System;
using System.Globalization;
using MessageTemplates;
using Xunit;

public class FormatTests
{
    class Chair
    {
        // ReSharper disable UnusedMember.Local
        public string Back
        {
            get { return "straight"; }
        }

        public int[] Legs
        {
            get { return new[] {1, 2, 3, 4}; }
        }
        // ReSharper restore UnusedMember.Local

        public override string ToString()
        {
            return "a chair";
        }
    }

    class Receipt
    {
        // ReSharper disable UnusedMember.Local
        public decimal Sum
        {
            get { return 12.345m; }
        }

        public DateTime When
        {
            get { return new DateTime(2013, 5, 20, 16, 39, 0); }
        }
        // ReSharper restore UnusedMember.Local

        public override string ToString()
        {
            return "a receipt";
        }
    }

    [Fact]
    public void AnObjectIsRenderedInSimpleNotation()
    {
        var m = Render("I sat at {@Chair}", new Chair());
        Assert.Equal("I sat at Chair { Back: \"straight\", Legs: [1, 2, 3, 4] }", m);
    }

    [Fact]
    public void AnObjectIsRenderedInSimpleNotationUsingFormatProvider()
    {
        var m = Render(new CultureInfo("fr-FR"), "I received {@Receipt}", new Receipt());
        Assert.Equal("I received Receipt { Sum: 12,345, When: 20/05/2013 16:39:00 }", m);
    }

    [Fact]
    public void AnAnonymousObjectIsRenderedInSimpleNotationWithoutType()
    {
        var m = Render("I sat at {@Chair}", new {Back = "straight", Legs = new[] {1, 2, 3, 4}});
        Assert.Equal("I sat at { Back: \"straight\", Legs: [1, 2, 3, 4] }", m);
    }

    [Fact]
    public void AnAnonymousObjectIsRenderedInSimpleNotationWithoutTypeUsingFormatProvider()
    {
        var m = Render(new CultureInfo("fr-FR"), "I received {@Receipt}", new {Sum = 12.345, When = new DateTime(2013, 5, 20, 16, 39, 0)});
        Assert.Equal("I received { Sum: 12,345, When: 20/05/2013 16:39:00 }", m);
    }

    [Fact]
    public void AnObjectWithDefaultDestructuringIsRenderedAsAStringLiteral()
    {
        var m = Render("I sat at {Chair}", new Chair());
        Assert.Equal("I sat at \"a chair\"", m);
    }

    [Fact]
    public void AnObjectWithStringifyDestructuringIsRenderedAsAString()
    {
        var m = Render("I sat at {$Chair}", new Chair());
        Assert.Equal("I sat at \"a chair\"", m);
    }

    [Fact]
    public void MultiplePropertiesAreRenderedInOrder()
    {
        var m = Render("Just biting {Fruit} number {Count}", "Apple", 12);
        Assert.Equal("Just biting \"Apple\" number 12", m);
    }

    [Fact]
    public void MultiplePropertiesUseFormatProvider()
    {
        var m = Render(new CultureInfo("fr-FR"), "Income was {Income} at {Date:d}", 1234.567, new DateTime(2013, 5, 20));
        Assert.Equal("Income was 1234,567 at 20/05/2013", m);
    }

    [Fact]
    public void FormatStringsArePropagated()
    {
        var m = Render("Welcome, customer {CustomerId:0000}", 12);
        Assert.Equal("Welcome, customer 0012", m);
    }

    [Theory]
    [InlineData("Welcome, customer #{CustomerId,-10}, pleasure to see you", "Welcome, customer #1234      , pleasure to see you")]
    [InlineData("Welcome, customer #{CustomerId,-10:000000}, pleasure to see you", "Welcome, customer #001234    , pleasure to see you")]
    [InlineData("Welcome, customer #{CustomerId,10}, pleasure to see you", "Welcome, customer #      1234, pleasure to see you")]
    [InlineData("Welcome, customer #{CustomerId,10:000000}, pleasure to see you", "Welcome, customer #    001234, pleasure to see you")]
    [InlineData("Welcome, customer #{CustomerId,10:0,0}, pleasure to see you", "Welcome, customer #     1,234, pleasure to see you")]
    [InlineData("Welcome, customer #{CustomerId:0,0}, pleasure to see you", "Welcome, customer #1,234, pleasure to see you")]
    public void AlignmentStringsArePropagated(string value, string expected)
    {
        Assert.Equal(expected, Render(value, 1234));
    }

    [Fact]
    public void FormatProviderIsUsed()
    {
        var m = Render(new CultureInfo("fr-FR"), "Please pay {Sum}", 12.345);
        Assert.Equal("Please pay 12,345", m);
    }

    static string Render(string messageTemplate, params object[] properties)
    {
        return Render(null, messageTemplate, properties);
    }

    static string Render(IFormatProvider formatProvider, string messageTemplate, params object[] properties)
    {
        return MessageTemplate.Format(formatProvider, messageTemplate, properties);
    }

    [Fact]
    public void ATemplateWithOnlyPositionalPropertiesIsAnalyzedAndRenderedPositionally()
    {
        var m = Render("{1}, {0}", "world", "Hello");
        Assert.Equal("\"Hello\", \"world\"", m);
    }

    [Fact]
    public void ATemplateWithOnlyPositionalPropertiesUsesFormatProvider()
    {
        var m = Render(new CultureInfo("fr-FR"), "{1}, {0}", 12.345, "Hello");
        Assert.Equal("\"Hello\", 12,345", m);
    }

    // Debatable what the behavior should be, here.
    [Fact]
    public void ATemplateWithNamesAndPositionalsUsesNamesForAllValues()
    {
        var m = Render("{1}, {Place}", "world", "Hello");
        Assert.Equal("\"world\", \"Hello\"", m);
    }

    [Fact]
    public void MissingPositionalParametersRenderAsTextLikeStandardFormats()
    {
        var m = Render("{1}, {0}", "world");
        Assert.Equal("{1}, \"world\"", m);
    }

    enum Size
    {
        Large
    }

    class SizeFormatter : IFormatProvider, ICustomFormatter
    {
        private readonly IFormatProvider _innerFormatProvider;

        public SizeFormatter(IFormatProvider innerFormatProvider)
        {
            _innerFormatProvider = innerFormatProvider;
        }

        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : _innerFormatProvider.GetFormat(formatType);
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is Size size)
            {
                return size == Size.Large ? "Huge" : size.ToString();
            }

            if (arg is IFormattable formattable)
            {
                return formattable.ToString(format, _innerFormatProvider);
            }

            return arg.ToString();
        }
    }

    [Fact]
    public void AppliesCustomFormatterToEnums()
    {
        var rendered = Render(new SizeFormatter(CultureInfo.InvariantCulture), "Size {size}", Size.Large);
        Assert.Equal("Size Huge", rendered);
    }
}