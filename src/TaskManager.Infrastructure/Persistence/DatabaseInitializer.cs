using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Tasks;
using TaskManager.Infrastructure.Identity;

namespace TaskManager.Infrastructure.Persistence;

/// <summary>Applies migrations and creates deterministic demonstration data.</summary>
public static class DatabaseInitializer
{
    /// <summary>Initializes the database for an application service provider.</summary>
    public static async Task InitializeAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();
        await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        const string demoEmail = "demo@taskmanager.local";
        var user = await userManager.FindByEmailAsync(demoEmail).ConfigureAwait(false);
        if (user is null)
        {
            user = new ApplicationUser
            {
                Id = Guid.Parse("9b9df3aa-ef15-4384-a66c-60488c752e0e"),
                Name = "Demo User",
                Email = demoEmail,
                UserName = demoEmail,
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, "Demo1234").ConfigureAwait(false);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ", result.Errors.Select(error => error.Description)));
            }
        }

        if (!await context.Tasks.AnyAsync(task => task.OwnerId == user.Id, cancellationToken)
            .ConfigureAwait(false))
        {
            context.Tasks.AddRange(
                TaskItem.Create(
                    user.Id,
                    "Review Clean Architecture",
                    "Prepare the architecture walkthrough for the interview.",
                    DateTimeOffset.UtcNow.AddDays(2)),
                TaskItem.Create(
                    user.Id,
                    "Rehearse product demo",
                    "Validate registration, login and the complete task CRUD.",
                    DateTimeOffset.UtcNow.AddDays(4)));
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
