//using System;
//using System.Threading.Tasks;
//using MongoDB.Driver.Linq;
//using TEDAMgrAPI.Models.UserLogin;

//namespace TEDAMgrAPI.Repository.Interface {
//    public interface IUsersRepository
//    {
//        IMongoQueryable<UserWithSecrete> GetUsers();
//        IMongoQueryable<UserWithSecrete> GetUsers(string userId, string employeeId);
//        Task<UserWithSecrete> GetUserByIdAsync(string userId);
//        Task<UserWithSecrete> CreateUserAsync(UserCreateRequest userCreateRequest);
//        Task UpdateUserAsync(UserWithSecrete userWithSecrete);
//        Task<(DateTime deleteDateTime, UserWithSecrete userWithSecrete)> DeleteUserAsync(string userId);
//        IMongoQueryable<UserWithSecrete> GetUsersForRole(string role, bool isDisabled = false);
//    }
//}