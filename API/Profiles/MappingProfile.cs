using AutoMapper;
using BankTransferApi.Models;

namespace BankTransferApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.FromTransactions, opt => opt.MapFrom(src => src.FromTransactions))
                .ForMember(dest => dest.ToTransactions, opt => opt.MapFrom(src => src.ToTransactions));

            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.FromAccountNumber, opt => opt.MapFrom(src => src.FromAccount.AccountNumber))
                .ForMember(dest => dest.ToAccountNumber, opt => opt.MapFrom(src => src.ToAccount.AccountNumber));
        }
    }
}
