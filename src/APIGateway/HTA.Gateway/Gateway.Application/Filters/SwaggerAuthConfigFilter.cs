using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Readers;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.Application
{
    /// <summary>
    /// Lớp này là một bộ lọc cấu hình proxy của YARP (Yet Another Reverse Proxy).
    /// Nó được sử dụng để tự động áp dụng các chính sách ủy quyền cho các tuyến đường (routes)
    /// dựa trên thông tin từ tài liệu Swagger (OpenAPI) của các dịch vụ backend.
    /// </summary>
    public class SwaggerAuthConfigFilter : IProxyConfigFilter
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger<SwaggerAuthConfigFilter> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Hàm khởi tạo cho SwaggerAuthConfigFilter.
        /// </summary>
        /// <param name="logger">Đối tượng logger để ghi log.</param>
        /// <param name="configuration">Đối tượng configuration để truy cập cấu hình ứng dụng.</param>
        public SwaggerAuthConfigFilter(ILogger<SwaggerAuthConfigFilter> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Cấu hình một cụm (cluster) proxy. Phương thức này không làm gì trong bộ lọc này.
        /// </summary>
        /// <param name="cluster">Cấu hình cụm hiện tại.</param>
        /// <param name="cancel">Mã thông báo hủy.</param>
        /// <returns>Cấu hình cụm đã được sửa đổi (hoặc không).</returns>
        public async ValueTask<ClusterConfig> ConfigureClusterAsync(ClusterConfig cluster, CancellationToken cancel)
        {
            // Không có thay đổi nào được thực hiện cho cấu hình cụm.
            await Task.CompletedTask;
            return cluster;
        }

        /// <summary>
        /// Cấu hình một tuyến đường (route) proxy.
        /// Phương thức này sẽ kiểm tra tài liệu Swagger của dịch vụ backend được liên kết với tuyến đường.
        /// Nếu bất kỳ hoạt động nào trong tài liệu Swagger yêu cầu bảo mật, nó sẽ áp dụng chính sách ủy quyền "RequireAuth" cho tuyến đường.
        /// </summary>
        /// <param name="route">Cấu hình tuyến đường hiện tại.</param>
        /// <param name="cluster">Cấu hình cụm được liên kết với tuyến đường.</param>
        /// <param name="cancel">Mã thông báo hủy.</param>
        /// <returns>Cấu hình tuyến đường đã được sửa đổi (hoặc không).</returns>
        public async ValueTask<RouteConfig> ConfigureRouteAsync(
            RouteConfig route,
            ClusterConfig? cluster,
            CancellationToken cancel)
        {
            // Nếu không có cụm nào được liên kết với tuyến đường này, hãy bỏ qua.
            if (cluster == null)
                return route;

            // Lấy URL Swagger từ cấu hình thay vì hardcode.
            // Đây là best practice để tránh hardcode URL trong mã nguồn.
            // Bạn nên thêm các URL này vào tệp appsettings.json của mình.
            // Ví dụ:
            // "SwaggerEndpoints": {
            //   "accountCluster": "https://localhost:5001/swagger/v1/swagger.json",
            //   "productCluster": "https://localhost:5002/swagger/v1/swagger.json"
            // }
            string? serviceSwaggerUrl = _configuration[$"SwaggerEndpoints:{cluster.ClusterId}"];

            // Nếu không tìm thấy URL Swagger cho cụm này, hãy bỏ qua.
            if (string.IsNullOrEmpty(serviceSwaggerUrl))
            {
                _logger.LogWarning($"Không tìm thấy URL Swagger cho cụm: {cluster.ClusterId}");
                return route;
            }

            try
            {
                // Tải tài liệu Swagger từ URL.
                using var stream = await _httpClient.GetStreamAsync(serviceSwaggerUrl, cancel);
                var openApiDoc = new OpenApiStreamReader().Read(stream, out var diagnostic);

                // Lặp lại qua tất cả các đường dẫn và hoạt động trong tài liệu Swagger.
                foreach (var path in openApiDoc.Paths)
                {
                    foreach (var op in path.Value.Operations)
                    {
                        // Nếu một hoạt động yêu cầu bảo mật, hãy áp dụng chính sách ủy quyền.
                        if (op.Value.Security != null && op.Value.Security.Count > 0)
                        {
                            _logger.LogInformation($"🔒 Đường dẫn {path.Key} yêu cầu xác thực → áp dụng chính sách.");
                            // Trả về một cấu hình tuyến đường mới với chính sách ủy quyền được áp dụng.
                            return route with { AuthorizationPolicy = "RequireAuth" };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu không thể tải hoặc phân tích tài liệu Swagger.
                _logger.LogError(ex, $"Không thể tải swagger cho {cluster.ClusterId}: {ex.Message}");
            }

            // Trả về tuyến đường ban đầu nếu không có yêu cầu bảo mật nào được tìm thấy.
            return route;
        }
    }
}