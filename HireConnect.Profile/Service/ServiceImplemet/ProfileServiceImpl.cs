
using HireConnect.Profile.DTO;
using HireConnect.Profile.Exceptions;
using HireConnect.Profile.Models;
using HireConnect.Profile.Repository.Interface;
using HireConnect.Profile.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HireConnect.Profile.Service.ServiceImplemet;

public class ProfileServiceImpl : IProfileService
{
    private readonly IProfileRepository _repository;
    private readonly IFileService _fileService;
    public ProfileServiceImpl(IProfileRepository repository, IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    public async Task AddCandidateProfileAsync(CandidateProfileDTO dto, int userId)
    {
        var existing = await _repository.FindCandidateByUserIdAsync(userId);
        if (existing != null) throw new ProfileAlreadyExistsException("Candidate");



        var uploadDocumentResult = await _fileService.UploadDocumentAsync(dto.Resume!, "HireConnect/Resumes");
        string resumeUrl = uploadDocumentResult.SecureUrl.ToString();

        var uploadImageResult = await _fileService.UploadImageAsync(dto.ProfileImage!, "HireConnect/ProfilePictures");
        string profilePictureUrl = uploadImageResult.SecureUrl.ToString();

        var newAddress = new Address
        {
            HouseNo = dto.Address.HouseNo,
            Street = dto.Address.Street,
            City = dto.Address.City,
            State = dto.Address.State,
            Pincode = dto.Address.Pincode
        };
        var profile = new CandidateProfile
        {
            UserId = userId,
            FullName = dto.FullName,
            Email = dto.Email,
            Mobile = dto.Mobile,
            DOB = dto.DOB,
            Skills = dto.Skills,
            Experience = dto.Experience,
            ResumeUrl = resumeUrl,
            ProfileImageUrl = profilePictureUrl,
            Address = newAddress
        };
        await _repository.AddCandidateAsync(profile);
        await _repository.SaveChangesAsync();
    }
    public async Task UpdateCandidateProfileAsync(
     CandidateProfileDTO dto,
     int userId)
    {
        var candidate =
            await _repository.FindCandidateByUserIdAsync(userId);

        if (candidate == null)
            throw new ProfileNotFoundException(userId);

        // =========================
        // Resume Update
        // =========================

        if (dto.Resume != null && dto.Resume.Length > 0)
        {
            // Delete old resume if exists
            if (!string.IsNullOrWhiteSpace(candidate.ResumeUrl))
            {
                await _fileService.DeleteFileAsync(
                    candidate.ResumeUrl
                );
            }

            // Upload new resume
            var uploadDocumentResult =
                await _fileService.UploadDocumentAsync(
                    dto.Resume,
                    "HireConnect/Resumes"
                );

            candidate.ResumeUrl =
                uploadDocumentResult.SecureUrl.ToString();
        }

        if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
        {
            // Delete old profile image if exists
            if (!string.IsNullOrWhiteSpace(candidate.ProfileImageUrl))
            {
                await _fileService.DeleteFileAsync(
                    candidate.ProfileImageUrl
                );
            }

            // Upload new profile image
            var uploadImageResult =
                await _fileService.UploadImageAsync(
                    dto.ProfileImage,
                    "HireConnect/ProfilePictures"
                );

            candidate.ProfileImageUrl =
                uploadImageResult.SecureUrl.ToString();
        }


        candidate.FullName = dto.FullName;

        candidate.Email = dto.Email;

        candidate.Mobile = dto.Mobile;
        candidate.DOB = dto.DOB;


        candidate.Skills = dto.Skills ?? "";

        candidate.Experience = dto.Experience;


        if (candidate.Address == null)
        {
            candidate.Address = new Address();
        }

        candidate.Address.HouseNo =
            dto.Address.HouseNo;

        candidate.Address.Street =
            dto.Address.Street;

        candidate.Address.City =
            dto.Address.City;

        candidate.Address.State =
            dto.Address.State;

        candidate.Address.Pincode =
            dto.Address.Pincode;

        await _repository.UpdateCandidateAsync(candidate);

        await _repository.SaveChangesAsync();
    }

    public async Task AddRecruiterProfileAsync(RecruiterProfileDTO dto, int userId)
    {
        var existing = await _repository.FindRecruiterByUserIdAsync(userId);
        if (existing != null) throw new ProfileAlreadyExistsException("Recruiter");

        var uploadImageResult = await _fileService.UploadImageAsync(dto.CompanyLogo!, "HireConnect/CompanyLogos");
        string companyLogoUrl = uploadImageResult.SecureUrl.ToString();

        var profile = new RecruiterProfile
        {
            UserId = userId,
            FullName = dto.FullName,
            Email = dto.Email,
            CompanyName = dto.CompanyName,
            CompanySize = dto.CompanySize,
            Industry = dto.Industry,
            Website = dto.Website,
            CompanyLogoUrl = companyLogoUrl,
            Address = new Address
            {
                HouseNo = dto.Address.HouseNo,
                Street = dto.Address.Street,
                City = dto.Address.City,
                State = dto.Address.State,
                Pincode = dto.Address.Pincode
            }
        };
        await _repository.AddRecruiterAsync(profile);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateRecruiterProfileAsync(RecruiterProfileDTO dto, int userId)
    {
        var recruiter = await _repository.FindRecruiterByUserIdAsync(userId);

        if (recruiter == null)
            throw new ProfileNotFoundException(userId);

        if (dto.CompanyLogo != null && dto.CompanyLogo.Length > 0)
        {
            // Delete old company logo if exists
            if (!string.IsNullOrWhiteSpace(recruiter.CompanyLogoUrl))
            {
                await _fileService.DeleteFileAsync(
                    recruiter.CompanyLogoUrl
                );
            }

            var uploadImageResult =
                await _fileService.UploadImageAsync(
                    dto.CompanyLogo,
                    "HireConnect/CompanyLogos"
                );

            recruiter.CompanyLogoUrl =
                uploadImageResult.SecureUrl.ToString();
        }

        recruiter.FullName = dto.FullName;
        recruiter.Email = dto.Email;
        recruiter.CompanyName = dto.CompanyName ?? "";
        recruiter.CompanySize = dto.CompanySize ?? "";
        recruiter.Industry = dto.Industry ?? "";
        recruiter.Website = dto.Website ?? "";

        if (recruiter.Address == null)
        {
            recruiter.Address = new Address();
        }
        recruiter.Address.HouseNo = dto.Address.HouseNo ?? "";
        recruiter.Address.Street = dto.Address.Street ?? "";
        recruiter.Address.City = dto.Address.City ?? "";
        recruiter.Address.State = dto.Address.State ?? "";
        recruiter.Address.Pincode = dto.Address.Pincode ?? "";

        await _repository.UpdateRecruiterAsync(recruiter);

        await _repository.SaveChangesAsync();
    }

    public async Task<UserProfileDTO?> GetProfileByUserIdAsync(int userId)
    {
        // First check Candidate table
        var candidate = await _repository.FindCandidateByUserIdAsync(userId);
        if (candidate != null) return MapToDto(candidate);

        // Then check Recruiter table
        var recruiter = await _repository.FindRecruiterByUserIdAsync(userId);
        if (recruiter != null) return MapToDto(recruiter);

        return null; // No profile found                
    }



    public async Task DeleteProfileAsync(int userId)
    {
        await _repository.DeleteCandidateByUserIdAsync(userId);
        await _repository.DeleteRecruiterByUserIdAsync(userId);
        await _repository.SaveChangesAsync();
    }

    private UserProfileDTO MapToDto(CandidateProfile c) => new UserProfileDTO
    {
        ProfileId = c.ProfileId,
        UserId = c.UserId,
        FullName = c.FullName,
        Email = c.Email,
        DOB = c.DOB,
        UserType = "Candidate",
        Mobile = c.Mobile,
        Skills = c.Skills,
        ResumeUrl = c.ResumeUrl,
        ProfileImageUrl = c.ProfileImageUrl,
        Experience = c.Experience,
        Address = new AddressDTO(c.Address!.HouseNo, c.Address.Street, c.Address.City, c.Address.State, c.Address.Pincode)
    };

    private UserProfileDTO MapToDto(RecruiterProfile r) => new UserProfileDTO
    {
        ProfileId = r.ProfileId,
        UserId = r.UserId,
        FullName = r.FullName,
        Email = r.Email,
        UserType = "Recruiter",
        CompanyName = r.CompanyName,
        CompanySize = r.CompanySize,
        CompanyLogoUrl = r.CompanyLogoUrl,
        Industry = r.Industry,
        Website = r.Website,
        Address = new AddressDTO(r.Address!.HouseNo, r.Address.Street, r.Address.City, r.Address.State, r.Address.Pincode)
    };

    // Implementation for other interface methods (GetByEmail, GetAll) following similar logic...
    public Task<UserProfileDTO?> GetByEmailAsync(string email) => throw new NotImplementedException();
    public Task<List<UserProfileDTO>> GetAllProfilesAsync() => throw new NotImplementedException();
}