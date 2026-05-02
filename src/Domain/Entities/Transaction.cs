using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Transcation
{
    
        public Guid Id { get; private set;}
        public Guid UserId { get; private set;}
        public Guid CategoryId { get; private set; }
        public CategorySource CategorySource {get; private set;}
        public decimal Amount {get; private set;}
        public DateOnly Date {get; private set;}
        public string Title {get; private set;}
        public string CounterParty {get; private set;}

        public RecurringDetail recurringDetail {get; private set;}



}