using System;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana
{
    public class HanaRequestException : Exception
    {
        public string Errors { get; }

        public HanaRequestException(string errors) : base(errors)
        {
            Errors = errors;
        }
    }
}
