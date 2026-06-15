using Application.Features.AutoAssign.Commands.AutoAssign;
using Application.Features.AutoAssign.Contracts;
using Application.Features.Transactions.Commands.CreateTransaction;
using Application.Features.Transactions.Commands.UpdateTransaction;
using Application.Features.Transactions.Contracts;
using Contracts.Features.AutoAssign.Requests;
using Contracts.Features.AutoAssign.Responses;
using Contracts.Features.Transactions.Requests;
using Contracts.Features.Transactions.Responses;
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

        config.NewConfig<AutoAssignResult, AutoAssignResponse>()
            .Map(dest => dest.Transaction, src => src.TransactionResult)
            .Map(
                dest => dest.ConflictingCategories,
                src => src.ConflictingCategories == null
                    ? null
                    : src.ConflictingCategories.Select(x => new CategoryConflictResponse(x.CategoryId, x.Score))
            );

        config.NewConfig<AutoAssignRequest, AutoAssignCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<(Guid id, UpdateTransactionRequest request), UpdateTransactionCommand>()
            .Map(dest => dest.TransactionId, src => src.id)
            .Map(dest => dest, src => src.request);
    }
}
