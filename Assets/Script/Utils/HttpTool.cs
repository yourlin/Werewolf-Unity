using System;
using System.Net.Http;
using System.Threading.Tasks;

public class HttpTool
{
	static readonly HttpClient _client = new HttpClient ();

	public static async Task<string> getRemoteMessage (string url) {
		try {
			// 发送GET请求
			HttpResponseMessage response = await _client.GetAsync (url);

			// 确保请求成功
			response.EnsureSuccessStatusCode ();

			// 读取响应内容
			string responseBody = await response.Content.ReadAsStringAsync ();

			// 返回响应数据
			return responseBody;
		} catch (HttpRequestException ex) {
			// 处理HTTP请求异常
			Console.WriteLine ($"Error: {ex.Message}");
			return null;
		}
	}
}
