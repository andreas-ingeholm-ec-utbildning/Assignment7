using Crito.Contexts;
using Crito.Models;
using Crito.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Persistence.EFCore.Scoping;
using Umbraco.Cms.Web.Website.Controllers;

namespace Crito.Controllers;

public class ContactController : SurfaceController
{

    readonly IEFCoreScopeProvider<ContactContext> scopeProvider;

    public ContactController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, IEFCoreScopeProvider<ContactContext> scopeProvider) :
        base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        this.scopeProvider = scopeProvider;
    }

    [HttpPost]
    public IActionResult Index(ContactForm form)
    {

        if (!ModelState.IsValid)
            return CurrentUmbracoPage();

        using var mail = new MailService("no-reply@crito.com", "smtp.crito.com", 465, "contact@crito.com", "test");
        mail.SendAsync(form.Email, "Your message was received.", "Hi, your message was received and we will respond as soon as we can.").ConfigureAwait(false);
        mail.SendAsync("contact@crito.com", $"{form.Name} sent a message.", form.Message).ConfigureAwait(false);

        TempData["form-success-message"] = "Thank you for your message, we will get back to you as soon as possible.";

        return RedirectToCurrentUmbracoPage();

    }

    [HttpPost]
    public async Task<IActionResult> RegisterEmail(EmailListForm form)
    {

        if (!ModelState.IsValid)
            return CurrentUmbracoPage();

        using var scope = scopeProvider.CreateScope();

        await scope.ExecuteWithContextAsync<Task>(async db =>
        {

            if (!db.EmailAddresses.Any(e => e.Email == form.Email))
            {
                db.EmailAddresses.Add(form);
                await db.SaveChangesAsync();
                TempData["form-success-message"] = "The email was successfully registered.";
            }
            else
                TempData["form-success-message"] = "The email was already registered.";

        });

        scope.Complete();
        return RedirectToCurrentUmbracoPage();

    }

}
