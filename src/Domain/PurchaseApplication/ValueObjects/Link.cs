using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Link : Record<Link>
    {
        private string value;
        
        public static Validation<ValidationError<LinkValidationError>, Link> Create(
            Option<string> value)
        {
            return value
                .Map(v => new Link(v))
                .ToValidation(new ValidationError<LinkValidationError>(
                    fieldId: nameof(Link),
                    errorCode: LinkValidationError.Required));
        }

        public Link(string value)
        {
            this.value = value;
        }
    }

    public enum LinkValidationError
    {
        Required
    }
}