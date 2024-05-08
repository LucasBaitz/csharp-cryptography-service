using Cryptography.Domain.Entities;
using Cryptography.Domain.Interfaces;
using Cryptography.Services.DTOs;
using Cryptography.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Cryptography.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CryptographyController : ControllerBase
    {
        private readonly IRepository<CryptData> _repository;
        private readonly ICryptographyService _cryptographyService;

        public CryptographyController(ICryptographyService cryptographyService, IRepository<CryptData> repository)
        {
            _cryptographyService = cryptographyService;
            _repository = repository;
        }

        [HttpGet]
        [Route("All/{encrypted?}")]
        public async Task<IActionResult> GetAll(bool encrypted = false)
        {
            var entities = await _repository.GetAllAsync();

            if (encrypted) return Ok(entities);

            var entitiesDtos = entities.Select(e =>
            {
                return new CryptDataDTO()
                {
                    Id = e.Id, 
                    CreditCardToken = _cryptographyService.Decrypt(e.CreditCardToken),
                    UserDocument = _cryptographyService.Decrypt(e.UserDocument),
                    Value = e.Value
                };
            });

            return Ok(entitiesDtos);
        }

        [HttpGet]
        [Route("Get/{id}/{encrypted?}")]
        public async Task<IActionResult> GetById(long id, bool encrypted = false)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound(); 
            }
            
            if (encrypted) return Ok(entity);

            var entityDto = new CryptDataDTO()
            {
                CreditCardToken = _cryptographyService.Decrypt(entity.CreditCardToken),
                UserDocument = _cryptographyService.Decrypt(entity.UserDocument),
                Value = entity.Value
            };

            return Ok(entityDto);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Create(CryptDataDTO data)
        {
            var cryptData = new CryptData()
            {
                UserDocument = _cryptographyService.Encrypt(data.UserDocument),
                CreditCardToken = _cryptographyService.Encrypt(data.CreditCardToken),
                Value = data.Value,
            };

           var createdEntity = await _repository.AddAsync(cryptData);

            return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id, crypted = true}, createdEntity);
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(long id, CryptDataDTO data)
        {
            var entity = await _repository.GetByIdAsync(id);
            
            if (entity is null)
            {
                return NotFound();
            }

            entity.CreditCardToken = _cryptographyService.Encrypt(data.CreditCardToken);
            entity.UserDocument = _cryptographyService.Encrypt(data.UserDocument);
            entity.Value = data.Value;
            
            await _repository.UpdateAsync(entity);

            return NoContent();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteById(long id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity is null) return NotFound();

            await _repository.DeleteAsync(entity);

            return NoContent();
        }

    }
}
