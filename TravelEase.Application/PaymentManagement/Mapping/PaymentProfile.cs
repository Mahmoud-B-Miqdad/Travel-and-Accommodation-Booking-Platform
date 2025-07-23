using AutoMapper;
using TravelEase.Application.PaymentManagement.Commands;
using TravelEase.Application.PaymentManagement.Requests;

namespace TravelEase.Application.PaymentManagement.Mapping
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<CreatePaymentIntentRequest, CreatePaymentIntentCommand>();
        }
    }
}