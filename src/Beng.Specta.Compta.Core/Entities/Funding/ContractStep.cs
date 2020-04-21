using System;
using System.ComponentModel.DataAnnotations;

using Beng.Specta.Compta.Core.Events.Funding;
using Beng.Specta.Compta.Core.ValueTypes.Funding;
using Beng.Specta.Compta.SharedKernel;

using Dawn;

using NodaMoney;

namespace Beng.Specta.Compta.Core.Entities.Funding
{
    /// <summary>
    /// Contract Steps (Echanciers)
    /// </summary>
    public class ContractStep : BaseEntity
    {
        public ContractStep(
            string title,
            Money amountHT,
            DateTime billingDate,
            DateTime paymentDate, 
            bool isComplete)
        {
            Guard.Argument(title, nameof(title)).NotNull();

            Title = title;
            AmountHT = amountHT;
            BillingDate = billingDate;
            PaymentDate = paymentDate;
            IsComplete = isComplete;
        }

        public ContractStep(
            string title,
            Money amountHT,
            DateTime billingDate,
            DateTime paymentDate,
            bool isComplete,
            FundingOrganization name) : this(title, amountHT, billingDate, paymentDate, isComplete)
        {
            Organization = name;
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public Money AmountHT { get; set; }

        [Required]
        public decimal Amount
        {
            get => AmountHT.Amount;
            set
            {
                AmountHT = Money.Euro(value);
            }
        }

        [Required]
        public DateTime BillingDate { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public bool IsComplete { get; set; }

        // Foreign key (ManyToOne) with FundingGroup
        [Required]
        public FundingOrganization Organization { get; set; }

        public void MarkComplete()
        {
            IsComplete = true;
            Events.Add(new ContractStepCompletedEvent(this));
        }
    }
}