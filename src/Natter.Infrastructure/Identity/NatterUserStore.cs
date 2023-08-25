namespace Natter.Infrastructure.Identity;

using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Natter.Domain.Entities;
using Natter.Shared.Architecture;

// TODO: think whether this class should commit the changes
// to DB.
public class NatterUserStore : IUserStore<NatterUser>, IUserPasswordStore<NatterUser>
{
    private readonly IConnectionFactory _connFactory;

    public NatterUserStore(IConnectionFactory connFactory) => _connFactory = connFactory;

    Task<string> IUserStore<NatterUser>.GetUserIdAsync(NatterUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.Id);

    Task<string?> IUserStore<NatterUser>.GetUserNameAsync(NatterUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.UserName);

    Task<string?> IUserStore<NatterUser>.GetNormalizedUserNameAsync(NatterUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.NormalizedUserName);

    async Task<IdentityResult> IUserStore<NatterUser>.CreateAsync(NatterUser user, CancellationToken cancellationToken)
    {
        var connection = _connFactory.GetConnection();

        await connection.ExecuteAsync(@"
            INSERT INTO natter_users(
                id,
                username,
                normalized_username,
                password_hash
            ) VALUES (@Id, @Username, @NormalizedUsername, @PasswordHash)
        ", new
        {
            user.Id,
            user.UserName,
            NormalizedUsername = user.NormalizedUserName,
            user.PasswordHash,
        });

        return IdentityResult.Success;
    }

    async Task<IdentityResult> IUserStore<NatterUser>.UpdateAsync(NatterUser user, CancellationToken cancellationToken)
    {
        var connection = _connFactory.GetConnection();

        await connection.ExecuteAsync(@"
            UPDATE natter_users
            SET 
                [id] = @Id,
                [username] = @UserName,
                [normalized_username] = @NormalizedUserName,
                [password_hash] = @PasswordHash,
            WHERE [id] = @id
        ", new
        {
            user.Id,
            user.UserName,
            user.NormalizedUserName,
            user.PasswordHash
        });

        return IdentityResult.Success;
    }

    async Task<IdentityResult> IUserStore<NatterUser>.DeleteAsync(NatterUser user, CancellationToken cancellationToken)
    {
        var connection = _connFactory.GetConnection();

        await connection.ExecuteAsync(
            "DELETE FROM natter_users WHERE id = @Id",
            new { user.Id }
        );

        return IdentityResult.Success;
    }


    async Task<NatterUser> IUserStore<NatterUser>.FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var connection = _connFactory.GetConnection();

        return await connection.QueryFirstOrDefaultAsync<NatterUser>(
            "SELECT * FROM natter_users WHERE id = @Id",
            new { Id = userId }
        );
    }

    async Task<NatterUser> IUserStore<NatterUser>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var connection = _connFactory.GetConnection();

        return await connection.QueryFirstOrDefaultAsync<NatterUser>(
            "SELECT * FROM natter_users WHERE normalized_username = @Name",
            new { Name = normalizedUserName }
        );
    }

    Task IUserStore<NatterUser>.SetNormalizedUserNameAsync(NatterUser user, string normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    Task IUserStore<NatterUser>.SetUserNameAsync(NatterUser user, string userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    Task IUserPasswordStore<NatterUser>.SetPasswordHashAsync(NatterUser user, string passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    Task<string?> IUserPasswordStore<NatterUser>.GetPasswordHashAsync(NatterUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.PasswordHash);
    Task<bool> IUserPasswordStore<NatterUser>.HasPasswordAsync(NatterUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.PasswordHash != null);

    void IDisposable.Dispose()
        => GC.SuppressFinalize(this);
}
