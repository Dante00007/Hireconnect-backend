using HireConnect.Job.Models;

namespace HireConnect.Job.Repository.Interface;

public interface IJobRepository
    {
        // Identification and Retrieval
        Task<Jobs?> FindByIdAsync(int id); 
        Task<IEnumerable<Jobs>> GetAllAsync(); 

    
        Task<Jobs?> FindByTitleAsync(string title); 
        Task<IEnumerable<Jobs>> FindByTitleContainingAsync(string keyword);
        Task<IEnumerable<Jobs>> FindByCategoryAsync(string category); 
        Task<IEnumerable<Jobs>> FindByLocationAsync(string location); 
        Task<IEnumerable<Jobs>> FindByStatusAsync(string status); 
        Task<IEnumerable<Jobs>> FindByPostedByAsync(int recruiterId); 

        Task AddAsync(Jobs job); 
        Task UpdateAsync(Jobs job); 
        Task DeleteAsync(int id); 
        Task<bool> SaveChangesAsync();
    }
