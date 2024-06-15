﻿using Dan.Application.Contract;
using AliExpress.Application.IServices;

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Application.Services
{
    public class MotoService : IMotoService
    {
        private readonly IMotoRepository _brandRepository;
        private readonly IMapper _mapper;

        public MotoService(IMotoRepository brandRepository,IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        //public async Task AddBrand(Dto brandDto)
        //{
        //    var brand = _mapper.Map<BrandDto, Brand>(brandDto);
        //   await _brandRepository.AddAsync(brand);
        //}

        //public async Task DeleteBrand(int id)
        //{
            
        //    await _brandRepository.DeleteAsync(id);
        //}

        //public async Task<IEnumerable<BrandDto>> GetAllBrands()
        //{
        //    var brands =await _brandRepository.GetAllAsync();
        //    var mappedBrand = _mapper.Map<IEnumerable<Brand>, IEnumerable<BrandDto>>(brands);
        //    return mappedBrand;
        //}

        //public async Task<BrandDto> GetBrand(int id)
        //{
        //    var brand = await _brandRepository.GetAsync(id);
        //    var brandDto =_mapper.Map<Brand, BrandDto>(brand);
        //    return brandDto;
        //}

        //public async Task UpdateBrand(BrandDto brandDto)
        //{
        //    var brand = _mapper.Map<BrandDto, Brand>(brandDto);
        //    await _brandRepository.UpdateAsync(brand);
        //}
    }
}
