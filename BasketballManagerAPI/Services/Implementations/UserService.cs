using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.UserDto;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using BasketballManagerAPI.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Dto;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Azure.Core;
using BasketballManagerAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Security.Services.Interfaces;

namespace BasketballManagerAPI.Services.Implementations {
    public class UserService : IUserService {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly SecurityHttpClientConstants _securityHttpClientConstants;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UserService> _logger;
        private readonly IFileService _fileService;

        public UserService(ApplicationDbContext context, IMapper mapper, IHttpClientFactory httpClientFactory,
        IOptions<SecurityHttpClientConstants> securityHttpClientConstants, ICurrentUserService currentUserService, ILogger<UserService> logger, IFileService fileService) {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _securityHttpClientConstants = securityHttpClientConstants.Value;
            _httpClient = httpClientFactory.CreateClient(_securityHttpClientConstants.ClientName);
            _logger = logger;
            _fileService = fileService; 
        }

        public async Task<PagedList<UserResponseDto>> GetAllUsersAsync(UserFiltersDto userFiltersDto, CancellationToken cancellationToken = default)
        {

            IQueryable<User> userQuery = _context.Users.AsNoTracking();
            userQuery = ApplyFilters(userQuery, userFiltersDto);
            userQuery = userFiltersDto.SortOrder?.ToLower() == "desc"
                ? userQuery.OrderByDescending(GetSortProperty(userFiltersDto.SortColumn))
                : userQuery.OrderBy(GetSortProperty(userFiltersDto.SortColumn));


            var users = await PagedList<User>.CreateAsync(
                userQuery,
                userFiltersDto.Page,
                userFiltersDto.PageSize,
                cancellationToken);
            return _mapper.Map<PagedList<UserResponseDto>>(users);


        }

        public async Task<UserResponseDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default) {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"User with id {id} does not exist!");
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<bool> IsUserExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<UserResponseDto> RegisterUserAsync(UserSignUpDto userSignUpDto,
            CancellationToken cancellationToken = default, bool isAdmin = false) {
            var jsonContent = JsonSerializer.Serialize(userSignUpDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var url = _httpClient.BaseAddress.ToString();
            if (isAdmin) {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    _currentUserService.AccessTokenRaw);
                url += _securityHttpClientConstants.RegisterAdminEndpoint;
            } else {
                url += _securityHttpClientConstants.RegisterEndpoint;
            }

            var httpResponse = await _httpClient.PostAsync(url, content, cancellationToken);
            if (httpResponse.StatusCode != HttpStatusCode.OK) {
                var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                var problemDetails = await HandleHttpResponse<ProblemDetails>(httpResponse, cancellationToken);

                
                var headersDictionary = httpResponse.Headers
                    .Concat(httpResponse.Content.Headers)
                    .GroupBy(h => h.Key)
                    .ToDictionary(g => g.Key, g => g.First().Value);

                throw new ApiException(problemDetails.Detail, (int)httpResponse.StatusCode, responseContent, headersDictionary);
            }

            var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("Response body: {r}", responseBody);

            var securityUserResponseDto = await HandleHttpResponse<UserSecurityResponseDto>(httpResponse, cancellationToken);
            _logger.LogInformation("Security response: {r}", JsonSerializer.Serialize(securityUserResponseDto));

            var userToAdd = _mapper.Map<User>(securityUserResponseDto);
            var createdUser = await _context.AddAsync(userToAdd, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserResponseDto>(createdUser.Entity);
        }
        public async Task ConfirmEmail(Guid userId, string token, CancellationToken cancellationToken = default)
        {
            if (!await IsUserExistsAsync(userId, cancellationToken))
                throw new NotFoundException($"User with id {userId} does not exist!");
            var url = _httpClient.BaseAddress + _securityHttpClientConstants.ConfirmEmailEndpoint +
                      $"?userId={userId}&token={token}";
            var httpResponse = await _httpClient.GetAsync(url, cancellationToken);

            _logger.LogInformation("{b}, {s}",
                await httpResponse.Content.ReadAsStringAsync(cancellationToken),
                httpResponse.StatusCode);

            if (httpResponse.StatusCode != HttpStatusCode.OK) {
                var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                var problemDetails = await HandleHttpResponse<ProblemDetails>(httpResponse, cancellationToken);
                var headersDictionary = httpResponse.Headers
                    .Concat(httpResponse.Content.Headers)
                    .GroupBy(h => h.Key)
                    .ToDictionary(g => g.Key, g => g.First().Value);

                throw new ApiException(problemDetails.Detail, (int)httpResponse.StatusCode, responseContent, headersDictionary);
            }


            var userToConfirmEmail = await _context.Users
                .FirstAsync(u => u.Id == userId, cancellationToken);

            userToConfirmEmail.EmailConfirmed = true;

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default) {
            if (!await IsUserExistsAsync(id, cancellationToken))
                throw new NotFoundException($"User with id {id} does not exist!");
            if (!await CheckRoleViolationsForUpdate(id, cancellationToken))
                throw new ApiException("Forbidden to update the user", StatusCodes.Status403Forbidden);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                _currentUserService.AccessTokenRaw);

            var jsonContent = JsonSerializer.Serialize(userUpdateDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, ContentType.ApplicationJson.ToString());

            var url = _httpClient.BaseAddress + _securityHttpClientConstants.UpdateEndpoint + $"/{id}";

            var httpResponse = await _httpClient.PatchAsync(url, content, cancellationToken);

            if (httpResponse.StatusCode != HttpStatusCode.NoContent)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                var problemDetails = await HandleHttpResponse<ProblemDetails>(httpResponse, cancellationToken);
                var headersDictionary = httpResponse.Headers
                    .Concat(httpResponse.Content.Headers)
                    .GroupBy(h => h.Key)
                    .ToDictionary(g => g.Key, g => g.First().Value);

                throw new ApiException(problemDetails.Detail, (int)httpResponse.StatusCode, responseContent, headersDictionary);
            }


            var userFounded = await _context.Users.FirstAsync(u => u.Id == id, cancellationToken);
            userFounded.FirstName = userUpdateDto.FirstName;
            userFounded.LastName = userUpdateDto.LastName;

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default) {
            if (!await IsUserExistsAsync(id, cancellationToken))
                throw new NotFoundException($"User with id {id} does not exist!");
            if (!await CheckRoleViolationsForDelete(id, cancellationToken))
                throw new ApiException("Forbidden to delete user", StatusCodes.Status403Forbidden);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                _currentUserService.AccessTokenRaw);
            var url = _httpClient.BaseAddress + _securityHttpClientConstants.DeleteEndpoint + $"/{id}";
            var httpResponse = await _httpClient.DeleteAsync(url, cancellationToken);

            _logger.LogInformation("Response: Status {s}, Body: {b}",
                httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));

            if (httpResponse.StatusCode != HttpStatusCode.NoContent)
            {
                 var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                var problemDetails = await HandleHttpResponse<ProblemDetails>(httpResponse, cancellationToken);
                var headersDictionary = httpResponse.Headers
                    .Concat(httpResponse.Content.Headers)
                    .GroupBy(h => h.Key)
                    .ToDictionary(g => g.Key, g => g.First().Value);

                throw new ApiException(problemDetails.Detail, (int)httpResponse.StatusCode, responseContent, headersDictionary);
            }
               

            var userToDelete = await _context.Users
                .FirstAsync(e => e.Id == id, cancellationToken);

            _context.Remove(userToDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<bool> CheckRoleViolationsForUpdate(Guid userToUpdateId, CancellationToken cancellationToken = default) {
            var currentRole = _currentUserService.UserRole;
            var currentUserId = _currentUserService.UserId;
            var userToUpdateRole = (await _context.Users
                    .FirstAsync(u => u.Id == userToUpdateId, cancellationToken))
                .Role.ToString();

            switch (currentRole) {
                case "User" when userToUpdateRole != "User" ||
                                 currentUserId != userToUpdateId.ToString():
                case "Admin" when userToUpdateRole == "SuperAdmin":
                case "Admin" when userToUpdateRole == "Admin" &&
                                  currentUserId != userToUpdateId.ToString():
                    return false;
                default:
                    return true;
            }
        }

        private async Task<bool> CheckRoleViolationsForDelete(Guid userToDeleteId,
            CancellationToken cancellationToken = default)
        {
            var userToDeleteRole = (await _context.Users
                    .FirstAsync(u => u.Id == userToDeleteId, cancellationToken))
                .Role.ToString();

            switch (userToDeleteRole)
            {
                case "SuperAdmin":
                case "Admin" when _currentUserService.UserRole != "SuperAdmin":
                    return false;
                default:
                    return true;
            }
        }

        private static async Task<T> HandleHttpResponse<T>(HttpResponseMessage httpResponse,
            CancellationToken cancellationToken = default) {
            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
            var securityUserResponseDto = await JsonSerializer.DeserializeAsync<T>(
                stream,
                new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                },
                cancellationToken
            );

            return securityUserResponseDto;
        }





        public async Task UpdateUserAvatarAsync(Guid id, IFormFile picture, CancellationToken cancellationToken = default) {
            var foundedUser = await _context.Users.FindAsync(id, cancellationToken);
            if (foundedUser == null)
                throw new NotFoundException($"User with ID {id} not found.");
            if (!await CheckRoleViolationsForUpdate(id, cancellationToken))
                throw new ApiException("Forbidden to update the user", StatusCodes.Status403Forbidden);
            if (picture.Length <= 0 || picture == null)
                throw new BadRequestException("Uploaded  image is  empty!");
            await _fileService.RemoveFilesByNameIfExistsAsync(fileName: id.ToString(), cancellationToken);
            var filePath = await _fileService.StoreFileAsync(picture, fileName: id.ToString(), cancellationToken);

            foundedUser.PhotoPath = filePath;

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<FileDto> DownloadUserAvatarAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedUser = await _context.Users.FindAsync(id, cancellationToken);
            if (foundedUser == null)
                throw new NotFoundException($"User with ID {id} not found.");
            var filePath = foundedUser.PhotoPath;

            if (filePath.IsNullOrEmpty())
                throw new NotFoundException($"User with ID {id} has not  this file path!");

            var fileDto = await _fileService.GetFileAsync(filePath, cancellationToken);
            return fileDto;
        }


        private IQueryable<User> ApplyFilters(IQueryable<User> query, UserFiltersDto userFiltersDto)
        {
            if (!string.IsNullOrEmpty(userFiltersDto.Email))
                query = query.Where(u => u.Email == userFiltersDto.Email);
            if (!string.IsNullOrEmpty(userFiltersDto.LastName))
                query = query.Where(u => u.LastName == userFiltersDto.LastName);
            if (!string.IsNullOrEmpty(userFiltersDto.Role) &&
                Enum.TryParse<Role>(userFiltersDto.Role, true, out var userRole)) {
                query = query.Where(u => u.Role == userRole);
            }
            return query;
        }
        private Expression<Func<User, object>> GetSortProperty(string? sortColumn) {
            return sortColumn?.ToLower() switch {
                "lastname" => u => u.LastName,
                "firstname" => u => u.FirstName,

                "role" => p => p.Role,
                "balance" => p => p.Balance,
                "createddate" => p => p.CreatedTime,
                _ => p => p.Id
            };
        }
    }
}
