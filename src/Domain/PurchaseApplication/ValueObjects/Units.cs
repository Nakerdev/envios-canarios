using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Units : Record<Units>
    {
        private int value;

        public static Either<UnitsValidationError, Units> Create(Option<string> value)
        {
            return value
                .Map(v => new Units(int.Parse(v)))
                .ToEither(() => UnitsValidationError.Required);
        }

        private Units(int value)
        {
            this.value = value;
        }
    }

    public enum UnitsValidationError
    {
        Required
    }
}