using Cardiac_Patient_Monitoring_System.DTO_S.EmergencyContactDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class EmergencyContactService
        : IEmergencyContactService
    {
        private readonly IEmergencyContactRepository _repository;

        public EmergencyContactService(
            IEmergencyContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<EmergencyContactResponseDto?>
            GetByIdAsync(int id)
        {
            var contact =
                await _repository.GetByIdAsync(id);

            if (contact == null)
                return null;

            return new EmergencyContactResponseDto
            {
                EmergencyContactId =
                    contact.EmergencyContactId,

                PatientId =
                    contact.PatientId,

                Name =
                    contact.Name,

                Phone =
                    contact.Phone,

                Relation =
                    contact.Relation,

                IsPrimary =
                    contact.IsPrimary,

                Email =
                    contact.Email,

                Notes =
                    contact.Notes,

                CreatedAt =
                    contact.CreatedAt,

                UpdatedAt =
                    contact.UpdatedAt
            };
        }

        public async Task<IEnumerable<EmergencyContactResponseDto>>
            GetByPatientIdAsync(int patientId)
        {
            var contacts =
                await _repository.GetByPatientIdAsync(
                    patientId);

            return contacts.Select(contact =>
                new EmergencyContactResponseDto
                {
                    EmergencyContactId =
                        contact.EmergencyContactId,

                    PatientId =
                        contact.PatientId,

                    Name =
                        contact.Name,

                    Phone =
                        contact.Phone,

                    Relation =
                        contact.Relation,

                    IsPrimary =
                        contact.IsPrimary,

                    Email =
                        contact.Email,

                    Notes =
                        contact.Notes,

                    CreatedAt =
                        contact.CreatedAt,

                    UpdatedAt =
                        contact.UpdatedAt
                });
        }

        public async Task<IEnumerable<EmergencyContactResponseDto>>
            GetAllAsync()
        {
            var contacts =
                await _repository.GetAllAsync();

            return contacts.Select(contact =>
                new EmergencyContactResponseDto
                {
                    EmergencyContactId =
                        contact.EmergencyContactId,

                    PatientId =
                        contact.PatientId,

                    Name =
                        contact.Name,

                    Phone =
                        contact.Phone,

                    Relation =
                        contact.Relation,

                    IsPrimary =
                        contact.IsPrimary,

                    Email =
                        contact.Email,

                    Notes =
                        contact.Notes,

                    CreatedAt =
                        contact.CreatedAt,

                    UpdatedAt =
                        contact.UpdatedAt
                });
        }

        public async Task<EmergencyContactResponseDto?>
            CreateAsync(
                int userId,
                CreateEmergencyContactDto dto)
        {
            var patientId =
                await _repository
                    .GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
                return null;

            var emergencyContact = new EmergencyContact
            {
                PatientId =
                    patientId.Value,

                Name =
                    dto.Name,

                Phone =
                    dto.Phone,

                Relation =
                    dto.Relation,

                IsPrimary =
                    dto.IsPrimary,

                Email =
                    dto.Email,

                Notes =
                    dto.Notes,

                CreatedAt =
                    DateTime.UtcNow,

                UpdatedAt =
                    DateTime.UtcNow
            };

            var createdContact =
                await _repository.AddAsync(
                    emergencyContact);

            return new EmergencyContactResponseDto
            {
                EmergencyContactId =
                    createdContact.EmergencyContactId,

                PatientId =
                    createdContact.PatientId,

                Name =
                    createdContact.Name,

                Phone =
                    createdContact.Phone,

                Relation =
                    createdContact.Relation,

                IsPrimary =
                    createdContact.IsPrimary,

                Email =
                    createdContact.Email,

                Notes =
                    createdContact.Notes,

                CreatedAt =
                    createdContact.CreatedAt,

                UpdatedAt =
                    createdContact.UpdatedAt
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateEmergencyContactDto dto)
        {
            var emergencyContact =
                await _repository.GetByIdAsync(id);

            if (emergencyContact == null)
                return false;

            if (dto.Name != null)
                emergencyContact.Name = dto.Name;

            if (dto.Phone != null)
                emergencyContact.Phone = dto.Phone;

            if (dto.Relation != null)
                emergencyContact.Relation =
                    dto.Relation;

            if (dto.IsPrimary.HasValue)
                emergencyContact.IsPrimary =
                    dto.IsPrimary.Value;

            if (dto.Email != null)
                emergencyContact.Email =
                    dto.Email;

            if (dto.Notes != null)
                emergencyContact.Notes =
                    dto.Notes;

            emergencyContact.UpdatedAt =
                DateTime.UtcNow;

            await _repository.UpdateAsync(
                emergencyContact);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var emergencyContact =
                await _repository.GetByIdAsync(id);

            if (emergencyContact == null)
                return false;

            await _repository.DeleteAsync(
                emergencyContact);

            return true;
        }

        public async Task<IEnumerable<EmergencyContactResponseDto>>
            GetMyEmergencyContactsAsync(int userId)
        {
            var contacts =
                await _repository.GetByUserIdAsync(
                    userId);

            return contacts.Select(contact =>
                new EmergencyContactResponseDto
                {
                    EmergencyContactId =
                        contact.EmergencyContactId,

                    PatientId =
                        contact.PatientId,

                    Name =
                        contact.Name,

                    Phone =
                        contact.Phone,

                    Relation =
                        contact.Relation,

                    IsPrimary =
                        contact.IsPrimary,

                    Email =
                        contact.Email,

                    Notes =
                        contact.Notes,

                    CreatedAt =
                        contact.CreatedAt,

                    UpdatedAt =
                        contact.UpdatedAt
                });
        }
    }
}