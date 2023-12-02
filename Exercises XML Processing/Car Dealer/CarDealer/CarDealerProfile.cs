using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Globalization;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDto, Supplier>();

            this.CreateMap<ImportPartsDto, Part>()
                .ForMember(d => d.SupplierId,
                opt => opt.MapFrom(s => s.SupplierId.Value));
            this.CreateMap<Part, ExportPartCarsDto>();


            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());
            this.CreateMap<Car, ExportCarDto>();
            this.CreateMap<Car, ExportBMWCarsDto>();
            this.CreateMap<Car, ExportCarWIthPartsDto>()
                .ForMember(d => d.Parts,
                    opt => opt.MapFrom(s =>
                        s.PartsCars
                            .Select(pc => pc.Part)
                            .OrderByDescending(p => p.Price)
                            .ToArray()));

            this.CreateMap<ImportCustomerDto, Customer>();

            this.CreateMap<ImportSaleDto, Sale>();
        }
    }
}
