using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Bail;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Habitat.Buildings.Commands
{
    public class AddEditShopTenantCommand: IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
    }
    internal class AddEditShopTenanCommandtHandler: IRequestHandler<AddEditShopTenantCommand, Result<int>>
    {
        private IUnitOfWork<int> _unitOfWork;
        public AddEditShopTenanCommandtHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(AddEditShopTenantCommand request, CancellationToken cancellationToken)
        {
            var db = _unitOfWork.Repository<ShopTenant>();
            var isDubplicated = db.Entities.Include(_=>_.RentalAgreements).Any(_=>_.PhoneNumber == request.PhoneNumber && _.Id!= request.Id && _.RentalAgreements.Any(ra=>ra.EndDate!=null));
            if(isDubplicated)
            {
                return await Result<int>.FailAsync("Un Client Actif ayant le même numéro existe déjà");
            }
            if(request.Id==0)
            {
                var data = new ShopTenant()
                {
                    Id = request.Id,
                    FirstName = request.FirstName,
                    LastName = request.Name,
                    PhoneNumber = request.PhoneNumber,
                };
                await db.AddAsync(data);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(data.Id, "Locataire enregistré avec succès!");
            }
            var dbitem = await db.GetByIdAsync(request.Id);
            if (dbitem is null)
                return await Result<int>.FailAsync("Locataire Inexistant");
            dbitem.FirstName = request.FirstName;
            dbitem.LastName = request.Name;
            dbitem.PhoneNumber = request.PhoneNumber;
            await db.UpdateAsync(dbitem);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(dbitem.Id, $"Locataire {request.Name} {request.FirstName} modifié avec succès!");
        }
    }
}
