﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Otiva.AppServeces.IRepository;
using Otiva.Contracts.AdDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otiva.AppServeces.Service.Ad
{
    public class AdService : IAdService
    {
        public readonly IAdRepository _adRepository;
        public readonly IMapper _mapper;
        public AdService(IAdRepository adRepository, IMapper mapper)
        {
            _adRepository = adRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAdAsync(CreateOrUpdateAdRequest createAd)
        {
            var newUser = _mapper.Map<Domain.Ad>(createAd);
            newUser.CreateTime = DateTime.UtcNow;
            await _adRepository.AddAsync(newUser);

            return newUser.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingUser = await _adRepository.FindById(id);
            await _adRepository.DeleteAsync(existingUser);
        }

        public async Task<InfoAdResponse> EditAdAsync(Guid Id, CreateOrUpdateAdRequest editAd)
        {
            var existingAd = await _adRepository.FindById(Id);
            await _adRepository.EditAdAsync(_mapper.Map(editAd, existingAd));

            return _mapper.Map<InfoAdResponse>(editAd);

        }

        public async Task<IReadOnlyCollection<InfoAdResponse>> GetAll(int take, int skip)
        {
            return await _adRepository.GetAll()
                 .Select(a => new InfoAdResponse
                 {
                     Id = a.Id,
                     Name = a.Name,
                     Description = a.Description,
                     SubcategoryId = a.SubcategoryId,
                     CreateTime = a.CreateTime,
                 }).OrderBy(d=>d.CreateTime).Skip(skip).Take(take).ToListAsync();
        }

        public async  Task<InfoAdResponse> GetByIdAsync(Guid id)
        {
            var exitAd = await _adRepository.FindById(id);
            return _mapper.Map<InfoAdResponse>(exitAd);
        }
    }
}