using Application.Features.Transactions.Commands.CreateTransaction;
using Contracts.Features.Transactions;
using Mapster;

namespace Api.Mappings;

public class TransactionMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
        => config.NewConfig<CreateTransactionRequest, CreateTransactionCommand>()
            .Map(dest => dest, src => src);
}
