using Application.Features.Transactions.Commands.CreateTransaction;
using Application.Features.Transactions.Commands.UpdateTransaction;
using Application.Features.Transactions.Common;
using Contracts.Features.Transactions;
using Mapster;

namespace Api.Mappings;

public class TransactionMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TransactionResult, TransactionResponse>()
            .Map(dest => dest, src => src);

        config.NewConfig<CreateTransactionRequest, CreateTransactionCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<(Guid id, UpdateTransactionRequest request), UpdateTransactionCommand>()
            .Map(dest => dest.TransactionId, src => src.id)
            .Map(dest => dest, src => src.request);
    }
}
