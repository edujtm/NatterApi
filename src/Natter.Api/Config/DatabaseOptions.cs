
namespace Natter.Api.Config;


public class DatabaseOptions
{
    public string? User { get; set; }
    public string? Database { get; set; }
    public string? Host { get; set; }

    public string? Port { get; set; }
    public string? Password { get; set; }


    public string ConnectionString
        => $"User ID={this.User};Password={this.Password};Database={this.Database};Host={this.Host};Port={this.Port}";
}
