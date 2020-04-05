using System;
using System.ComponentModel.DataAnnotations;

using Beng.Specta.Compta.Core.Events.Funding;
using Beng.Specta.Compta.Core.ValueTypes.Funding;
using Beng.Specta.Compta.Core.ValueTypes.Ids;
using Beng.Specta.Compta.SharedKernel;

using Dawn;

using NodaMoney;

namespace Beng.Specta.Compta.Core.Entities.Funding
{
    /// <summary>
    /// Funding Steps (Echanciers)
    /// </summary>
    public class FundingItem : BaseEntity
    {
        public FundingItem(
            string title,
            Money amountHT,
            DateTime billingDate,
            DateTime paymentDate, 
            bool isComplete)
        {
            Guard.Argument(title, nameof(title)).NotNull();

            this.Title = title;
            this.AmountHT = amountHT;
            this.BillingDate = billingDate;
            this.PaymentDate = paymentDate;
            this.IsComplete = isComplete;
        }

        public FundingItem(
            string title,
            Money amountHT,
            DateTime billingDate,
            DateTime paymentDate,
            bool isComplete,
            FundingOrganization name) : this(title, amountHT, billingDate, paymentDate, isComplete)
        {
            this.Organization = name;
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
            Events.Add(new FundingItemCompletedEvent(this));
        }
    }
}