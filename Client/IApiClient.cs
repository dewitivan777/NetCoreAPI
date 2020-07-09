using Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    public interface IApiClient
    {
        Task<IApiResponse<T>> CreateAsync<T, TContent>(
            string baseUrl,
            TContent content, 
            string pathWithQuery, 
            string token = null, 
            AuditInfo audit = null) where T : class where TContent : class;

        Task<IApiResponse<T>> DeleteAsync<T>(
            string baseUrl,
            string pathWithQuery, 
            string token = null, 
            AuditInfo audit = null) where T : class;

        Task<IApiResponse<T>> EditAsync<T, TContent>(
            string baseUrl,
            TContent content, 
            string pathWithQuery, 
            string token = null, 
            AuditInfo audit = null) where T : class where TContent : class;

        Task<IApiResponse<T>> GetAsync<T>(
            string baseUrl,
            string pathWithQuery, 
            string token = null, 
            string userClaim = null) where T : class;

        Task<IApiResponse<T>> GetByQueryAsync<T>(
            string baseUrl,
            string pathWithQuery, 
            string token = null, 
            params KeyValuePair<string, string>[] queryValues) where T : class;
    }
}