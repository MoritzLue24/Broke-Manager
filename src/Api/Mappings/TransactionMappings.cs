using Mapster;
using Application.Features.Transactions.Commands.CreateTransaction;
using Contracts.Features.Transactions;

namespace Api.Mappings;

public class TransactionMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
        => config.NewConfig<(CreateTransactionRequest Request, Guid UserId), CreateTransactionCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);
}