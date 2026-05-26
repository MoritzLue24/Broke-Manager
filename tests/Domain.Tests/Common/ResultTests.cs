using Domain.Common;

namespace Domain.Tests.Common;

public class ResultTests
{
    [Fact]
    public void Value_ShoudReturnValue_WhenResultSuccess()
    {
        // Setup
        var value = 2;
        Result<int> result = value;

        // Execute & assert
        Assert.Equal(2, result.Value);
    }

    [Fact]
    public void Value_ShouldThrowInvalidOperationException_WhenResultFailure()
    {
        // Setup
        Result<int> result = new InvalidGuidError();

        // Execute & assert
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Value; });
    }

    [Fact]
    public void Errors_ShouldReturnAllErrors_WhenResultMultipleErrors()
    {
        // Setup
        Result<int> result = new List<Error>([new InvalidGuidError(), new InvalidAmountError()]);

        // Execute & assert
        Assert.Equal([new InvalidGuidError(), new InvalidAmountError()], result.Errors);
    }

    [Fact]
    public void Errors_ShouldThrowInvalidOperationException_WhenResultSuccess()
    {
        // Setup
        Result<int> result = 1;

        // Execute & assert
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Errors; });
    }

    [Fact]
    public void FirstError_ShouldReturnFirstError_WhenMultipleErrors()
    {
        // Setup
        Error[] errors = [new InvalidGuidError(), new InvalidAmountError()];
        Result<int> result = errors;

        // Execute & Assert
        Assert.Equal(new InvalidGuidError(), result.FirstError);
    }

    [Fact]
    public void FirstError_ShouldThrowInvalidOperationException_WhenResultSuccess()
    {
        // Setup
        Result<int> result = 1;

        // Execute & assert
        Assert.Throws<InvalidOperationException>(() => { var _ = result.FirstError; });
    }

    [Fact]
    public void Cast_ShouldCastValue_WhenResultSuccessAndConverterGiven()
    {
        // Setup
        Result<int> resultA = 1;

        // Execute
        var resultB = resultA.Cast(v => v.ToString());

        // Assert
        Assert.Equal(typeof(string), resultB.Value.GetType());
        Assert.Equal("1", resultB.Value);
    }

    [Fact]
    public void Cast_ShouldDoNothing_WhenResultSuccessAndTypesEqual()
    {
        // Setup
        Result<int> resultA = 1;

        // Execute
        var resultB = resultA.Cast<int>();

        // Assert
        Assert.Equal(typeof(int), resultB.Value.GetType());
        Assert.Equal(1, resultB.Value);
    }

    [Fact]
    public void Cast_ShouldReturnErrors_WhenResultHasErrors()
    {
        // Setup
        Error[] errors = [new InvalidGuidError(), new InvalidAmountError()];
        Result<int> resultA = errors;

        // Execute
        var resultB = resultA.Cast<int>();

        // Assert
        Assert.Equal(errors, resultB.Errors);
    }

    [Fact]
    public void Cast_ShouldThrowInvalidOperationException_WhenConverterNotGiven()
    {
        // Setup
        Result<int> resultA = 1;

        // Execute & assert
        Assert.Throws<InvalidOperationException>(() => resultA.Cast<string>());
    }

    [Fact]
    public void Match_ShouldCallOnSuccess_WhenResultSuccess()
    {
        // Setup
        Result<int> result = 1;
        int a = 2;

        // Execute
        result.Match(
            value => a++,
            error => throw new InvalidOperationException()
        );

        // Assert
        Assert.Equal(1, result.Value);
        Assert.Equal(3, a);
    }

    [Fact]
    public void Match_ShouldCallOnFailure_WhenResultNotSuccess()
    {
        // Setup
        Result<int> result = new InvalidAmountError();
        int a = 2;

        // Execute
        result.Match(
            value => throw new InvalidOperationException(),
            error => a++
        );

        // Assert
        Assert.Equal(3, a);
    }
}
