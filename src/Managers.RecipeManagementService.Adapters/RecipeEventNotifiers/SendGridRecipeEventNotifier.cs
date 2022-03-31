using StrongGrid;
using StrongGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.RecipeManagement.Adapters.RecipeEventNotifiers
{
    public class SendGridRecipeEventNotifier : IRecipeEventNotifier
    {
        public record SendGridConnection(string SendGridApiKey);
        public record TemplateConfig(string FromEmail, string? ToEmail, string SendGridTemplateId);

        private readonly SendGridConnection connection;
        private readonly TemplateConfig templateConfig;
        public SendGridRecipeEventNotifier(SendGridConnection connection, TemplateConfig templateConfig)
        {
            this.connection = connection;
            this.templateConfig = templateConfig;
        }

        public void Notify(RecipeEvent recipeEvent, RecipeId recipeId)
        {
            var client = new Client(connection.SendGridApiKey);
            var toAddress = SendgridEmailUtilities.ParseEmail(templateConfig.ToEmail);
            var fromAddress = SendgridEmailUtilities.ParseEmail(templateConfig.FromEmail);
            client.Mail.
                SendToSingleRecipientAsync(to: toAddress, from: fromAddress, dynamicTemplateId: templateConfig.SendGridTemplateId)
                .Wait();
        }
    }

    static class SendgridEmailUtilities
    {
        public static MailAddress ParseEmail(string email)
        {
            var parsedEmail = new System.Net.Mail.MailAddress(email);
            return new MailAddress(parsedEmail.Address, parsedEmail.DisplayName);
        }
    }
}
