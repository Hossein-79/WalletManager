using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WalletManager.Models;

namespace WalletManager.Services
{
    public class TimedUpdateCoinPrice : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly double _jobTimer = 60;

        public TimedUpdateCoinPrice(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        private async void DoWork(object state)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var coinPriceService = scope.ServiceProvider.GetRequiredService<ICoinPriceService>();

            var allCoins = await coinPriceService.GetAllCoins();


            foreach (var item in allCoins ?? Enumerable.Empty<CoinPrice>())
            {

            }

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_jobTimer));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
