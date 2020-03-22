using AutoMapper;
using PayX.Api.Controllers.Resources;
using PayX.Core.Models;
using PayX.Core.Extensions;

namespace PayX.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap<Currency, CurrencyResource>();
            CreateMap<Payment, PaymentResource>()
                .ForMember(pr => pr.Currency, opt => opt.MapFrom(p => p.Currency.Name))
                .ForMember(pr => pr.CardNumber, opt => opt.MapFrom(p => p.CardNumber.GetMaskedCardNumber()));

            // API Resource to Domain
            CreateMap<CurrencyResource, Currency>();
            CreateMap<PaymentResource, Payment>();
            CreateMap<ProcessPaymentResource, Payment>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.IsSuccessful, opt => opt.Ignore())
                .ForMember(p => p.CreatedAt, opt => opt.Ignore());
        }
    }
}