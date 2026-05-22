using Infrastructure.Security;

namespace Infrastructure.Tests.Security;

public class HasherTests
{
    private readonly Hasher _hasher = new();

    [Fact]
    public void Hash_ReturnsDifferentString()
    {
        // Setup
        var password = "password123!";

        // Execute
        var hash = this._hasher.Hash(password);

        // Assert
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void Hash_ReturnsDifferentHashes_WhenSamePlain()
    {
        // Execute
        // BCrypt generiert jedes Mal einen neuen Salt
        var hash1 = this._hasher.Hash("password123");
        var hash2 = this._hasher.Hash("password123");

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Verify_ReturnsTrue_WhenPasswordCorrect()
    {
        var password = "password123!";
        var hash = this._hasher.Hash(password);
        Assert.True(this._hasher.Verify(password, hash));
    }

    [Fact]
    public void Verify_ReturnsFalse_WhenPasswordIncorrect()
    {
        var password = "password123!";
        var hash = this._hasher.Hash(password);
        Assert.False(this._hasher.Verify("wrongpassword", hash));
    }
}
