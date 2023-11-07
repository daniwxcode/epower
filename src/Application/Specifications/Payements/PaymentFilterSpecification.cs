using BlazorHero.CleanArchitecture.Application.Specifications.Base;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Specifications.Payements
{
    public class PaymentFilterSpecification : HeroSpecification<Payment>
    {
        public PaymentFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.BilledAmount.ToString().Contains(searchString) || p.Amount.ToString().Contains(searchString)) || (p.InternalReference.Contains(searchString));
            }
            else
            {
                Criteria = p => (p.IsConfirmed);
            }
        }
        public PaymentFilterSpecification(string searchString, string userId)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.BilledAmount.ToString().Contains(searchString) || p.Amount.ToString().Contains(searchString)) || (p.InternalReference.Contains(searchString)) && p.CreatedBy == userId;
            }
            else
            {
                Criteria = p => (p.IsConfirmed && p.CreatedBy==userId);
            }
        }
    }
}
