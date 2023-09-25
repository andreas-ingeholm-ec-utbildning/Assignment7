using Crito.Models;
using Microsoft.EntityFrameworkCore;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Crito.Contexts;

public class ContactContext : DbContext
{

    public ContactContext(DbContextOptions options) : base(options)
    { }

    public required DbSet<EmailListForm> EmailAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailListForm>(entity =>
        {
            entity.ToTable("EmailList");
            entity.HasKey(e => e.Email);
            entity.Property(e => e.Email).HasColumnName("email");
        });
    }

}

public class RunContactMigration : INotificationAsyncHandler<UmbracoApplicationStartedNotification>
{

    readonly ContactContext context;

    public RunContactMigration(ContactContext context) =>
        this.context = context;

    public async Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken)
    {

        var pending = await context.Database.GetPendingMigrationsAsync(cancellationToken);
        if (pending.Any())
            await context.Database.MigrateAsync(cancellationToken);

    }

}

public class ContactComposer : IComposer
{

    public void Compose(IUmbracoBuilder builder) =>
        builder.AddNotificationAsyncHandler<UmbracoApplicationStartedNotification, RunContactMigration>();

}
