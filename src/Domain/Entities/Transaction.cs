
using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Transaction
{
    
        public Guid Id { get; private set;}
        public Guid UserId { get; private set;}
        public Guid CategoryId { get; private set; }
        public CategorySource CategorySource {get; private set;}
        public decimal Amount {get; private set;}
        public DateOnly Date {get; private set;}
        public string Title {get; private set;}
        public string CounterParty {get; private set;}

        public RecurringDetail? RecurringDetail {get; private set;}


        private Transaction(
                Guid userId, 
                Guid categoryId, 
                CategorySource categorySource,
                decimal amount, 
                DateOnly date, 
                string title, 
                RecurringDetail? recurringDetail, 
                string counterParty)
        {
                Id = Guid.NewGuid();
                UserId = userId;
                CategoryId = categoryId;
                CategorySource = categorySource;
                Amount = amount;
                Date = date;
                Title = title;
                CounterParty = counterParty;
                RecurringDetail = recurringDetail;
                
        }

        public static DomainResult<Transaction> Create(
                Guid userId, 
                Guid categoryId, 
                CategorySource categorySource,
                decimal amount, 
                DateOnly date, 
                string title, 
                RecurringDetail? recurringDetail, 
                string counterParty)
        {
                if(userId == Guid.Empty || categoryId == Guid.Empty)
                {
                        return DomainResult<Transaction>.Fail(DomainErrorCode.InvalidId);
                }
                
                if(amount == 0)
                {
                        return DomainResult<Transaction>.Fail(DomainErrorCode.WrongAmount );
                }

                if (string.IsNullOrWhiteSpace(title))
                {
                        return DomainResult<Transaction>.Fail(DomainErrorCode.TransactionTitleEmpty);
                }
                 
                if (string.IsNullOrWhiteSpace(counterParty))
                {
                        return DomainResult<Transaction>.Fail(DomainErrorCode.TransactionCounterPartyEmpty);
                }

                if (!Enum.IsDefined(typeof(CategorySource), categorySource))
                {
                        return DomainResult<Transaction>.Fail(DomainErrorCode.InvalidCategorySource);
                }

                if(recurringDetail !=null)
                {
                        if(date > recurringDetail.End)
                        {
                                return DomainResult<Transaction>.Fail(DomainErrorCode.InvalidTransactionDate);
                        }
                }

                return DomainResult<Transaction>.Ok(new Transaction(userId, categoryId, categorySource, amount, date, title, recurringDetail, counterParty));
        }

        public DomainResult<Unit> ChangeTitle(string title)
        {
                if (string.IsNullOrWhiteSpace(title))
                {
                        return DomainResult<Unit>.Fail(DomainErrorCode.TransactionTitleEmpty);
                }

                return DomainResult<Unit>.Ok();
        }

        public DomainResult<Unit> ChangeAmount(decimal amount)
        {
                if(amount == 0)
                {
                        return DomainResult<Unit>.Fail(DomainErrorCode.WrongAmount );
                }
                
                return DomainResult<Unit>.Ok();
        }





}