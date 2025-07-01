using Microsoft.AspNetCore.Mvc;
using ApiDocBot.Data;
using ApiDocBot.DTOs.UserDTOs;
using ApiDocBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using ApiDocBot.DTOs.UserRoleDTOs;
using ApiDocBot.DTOs.PsychologistProfileDTOs;

namespace ApiDocBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PsychologistProfileController:ControllerBase
    {
        private readonly AppDbContext _context;

        public PsychologistProfileController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/psychologistprofile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PsychologistProfileReadDTOs>>> GetPsychologistProfile()
        {
            var psychologistProfiles = await _context.psychologist_profile.ToListAsync();

            var psychologistProfileDTOs = psychologistProfiles.Select(f => new PsychologistProfileReadDTOs
            {
                PsychologistProfileId = f.psychologistProfile_id,
                PsychologistProfileLicenseNumber = f.psychologistProfile_licenseNumber,
                PsychologistProfileSpeciality = f.psychologistProfile_speciality,
                PsychologistProfileYearsEsperience = f.psychologistProfile_yearsEsperience,
                PsychologistProfileWorkInstitution = f.psychologistProfile_workInstitution,
                PsychologistProfileContactPhone = f.psychologistProfile_contactPhone,
                PsychologistProfileBiography = f.psychologistProfile_biography
            });
            return Ok(psychologistProfileDTOs);
        }

        //POST: api/psychologistprofile
        [HttpPost]
        public async Task<ActionResult<PsychologistProfileCreateDTOs>> CreatePsychologistProfile(PsychologistProfileCreateDTOs psychologistProfileDTOs)
        {
            var psychologistProfiles = new PsychologistProfileModel
            {
                psychologistProfile_id = psychologistProfileDTOs.PsychologistProfileId,
                psychologistProfile_licenseNumber = psychologistProfileDTOs.PsychologistProfileLicenseNumber,
                psychologistProfile_speciality = psychologistProfileDTOs.PsychologistProfileSpeciality,
                psychologistProfile_yearsEsperience = psychologistProfileDTOs.PsychologistProfileYearsEsperience,
                psychologistProfile_workInstitution = psychologistProfileDTOs.PsychologistProfileWorkInstitution,
                psychologistProfile_contactPhone = psychologistProfileDTOs.PsychologistProfileContactPhone,
                psychologistProfile_biography = psychologistProfileDTOs.PsychologistProfileBiography
            };

            _context.psychologist_profile.Add(psychologistProfiles);
            await _context.SaveChangesAsync();

            var result = new PsychologistProfileReadDTOs
            {
                PsychologistProfileId = psychologistProfiles.psychologistProfile_id,
                PsychologistProfileLicenseNumber = psychologistProfiles.psychologistProfile_licenseNumber,
                PsychologistProfileSpeciality = psychologistProfiles.psychologistProfile_speciality,
                PsychologistProfileYearsEsperience = psychologistProfiles.psychologistProfile_yearsEsperience,
                PsychologistProfileWorkInstitution = psychologistProfiles.psychologistProfile_workInstitution,
                PsychologistProfileContactPhone = psychologistProfiles.psychologistProfile_contactPhone,
                PsychologistProfileBiography = psychologistProfiles.psychologistProfile_biography
            };

            return CreatedAtAction(nameof(GetPsychologistProfile), new {id=psychologistProfiles.psychologistProfile_id}, result);
        }
    }
}
