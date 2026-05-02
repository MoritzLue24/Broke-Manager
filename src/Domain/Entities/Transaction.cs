
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
                RecurringDetail? 
                recurringDetail, 
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
                RecurringDetail? 
                recurringDetail, 
                string counterParty)
        {
                
        }





}