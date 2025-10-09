using domain.Entities;
using domain.Interfaces;
using Microsoft.Extensions.Configuration;
using NATS.Client.Core;


namespace infrastructure.Adapters
{
    public class NatsManagementAdapter : INatsManagement
    {

        private readonly IConfiguration _config;
        public NatsManagementAdapter(IConfiguration config) 
        {
            _config = config;
        }

        public async Task SendMessage(Template template)
        {
            try
            {
                var opts = new NatsOpts { Url = _config.GetSection("natsurl").Value };
                var nats = new NatsConnection(opts);                
                await nats.PublishAsync(_config.GetSection("subject").Value, template.id);
            }
            catch (Exception e) 
            {
                e.Message.ToString();
            }
        }

    }
}
