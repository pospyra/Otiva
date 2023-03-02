﻿using Microsoft.AspNetCore.Mvc;
using Otiva.AppServeces.Service.Ad;
using Otiva.AppServeces.Service.SelectedAds;
using Otiva.Contracts.AdDto;
using Otiva.Contracts.SelectedAdDto;
using System.Net;

namespace Otiva.API.Controllers
{
    public class SelectedAdController : ControllerBase
    {
        public readonly ISelectedAdsService _selectedadService;

        public SelectedAdController(ISelectedAdsService selectedadService)
        {
            _selectedadService = selectedadService;
        }

        [HttpGet("/allSelectedByUserID{Id}")]
        [ProducesResponseType(typeof(IReadOnlyCollection<InfoSelectedResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(Guid Id)
        {
            var result = await _selectedadService.GetSelectedUsers(Id);

            return Ok(result);
        }


        [HttpPost("selectedAd/add")]
        [ProducesResponseType(typeof(IReadOnlyCollection<InfoSelectedResponse>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateAdAsync(Guid UserId, Guid AdId)
        {
            var result = await _selectedadService.AddSelectedAsync(UserId, AdId);

            return Created("", result);
        }


        [HttpDelete("/selected/delete")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAdAsync(Guid UserId, Guid AdId)
        {
            await _selectedadService.DeleteAsync(UserId, AdId);

            return NoContent();
        }
    }
}