using BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.DTO;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Models;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Queries
{
    public class GetPayementReceipt: IRequest<Result<BuyCreditResponse>>
    {
        public int PayementID { get; set; }
    }
    internal class GetPayementReceiptHandler: IRequestHandler<GetPayementReceipt, Result<BuyCreditResponse>>
    {
        private IUnitOfWork<int> _unitOfWork;
        private IUserService _userService;
        public GetPayementReceiptHandler(IUnitOfWork<int> unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        public async Task<Result<BuyCreditResponse>> Handle(GetPayementReceipt request, CancellationToken cancellationToken)
        {
            if (request.PayementID == 0)
                return await Result<BuyCreditResponse>.FailAsync("Paiement Inexistant");
            var payement =await _unitOfWork.Repository<Payment>().GetByIdAsync(request.PayementID);
            var user = await _userService.GetAsync(payement.CreatedBy);
            var userName = $"{user.Data.LastName.ToUpper()} {user.Data.FirstName}";
            return Result<BuyCreditResponse>.Success(new BuyCreditResponse(payement.Id,(int) payement.Amount, payement.BilledAmount, payement.SerialNumber, payement.ExternalReference, payement.CreatedOn, payement.InternalReference, payement.CreditCode, payement.Credits, userName));

        }
    }
}
