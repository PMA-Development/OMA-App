﻿namespace OMA_App.API
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using OMA_App.Storage;

    public partial class OMAClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private static readonly Lazy<JsonSerializerSettings> _settings = new Lazy<JsonSerializerSettings>(CreateSerializerSettings);
        private readonly string _jwtToken;

        public OMAClient(string baseUrl, HttpClient httpClient)
        {
            _baseUrl = baseUrl.EndsWith("/") ? baseUrl : $"{baseUrl}/";
            _httpClient = httpClient;
            Initialize();
        }

        private static JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            UpdateJsonSerializerSettings(settings);
            return settings;
        }



        protected virtual JsonSerializerSettings JsonSerializerSettings => _settings.Value;

        private HttpRequestMessage CreateRequest(HttpMethod method, string path, string query = "")
        {
            var request = new HttpRequestMessage(method, $"{_baseUrl}{path}{query}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //TODO: add authorization to the entire APP
            //var token = TokenService.GetAccessTokenAsync().GetAwaiter().GetResult();

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return request;


        }

        private async Task<T> SendAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    throw new ApiException($"Unexpected HTTP status code: {response.StatusCode}", (int)response.StatusCode, responseData, null, null);
                }

                var responseStream = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseStream, JsonSerializerSettings);
            }
        }

        // Attribute Endpoints
        public Task<AttributeDTO> GetAttributeAsync(int id, CancellationToken cancellationToken = default) =>
            SendAsync<AttributeDTO>(CreateRequest(HttpMethod.Get, $"api/Attribute/get-Attribute?id={id}"), cancellationToken);

        public Task<ICollection<AttributeDTO>> GetAttributesAsync(CancellationToken cancellationToken = default) =>
            SendAsync<ICollection<AttributeDTO>>(CreateRequest(HttpMethod.Get, "api/Attribute/get-Attributes"), cancellationToken);

        public Task<ICollection<AttributeDTO>> GetAttributeDataByTurbineIdAsync(int id, CancellationToken cancellationToken = default) =>
            SendAsync<ICollection<AttributeDTO>>(CreateRequest(HttpMethod.Get, $"api/Attribute/get-AttributeDataByTurbineId?id={id}"), cancellationToken);

        // Drone Endpoints
        public Task<DroneDTO> GetDroneAsync(int id, CancellationToken cancellationToken = default) =>
            SendAsync<DroneDTO>(CreateRequest(HttpMethod.Get, $"api/Drone/get-Drone?id={id}"), cancellationToken);

        public Task<ICollection<DroneDTO>> GetDronesAsync(CancellationToken cancellationToken = default) =>
            SendAsync<ICollection<DroneDTO>>(CreateRequest(HttpMethod.Get, "api/Drone/get-Drones"), cancellationToken);

        // Island Endpoints
        public Task<IslandDTO> GetIslandAsync(int id, CancellationToken cancellationToken = default) =>
            SendAsync<IslandDTO>(CreateRequest(HttpMethod.Get, $"api/Island/get-Island?id={id}"), cancellationToken);

        public Task<ICollection<IslandDTO>> GetIslandsAsync(CancellationToken cancellationToken = default) =>
            SendAsync<ICollection<IslandDTO>>(CreateRequest(HttpMethod.Get, "api/Island/get-Islands"), cancellationToken);

        // Task Endpoints
        public Task<TaskDTO> GetTaskAsync(int id, CancellationToken cancellationToken = default) =>
            SendAsync<TaskDTO>(CreateRequest(HttpMethod.Get, $"api/Task/get-Task?id={id}"), cancellationToken);

        public Task<ICollection<TaskDTO>> GetCompletedTasksAsync(CancellationToken cancellationToken = default) =>
            SendAsync<ICollection<TaskDTO>>(CreateRequest(HttpMethod.Get, "api/Task/get-Completed-Tasks"), cancellationToken);

        public Task<ICollection<TaskDTO>> GetUncompletedTasksAsync(CancellationToken cancellationToken = default) =>
            SendAsync<ICollection<TaskDTO>>(CreateRequest(HttpMethod.Get, "api/Task/get-Uncompleted-Tasks"), cancellationToken);

        public Task<ICollection<TaskDTO>> GetUserTasksAsync(Guid id, CancellationToken cancellationToken = default) =>
            SendAsync<ICollection<TaskDTO>>(CreateRequest(HttpMethod.Get, $"api/Task/get-User-Tasks?id={id}"), cancellationToken);

        public async Task<int> AddTaskAsync(TaskDTO task, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Post, "api/Task/add-Task");
            request.Content = new StringContent(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");
            return await SendAsync<int>(request, cancellationToken);
        }

        public async Task UpdateTaskAsync(TaskDTO task, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Put, "api/Task/update-Task");
            request.Content = new StringContent(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");
            await SendAsync<string>(request, cancellationToken);
        }

        // Turbine Endpoints
        public Task<TurbineDTO> GetTurbineAsync(int id, CancellationToken cancellationToken = default) =>
            SendAsync<TurbineDTO>(CreateRequest(HttpMethod.Get, $"api/Turbine/get-Turbine?id={id}"), cancellationToken);

        public Task<ICollection<TurbineDTO>> GetTurbinesAsync(CancellationToken cancellationToken = default) =>
            SendAsync<ICollection<TurbineDTO>>(CreateRequest(HttpMethod.Get, "api/Turbine/get-Turbines"), cancellationToken);

        public Task<ICollection<TurbineDTO>> GetTurbinesIslandAsync(int id, CancellationToken cancellationToken = default) =>
            SendAsync<ICollection<TurbineDTO>>(CreateRequest(HttpMethod.Get, $"api/Turbine/get-Turbines-Island?id={id}"), cancellationToken);

        public async Task ActionTurbineAsync(string action, int value, TurbineDTO turbine, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Post, "api/Turbine/action-Turbine", $"?action={action}&value={value}");
            request.Content = new StringContent(JsonConvert.SerializeObject(turbine), Encoding.UTF8, "application/json");
            await SendAsync<string>(request, cancellationToken);
        }

        // User Endpoints
        public Task<ICollection<UserDTO>> GetUsersAsync(CancellationToken cancellationToken = default) =>
            SendAsync<ICollection<UserDTO>>(CreateRequest(HttpMethod.Get, "api/User/get-Users"), cancellationToken);



        protected static void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
        {

            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.None;
        }


        partial void Initialize();
    }

    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string Response { get; }

        public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Response = response;
        }

        public override string ToString() => $"HTTP Response:\n\n{Response}\n\n{base.ToString()}";
    }



    // DTO classes

    public partial class AttributeDTO
    {
        [JsonProperty("attributeID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int AttributeID { get; set; }

        [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("value", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("deviceDataID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int DeviceDataID { get; set; }
    }

    public partial class TaskDTO
    {
        [JsonProperty("taskID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int TaskID { get; set; }

        [JsonProperty("title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("level", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public LevelEnum Level { get; set; }

        [JsonProperty("finishDescription", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FinishDescription { get; set; }

        [JsonProperty("isCompleted", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool IsCompleted { get; set; }

        [JsonProperty("ownerID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Guid OwnerID { get; set; }

        [JsonProperty("userID", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? UserID { get; set; }

        [JsonProperty("turbineID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int TurbineID { get; set; }
    }

    public partial class DroneDTO
    {
        [JsonProperty("droneID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int DroneID { get; set; }

        [JsonProperty("title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("available", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool Available { get; set; }

        [JsonProperty("taskID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int TaskID { get; set; }
    }

    public partial class IslandDTO
    {
        [JsonProperty("islandID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int IslandID { get; set; }

        [JsonProperty("title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("clientID", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ClientID { get; set; }

        [JsonProperty("abbreviation", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
    }

    public partial class TurbineDTO
    {
        [JsonProperty("turbineID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int TurbineID { get; set; }

        [JsonProperty("title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("clientID", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ClientID { get; set; }

        [JsonProperty("islandID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int IslandID { get; set; }
    }

    public partial class UserDTO
    {
        [JsonProperty("userID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Guid UserID { get; set; }

        [JsonProperty("fullName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("phone", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }
    }

    public partial class DeviceDTO
    {
        [JsonProperty("deviceId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int DeviceId { get; set; }

        [JsonProperty("type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("clientID", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ClientID { get; set; }

        [JsonProperty("state", Required = Required.Always)]
        public StateEnum State { get; set; }

        [JsonProperty("turbineID", Required = Required.Always)]
        public int TurbineID { get; set; }
    }

    public partial class DeviceDataDTO
    {
        [JsonProperty("deviceDataID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int DeviceDataID { get; set; }

        [JsonProperty("type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("timestamp", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("deviceID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int DeviceID { get; set; }
    }

    public enum LevelEnum
    {
        _1 = 1,
        _2 = 2,
        _3 = 3
    }

    public enum StateEnum
    {
        _0 = 0,
        _1 = 1,
        _2 = 2,
        _3 = 3
    }
}
