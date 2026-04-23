using System.ComponentModel;
using System.Reflection;
using Csp.Slide.Tools;

namespace Csp.Slide.Tools.Tests;

public class TextConstantTests
{
    private static IEnumerable<FieldInfo> AllConstants =>
        typeof(TextConstant)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly);

    // ── English constants ─────────────────────────────────────────────────────

    [Fact]
    public void AllConstants_EnglishValues_AreNotNullOrWhiteSpace()
    {
        foreach (var field in AllConstants)
        {
            var value = (string?)field.GetValue(null);
            Assert.False(
                string.IsNullOrWhiteSpace(value),
                $"Constant '{field.Name}' must have a non-empty English value.");
        }
    }

    // ── Chinese descriptions (via [Description]) ──────────────────────────────

    [Fact]
    public void AllConstants_ChineseDescriptions_AreNotNullOrWhiteSpace()
    {
        foreach (var field in AllConstants)
        {
            var description = field.GetDescription();
            Assert.False(
                string.IsNullOrWhiteSpace(description),
                $"Constant '{field.Name}' must have a non-empty [Description] (Chinese) value.");
        }
    }

    // ── Specific known values ─────────────────────────────────────────────────

    [Fact]
    public void ConvertSlideBatchDescription_HasExpectedEnglishValue()
    {
        Assert.Equal("Batch convert slide format", TextConstant.ConvertSlideBatchDescription);
    }

    [Fact]
    public void ConvertSlideBatchDescription_HasExpectedChineseDescription()
    {
        var field = typeof(TextConstant).GetField(nameof(TextConstant.ConvertSlideBatchDescription))!;
        Assert.Equal("批量转换切片格式", field.GetDescription());
    }

    [Fact]
    public void ArePathsSameDescription_EnglishAndChinese_BothNonEmpty()
    {
        var field = typeof(TextConstant).GetField(nameof(TextConstant.ArePathsSameDescription))!;
        Assert.NotEmpty(TextConstant.ArePathsSameDescription);
        Assert.NotEmpty(field.GetDescription());
    }
}
